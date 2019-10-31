using UnityEngine;

public class Singleton<TBehavior> : Behavior where TBehavior : Behavior {

	public static TBehavior instance {
		get {
      return GetInstance();
		}
	}

  public static bool exists {
    get {
      return _instance != null;
    }
  }

	protected static TBehavior _instance;

  public static void EnsureInitialized() {
    // try to get instance, which will initialize the instance if it hasn't been initialized already
    GetInstance();
  }

  #region Helpers

  protected static TBehavior GetInstance() {
    // if instance hasn't been set, find the object in the scene and set it
    if (_instance == null) {
      _instance = GameObject.FindObjectOfType<TBehavior>();

      // if the object in the scene couldn't be found, log an error and return null
      if (_instance == null) {
        Debug.LogError(string.Format("An instance of type {0} is needed in the scene, but none was found.", typeof(TBehavior)));
        return null;
      }

      // ensure that the singleton behavior has been awoken
      Singleton<TBehavior> instanceSingleton = _instance as Singleton<TBehavior>;
      instanceSingleton.EnsureAwoken();
    }

    return _instance;
  }

  #endregion

}
