using UnityEngine;

public class Behavior : MonoBehaviour {

  public void EnsureAwoken() {
    // if behavior hasn't been awoken yet
    if (this._awoken == false) {
      // awake it
      this.AwakeBehavior();

      // mark it as awoken
      this._awoken = true;
    }
  }

  // Override AwakeBehavior instead of Awake in subclasses
  private void Awake() {
    this.EnsureAwoken();
  }

  protected virtual void AwakeBehavior() {}

  protected virtual void OnEnable() {}

  protected virtual void Start() {}

  protected virtual void OnDisable() {}

  protected virtual void OnDestroy() {}

  protected virtual void Reset() {}

  protected virtual void OnValidate() {}

  // True if AwakeBehavior has been called
  protected bool _awoken;

}
