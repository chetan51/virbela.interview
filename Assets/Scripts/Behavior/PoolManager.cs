using UnityEngine;
using System;
using System.Collections.Generic;

public class PoolManager : Singleton<PoolManager> {

  protected Dictionary<string, Subpool> _subpools;

  public GameObject Spawn(string type, bool active = true) {
    // try to get the subpool from the subpool cache
    Subpool subpool;
    this._subpools.TryGetValue(type, out subpool);

    // if no subpool exists
    if (subpool == null) {
      subpool = CreatePool(type);

      if (subpool == null) return null;
    }

    // ask the subpool to spawn a new object
    GameObject obj = subpool.Spawn();

    // set active status
    obj.SetActive(active);

    // get PooledBehavior from object
    PooledBehavior behavior = obj.GetComponent<PooledBehavior>();

    // if behavior exists
    if (behavior) {
      // if this is the first time spawning the object, trigger its OnFirstSpawn method
      if (behavior.previouslySpawned == false) behavior.OnFirstSpawn();

      // trigger its OnSpawn method
      behavior.OnSpawn();

      // set its pool name
      behavior.poolName = type;
    }

    return obj;
  }

  public ComponentType Spawn<ComponentType>(string type, bool active = true) {
    GameObject obj = this.Spawn(type, active);

    return obj.GetComponent<ComponentType>();
  }

  public GameObject SpawnAndAttach(string type, Transform parent, bool active = true) {
    // spawn an instance
    GameObject obj = this.Spawn(type, active);

    // parent it
    obj.transform.SetParent(parent, false);

    return obj;
  }

  public GameObject SpawnAndAttach(string type, GameObject parent, bool active = true) {
    return this.SpawnAndAttach(type, parent.transform, active);
  }

  public ComponentType SpawnAndAttach<ComponentType>(string type, GameObject parent, bool active = true) {
    GameObject obj = this.SpawnAndAttach(type, parent.transform, active);

    return obj.GetComponent<ComponentType>();
  }

  public bool Reclaim(string type, GameObject obj) {
    // try to get the subpool from the subpool cache
    Subpool subpool;
    this._subpools.TryGetValue(type, out subpool);

    // if no subpool exists
    if (subpool == null) {
      Debug.LogError(string.Format("Could not reclaim object of type {0} – no subpool exists.", type));
      return false;
    }

    // ask the subpool to reclaim the object
    bool result = subpool.Reclaim(obj);

    if (!result) {
      Debug.LogError(string.Format("Subpool could not reclaim object of type {0}.", type));
      return false;
    }

    // get PooledBehavior from object
    PooledBehavior behavior = obj.GetComponent<PooledBehavior>();

    // if behavior exists
    if (behavior) {
      // call events on it
      behavior.OnReclaim();
    }

    return true;
  }

  public bool Reclaim(PooledBehavior behavior) {
    // return behavior to the pool that it came from
    return this.Reclaim(behavior.poolName, behavior.gameObject);
  }

  protected Subpool CreatePool(string type) {
    // find the requested resource
    GameObject template;
    try {
      template = Instantiate(Resources.Load("Prefabs/" + type)) as GameObject;
    } catch (Exception exception) {
      // on error, log and return null
      Debug.LogError(string.Format("Could not instantiate resource of type: {0}. Exception: {1}", type, exception));
      return null;
    }

    // create a new subpool object and parent it to this gameObject
    GameObject subpoolObject = new GameObject();
    subpoolObject.transform.SetParent(this.transform);

    // rename it based on type
    subpoolObject.name = string.Format("{0}Pool", type);

    // add a Subpool component to it
    Subpool subpool = subpoolObject.AddComponent<Subpool>();

    // add the Subpool to the subpool dictionary
    this._subpools.Add(type, subpool);

    // disable the template
    template.SetActive(false);

    // parent it to the subpool
    template.transform.SetParent(subpoolObject.transform);

    // name it based on the type
    template.name = type;

    // set the subpool's template and return
    subpool.template = template;

    return subpool;
  }

  protected override void AwakeBehavior() {
    base.AwakeBehavior();

    this._subpools = new Dictionary<string, Subpool>();
  }

}
