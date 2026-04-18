using DG.Tweening;
using UnityEngine;

public class Zombie : GridObject, Damageable {
    public Gametick tick;
    public float health {get; protected set;} = 3f;
    public Player player;
    bool attackQueued = false;
    public HordeController controller;

    public void Damage(float amt) {
        health -= amt;
        controller.ZombieHit(this);
    }

    new void Start() {
        tick.onEnemyTick += Tick;
        base.Start();
    }

    void Tick() {

        Debug.Log("Zombie's Tick");
        if (attackQueued) {
            attackQueued = false;
            if ((player.gridPos - gridPos).ChebyshevLength() <= 1) {
                player.Damage(0.25f);
            }
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        else if ((gridPos - player.gridPos).ChebyshevLength() <= 1) {
            attackQueued = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.darkRed;
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

            /*
            if ((gridPos + Vector2Int.up + Vector2Int.left - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.up + Vector2Int.left - player.gridPos).magnitude, gridPos + Vector2Int.up + Vector2Int.left);

            if ((gridPos + Vector2Int.left + Vector2Int.down - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.left + Vector2Int.down - player.gridPos).magnitude, gridPos + Vector2Int.left + Vector2Int.down);

            if ((gridPos + Vector2Int.down + Vector2Int.right - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.down + Vector2Int.right - player.gridPos).magnitude, gridPos + Vector2Int.down + Vector2Int.right);

            if ((gridPos + Vector2Int.right + Vector2Int.up - player.gridPos).magnitude < best.Item1) 
                best = ((gridPos + Vector2Int.right + Vector2Int.up - player.gridPos).magnitude, gridPos + Vector2Int.right + Vector2Int.up); */
            
            Move(best.Item2, 0.1f, Ease.Linear);
        }
    }

    public override bool Collide(GridObject obj)
    {
        throw new System.NotImplementedException();
    }
}
