public class PooledBehavior : Behavior {

  public string poolName { get; set; }

  public bool previouslySpawned { get; set; }

  // Called after OnEnable, before OnSpawn (but only the first time it is spawned)
  public virtual void OnFirstSpawn() {
    // mark behavior as previously spawned
    this.previouslySpawned = true;
  }

  // Called after OnEnable, before the spawned object is returned by the Pool
  public virtual void OnSpawn() {}

  public virtual void OnReclaim() {}

  protected override void Start() {
    // behavior might not have been spawned yet (if it wasn't created in code), so spawn it if necessary
    // NOTE: this can happen when you drag the prefab into the scene manually, or baked it into the scene
    if (this.previouslySpawned == false) {
      this.OnFirstSpawn();
      this.OnSpawn();
    }

    base.Start();
  }

}
