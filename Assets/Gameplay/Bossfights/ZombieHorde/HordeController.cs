using System.Collections.Generic;
using UnityEngine;

public class HordeController : MonoBehaviour {
    public Gametick tick;
    [SerializeField] Vector2Int[] waves; // (Spawned, Progress)
    int waveIndex = -1;
    List<Zombie> zombies = new();
    public GridRegistry grid;
    public Player plr;

    [SerializeField] GameObject zombiePrefab;


    void Start() {
        NewWave(waves[0].x);
        waveIndex = 0;
    }

    void NewWave(int num) {
        waveIndex++;
        Vector3[] poss = new Vector3[num];
        for (int i = 0; i < num; i++) {
            poss[i] = new Vector3(
                i * (i % 2 == 0 ? 1 : 0),
                1, 3
            );
        }

        foreach (Vector3 pos in poss) {
            GameObject zombObj = Instantiate(zombiePrefab, pos, Quaternion.identity);
            Zombie zomb = zombObj.GetComponent<Zombie>();
            zombies.Add(zomb);
            zomb.tick = tick;
            zomb.player = plr;
            zomb.controller = this;
            grid.Register(zomb);
        }
    }

    void Update() {
        if (zombies.Count <= waves[waveIndex].y) {
            if (waveIndex < waves.Length) NewWave(waves[waveIndex].x);
            else {Debug.Log("Win!"); Destroy(gameObject);}
        }
    }

    public void ZombieHit(Zombie z) {
        if (z.health <= 0) { zombies.Remove(z); Destroy(z.gameObject); }
    }
}
