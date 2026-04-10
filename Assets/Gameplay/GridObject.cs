using UnityEngine;
using DG.Tweening;

public abstract class GridObject : MonoBehaviour {
    public Vector2Int gridPos;
    public Vector2Int size;
    public int registryIndex;

    public GridRegistry grid;

    public bool moving = false;


    protected void Update() {
        if (!moving) transform.position = grid.Convert(gridPos, transform.position.y);
    }

    public bool CheckCollision(GridObject target) {
        if (moving) return (grid.LosslessConvert(transform.position) - target.gridPos).magnitude <= 0.5;

        else return target.gridPos == gridPos;
    }

    public void Move(Vector2Int finalPos, float time, Ease easing) {
        grid.Move(this, grid.ForceInGrid(finalPos), time, easing);
    }
}
