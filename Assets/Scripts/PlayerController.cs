using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode jump;
    [SerializeField] private float speed;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    public MiningMechanic miningMechanic;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        miningMechanic = GetComponent<MiningMechanic>();
    }

    private void Update() {
        if (Input.GetKey(left)) {
            // go left
            transform.position += Vector3.left * speed * Time.deltaTime;
        } else if (Input.GetKey(right)) {
            // go right
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(jump) && IsGrounded()) {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private bool IsGrounded() {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, layerMask);
        return hit.collider != null;
    }
}
