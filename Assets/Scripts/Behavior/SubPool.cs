using UnityEngine;
using System.Collections.Generic;

public class Subpool : Behavior {

  public GameObject template;

  protected Stack<GameObject> _prefabs;

  public int size {
    get {
      // check if prefab stack is null
      if (this._prefabs == null) {
        Debug.LogError("Trying to get size of null prefab stack.");
        return 0;
      }

      // otherwise, return the count of the prefab stack
      return this._prefabs.Count;
    }
  }

  public bool Populate(int num = 1) {
    // if the template is null, don't do anything
    if (this.template == null) {
      Debug.LogError("No template found.");
      return false;
    }

    // otherwise, spawn as many copies as specified
    for (int i = 0; i < num; i++) {
      GameObject instance = Instantiate(this.template) as GameObject;

      // check that the object was instantiated successfully
      if (instance == null) {
        Debug.LogError("Could not instantiate instance. Refusing to populate further.");
        return false;
      }

      // parent the instantiated object to the pool object
      instance.transform.name = this.template.name;
      instance.transform.SetParent(this.transform, false);
      instance.SetActive(false);

      // add the instantiated object to the prefab stack
      this._prefabs.Push(instance);
    }

    // if everything populated successfully, return true
    return true;
  }

  public virtual GameObject Spawn() {
    // if there are no prefabs in the stack, populate it
    if (this.size == 0) {
      bool populated = this.Populate();

      // if it failed to populate, return null
      if (!populated) {
        Debug.LogError("Failed to populate empty pool when requesting instance.");
        return null;
      }
    }

    // pop the last instance
    GameObject instance = this._prefabs.Pop();

    // unparent it from its default parent before returning
    instance.transform.SetParent(null);

    return instance;
  }

  public bool Reclaim(GameObject obj) {
    // check if obj is null, and if so then do nothing
    if (obj == null) {
      Debug.LogError("Object to reclaim was null.");
      return false;
    }

    // otherwise, parent it to the pool object
    obj.transform.SetParent(this.transform, false);

    // deactivate it
    obj.SetActive(false);

    // put it back onto the prefab stack
    this._prefabs.Push(obj);

    return true;
  }

  protected override void AwakeBehavior() {
    base.AwakeBehavior();

    // initialize prefab stack
    this._prefabs = new Stack<GameObject>();
  }

}
