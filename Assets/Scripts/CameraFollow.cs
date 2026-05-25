using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    public Vector3 offset = new Vector3(0f, 15f, -10f); 

    void Update () {
        if (player != null) {
            transform.position = player.transform.position + offset;
        }
    }
}