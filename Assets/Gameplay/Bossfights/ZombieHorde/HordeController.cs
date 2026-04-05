using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HordeController : MonoBehaviour {
    public Gametick tick;
    [SerializeField] Vector2Int[] waves; // (Spawned, Progress)
    int waveIndex;
    List<Zombie> zombies = new();

    [SerializeField] GameObject zombiePrefab;


    void Update() {
        foreach (Zombie z in zombies) {
            if (z.health <= 0) { zombies.Remove(z); Destroy(z); }
        }

        if (zombies.Count < waves[waveIndex].y) {
            // New Wave
            waveIndex++;
            Vector3[] poss = new Vector3[waves[waveIndex].x];
            for (int i = 0; i < waves[waveIndex].x; i++) {
                poss[i] = new Vector3(
                    i * (i % 2 == 0 ? 1 : 0),
                    5, 0
                );
            }

            foreach (Vector3 pos in poss) Instantiate(zombiePrefab, pos, Quaternion.identity);
        }
    }
}
