using UnityEngine;

public abstract class KnownPosition : MonoBehaviour {
    public Vector2Int gridPos {
        get {
            return (transform.position.CollapseXZ() + registry.boardSize / 2).Floor();
        }
    }
    public Vector2Int size;
    public int registryIndex;

    public GridRegistry registry;
}
