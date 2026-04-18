using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GridRegistry : MonoBehaviour {
    public List<int>[,] grid;
    public List<GridObject> registry;
    public Vector2Int boardSize;
    public float cellSize;

    public int collisionResolution;

    void Awake() {
        grid = new List<int>[boardSize.x, boardSize.y];
        for (int x = 0; x < boardSize.x; x++) 
        for (int y = 0; y < boardSize.y; y++) {
                grid[x,y] = new List<int>();
        }
        registry = new List<GridObject>();
    }


    public void Register(GridObject obj) {
        obj.grid = this;
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

    public void Move(GridObject obj, Vector2Int finalPos, float time, Ease easing) {
        Debug.Log("Moving");
        Vector2Int startPos = obj.gridPos;
        Debug.Log(startPos);
        Debug.Log(finalPos);
        obj.moving = true;
        List<Vector2Int> intermediates = new();
        if (collisionResolution == 0) {Debug.LogError("Zero Collison Res"); return;}
        if (Mathf.FloorToInt((finalPos - startPos).magnitude) == 0) {Debug.LogError("Start too close to end"); return;}
        Debug.Log("Intermediates");
        float stepSize = 1 / collisionResolution;
        float dist = Mathf.FloorToInt((finalPos - startPos).magnitude);
        float stepT = stepSize / dist;
        for (float t = 0; t <= 1; t += stepT) {
            Vector2Int pos = Vector2.Lerp(startPos, finalPos, t).Round();
            if (!intermediates.Contains(pos)) intermediates.Add(pos);
        }
        Debug.Log("Intermediates Found");
        grid[finalPos.x, finalPos.y].Append(obj.registryIndex); 
        foreach (Vector2Int inter in intermediates) {
            foreach (int i in grid[inter.x, inter.y]) {
                if (i == obj.registryIndex) continue;
                Debug.Log("Collision");
                GridObject coll = registry[i];
                if (coll.Collide(obj)) return;
            }
            grid[inter.x, inter.y].Add(obj.registryIndex);
        }
        Sequence move = DOTween.Sequence();
        move.Append(obj.transform.DOMove(Convert(finalPos, obj.transform.position.y), time).SetEase(easing));
        move.AppendCallback(() => { 
            obj.moving = false;
            foreach (Vector2Int inter in intermediates) {
                grid[inter.x, inter.y].Remove(obj.registryIndex);
            }
            grid[finalPos.x, finalPos.y].Append(obj.registryIndex);
            obj.gridPos = finalPos;
        });
    }

    public Vector3 Convert(Vector2Int gridPos, float vert) {
        Vector2 center = gridPos - (boardSize / 2);
        return new Vector3(center.x * cellSize, vert, center.y * cellSize);
    }

    public Vector2Int Convert(Vector3 pos) {return LosslessConvert(pos).Floor();}

    public Vector2 LosslessConvert(Vector3 pos) {
        Vector2 scaled = pos / cellSize;
        return scaled + (boardSize / 2);
    }

    public Vector2Int ForceInGrid(Vector2Int gridPos) {
        int x = Math.Min(Math.Max(gridPos.x, 0), boardSize.x - 1);
        int y = Math.Min(Math.Max(gridPos.y, 0), boardSize.y - 1);
        return new Vector2Int(x,y);
    }
}
