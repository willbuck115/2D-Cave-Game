using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode jump;
    [SerializeField] private float speed;
    [SerializeField] private int renderDistanceInclusive;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    public ClassicWorldGeneration worldGeneration;
    public MiningMechanic miningMechanic;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        miningMechanic = GetComponent<MiningMechanic>();

        LoadNewTiles();

    }

    private void Update() {
        if (Input.GetKey(left)) {
            // go left
            transform.position += Vector3.left * speed * Time.deltaTime;
            // if block is there start destroying if possible
        } else if (Input.GetKey(right)) {
            // go right
            transform.position += Vector3.right * speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(jump) && IsGrounded()) {
            rb.velocity = Vector2.up * jumpForce;
        }
    }
    private void LoadNewTiles() {
        Vector2 pos = transform.position;
        for (int x = (int)pos.x - renderDistanceInclusive; x <= (int)pos.x + renderDistanceInclusive; x++) {
            for (int y = (int)pos.y - renderDistanceInclusive; y <= (int)pos.y + renderDistanceInclusive; y++) {
                if (worldGeneration.tileMap.GetLength(1) > y) {
                    if (worldGeneration.tileMap[x, y] != 0) {
                        foreach (GeneratableTile t in worldGeneration.generatableTiles) {
                            if (t.generationID == worldGeneration.tileMap[x, y]) {
                                print(worldGeneration.tileMap[x, y]);
                                GameObject tile = Instantiate(t.prefab.prefab, worldGeneration.transform);
                                tile.transform.position = new Vector2(x + 1, y + 1);
                                print(tile);
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsGrounded() {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, layerMask);
        return hit.collider != null;
    }
}

/*public class Tile {
    public GameObject prefab;
    public Vector2 coordinates;
    public bool isTile = false;

    public Tile(GameObject p, bool isT) {
        prefab = p;
        isTile = isT;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        //Destroy(this.gameObject);
    }
}
*/