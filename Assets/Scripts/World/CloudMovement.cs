using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour {
    internal float speed;

    private void Update() {
        transform.position += Vector3.right * speed * Time.timeScale;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // if entered cloud teleporter
        if(collision.tag == "CloudTrigger") {
            transform.position = new Vector2(25, transform.position.y);
        }
    }
}
