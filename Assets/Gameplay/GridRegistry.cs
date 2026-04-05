using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridRegistry : MonoBehaviour {
    public List<int>[,] grid;
    public List<KnownPosition> registry;
    public Vector2Int boardSize;

    void Awake() {
        grid = new List<int>[(int)boardSize.x, (int)boardSize.y];
        registry = new List<KnownPosition>();
    }


    public void Register(KnownPosition obj) {
        obj.registry = this;
        int regI = registry.Count;
        registry.Add(obj);
        obj.registryIndex = regI;
        
        int baseX = obj.gridPos.x;
        int baseY = obj.gridPos.y;
        for (int x = baseX; x < baseX + obj.size.x - 1; x++) {
            for (int y = baseY; y < baseY + obj.size.y - 1; y++) {
                grid[x,y].Add(regI);
            }
        }
    }

    public void Move(KnownPosition obj, Vector2Int finalPos) {
        Vector2Int start = obj.gridPos;
    }
}
