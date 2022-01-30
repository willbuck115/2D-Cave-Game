using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

    [SerializeField] private Transform player;

    private void Update() {
        Vector2 newPosition;
        newPosition = new Vector3(player.transform.position.x, player.transform.position.y, -1f);
        transform.position = newPosition;
    }
}
