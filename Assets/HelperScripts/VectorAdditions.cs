using System;
using UnityEngine;

public static class VectorAdditions {
    public static Vector3 ExtendXZ(this Vector2 xz, float y) {
        return new Vector3(xz.x, y, xz.y);
    }

    public static Vector3Int ExtendXZ(this Vector2Int xz, int y) {
        return new Vector3Int(xz.x, y, xz.y);
    }

    public static Vector2 CollapseXZ(this Vector3 xz) {
        return new Vector2(xz.x, xz.z);
    }

    public static Vector2Int Floor(this Vector2 a) {
        return new Vector2Int(Mathf.FloorToInt(a.x), Mathf.FloorToInt(a.y));
    }

    public static Vector3Int Floor(this Vector3 a) {
        return new Vector3Int(Mathf.FloorToInt(a.x), Mathf.FloorToInt(a.y), Mathf.FloorToInt(a.z));
    }

    public static Vector3 SetInRange(this Vector3 a, Vector3 c1, Vector3 c2) {
        Vector3 cL = new Vector3(Mathf.Max(c1.x, c2.x), Mathf.Max(c1.y, c2.y), Mathf.Max(c1.z, c2.z));
        Vector3 cS = new Vector3(Mathf.Min(c1.x, c2.x), Mathf.Min(c1.y, c2.y), Mathf.Min(c1.z, c2.z));

        Vector3 i =  new Vector3(Mathf.Min(a.x, cL.x), Mathf.Min(a.y, cL.y), Mathf.Min(a.z, cL.z));
        return       new Vector3(Mathf.Max(i.x, cS.x), Mathf.Max(i.y, cS.y), Mathf.Max(i.z, cS.z));
    }

    public static Vector3 SetInRange(this Vector3 a, Vector3 corner) {
        return a.SetInRange(corner, -corner);
    }

    public static Vector3 GridToWorld(this Vector2Int a, Vector2Int worldSize, float y) {
        return ((Vector2)(a - worldSize/2)).ExtendXZ(y);
    }

    public static int DNDLength(this Vector2Int a) {
        return Math.Max(a.x, a.y);
    }
}