using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;

    void Update () {
        transform.position = player.transform.position + new Vector3(0f, 15f, -10f);
    }
}
