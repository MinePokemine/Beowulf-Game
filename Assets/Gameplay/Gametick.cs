using UnityEngine;

public class Gametick : MonoBehaviour {
    public float tickLength = 1;

    public int ticks;
    float toNextTick = 0;

    public GameObject tickTimer;

    public delegate void OnGametick( );

    public OnGametick onPlayerTick = new(() => { });
    public OnGametick onEnemyTick = new(() => { });


    bool tickType;

    void Update() {
        toNextTick += Time.deltaTime;
        if (toNextTick >= tickLength) { 
            ticks++;
            if (tickType) onEnemyTick();
            else onPlayerTick();
            tickType = !tickType;
            toNextTick = toNextTick % tickLength;
        }
        tickTimer.transform.position = new Vector3(toNextTick / tickLength * 8f - 4f, tickTimer.transform.position.y, 0);
    }
}
