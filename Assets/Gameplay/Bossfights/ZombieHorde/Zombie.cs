using DG.Tweening;
using UnityEngine;

public class Zombie : Damageable {
    public Gametick tick;
    public float health {get; private set;} = 1f;
    public Player player;
    bool attackQueued = false;

    public override void Damage(float amt) {
        health -= amt;
    }

    void Start() {
        tick.onEnemyTick += EnemyTick;
    }

    void EnemyTick() {
        if (attackQueued) {
            attackQueued = false;
            if ((player.gridPos - gridPos).DNDLength() <= 1) {
                player.Damage(0.25f);
            }
        }

        else if ((gridPos - player.gridPos).DNDLength() <= 1) {
            attackQueued = true;
        }

        else {
            (float, Vector2Int) best = (int.MaxValue, Vector2Int.zero);

            if ((gridPos + Vector2Int.up - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.up - player.gridPos).magnitude, gridPos + Vector2Int.up);

            if ((gridPos + Vector2Int.left - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.left - player.gridPos).magnitude, gridPos + Vector2Int.left);

            if ((gridPos + Vector2Int.down - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.down - player.gridPos).magnitude, gridPos + Vector2Int.down);

            if ((gridPos + Vector2Int.right - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.right - player.gridPos).magnitude, gridPos + Vector2Int.right);

                if ((gridPos + Vector2Int.up + Vector2Int.left - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.up + Vector2Int.left - player.gridPos).magnitude, gridPos + Vector2Int.up + Vector2Int.left);

            if ((gridPos + Vector2Int.left + Vector2Int.down - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.left + Vector2Int.down - player.gridPos).magnitude, gridPos + Vector2Int.left + Vector2Int.down);

            if ((gridPos + Vector2Int.down + Vector2Int.right - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.down + Vector2Int.right - player.gridPos).magnitude, gridPos + Vector2Int.down + Vector2Int.right);

            if ((gridPos + Vector2Int.right + Vector2Int.up - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.right + Vector2Int.up - player.gridPos).magnitude, gridPos + Vector2Int.right + Vector2Int.up);
            
            transform.DOMove(best.Item2.GridToWorld(grid.boardSize, transform.position.y), 0.1f);
        }
    }
}
