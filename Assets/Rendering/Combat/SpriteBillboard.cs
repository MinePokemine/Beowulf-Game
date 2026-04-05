using UnityEngine;

public class SpriteBillboard : MonoBehaviour {
    public GameObject mainCamera;

    void Update() {
        transform.rotation = Quaternion.Euler(new Vector3(0,Camera.main.transform.rotation.eulerAngles.y, 0));

    }
}
