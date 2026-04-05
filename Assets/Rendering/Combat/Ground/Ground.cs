using UnityEngine;

public class Ground : MonoBehaviour {
    void Start() {
        MeshRenderer render = gameObject.GetComponent<MeshRenderer>();
        render.material = Instantiate(render.material);
        render.material.SetInt("_TileY", (int)transform.localScale.x * 10);
        render.material.SetInt("_TileZ", (int)transform.localScale.y * 10);
    }
}
