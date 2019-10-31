using UnityEngine;

namespace Interview {

  public class ThingManager : Singleton<ThingManager> {

    #region Unity References

    [SerializeField]
    protected Thing[] _things;

    [SerializeField]
    protected GameObject _target;

    #endregion

    #region Protected Methods

    protected void ResetThingColors() {
      // reset colors of all the things
      foreach (Thing thing in this._things) {
        thing.color = Color.white;
      }
    }

    protected void MakeClosestThingRed(GameObject targetObject) {
      // reset colors of all the things
      this.ResetThingColors();

      // find the closest thing
      Thing closestThing = null;
      float clostestDistance = Mathf.Infinity;
      foreach (Thing thing in this._things) {
        float distance = (thing.transform.position - targetObject.transform.position).magnitude;

        if (distance < clostestDistance) {
          closestThing = thing;
          clostestDistance = distance;
        }
      }

      // if no thing was found, log an error and do nothing more
      if (closestThing == null) {
        Debug.LogError("No closest thing was found.");
        return;
      }

      // make the closest thing red
      closestThing.color = Color.red;
    }

    #endregion

    #region Behavior Lifecycle

    protected void Update() {
      this.MakeClosestThingRed(this._target);
    }

    #endregion

  }

}

