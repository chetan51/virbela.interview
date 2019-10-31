using UnityEngine;

namespace Interview {

  public class Thing : Behavior {

    #region Unity References

    [SerializeField]
    protected MeshRenderer _meshRenderer;

    #endregion

    #region Properties & Variables

    public Color color {
      get {
        return this._color;
      }
      set {
        this._color = value;

        // set material color
        this._meshRenderer.material.color = this.color;
      }
    }
    protected Color _color;

    #endregion

  }

}

