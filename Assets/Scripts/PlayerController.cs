using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private KeyCode left;
    [SerializeField] private KeyCode right;
    [SerializeField] private KeyCode jump;
    [SerializeField] private float speed;
    [SerializeField] private int renderDistanceInclusive;

    [Range(0, 200)] private int xMinRenderValue, xMaxRenderValue;
    [Range(0, 550)] private int yMinRenderValue, yMaxRenderValue;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    public ClassicWorldGeneration worldGeneration;
    public MiningMechanic miningMechanic;

    private Tile[,] loadedTiles;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        miningMechanic = GetComponent<MiningMechanic>();
        loadedTiles = new Tile[worldGeneration.xWidth, worldGeneration.yHeight];

        LoadNewTiles();

    }

    private void Update() {
        if (Input.GetKey(left)) {
            // go left
            transform.position += Vector3.left * speed * Time.deltaTime;
            LoadNewTiles();
        } else if (Input.GetKey(right)) {
            // go right
            transform.position += Vector3.right * speed * Time.deltaTime;
            LoadNewTiles();
        }

        if (Input.GetKeyDown(jump) && IsGrounded()) {
            rb.velocity = Vector2.up * jumpForce;
        }
    }
    private void LoadNewTiles() {
        Vector2 pos = transform.position;

        // Calculate the first and last parts of the tile loading sequence+
        xMinRenderValue = (int)pos.x - renderDistanceInclusive;
        xMaxRenderValue = (int)pos.x + renderDistanceInclusive;
        yMinRenderValue = (int)pos.y - renderDistanceInclusive;
        yMaxRenderValue = (int)pos.y + renderDistanceInclusive;

        // Clamp the values to the bounds of the map array
        if (xMinRenderValue < 0)
            xMinRenderValue = 0;

        if (xMaxRenderValue > worldGeneration.xWidth)
            xMaxRenderValue = worldGeneration.xWidth;

        if (yMinRenderValue < 0)
            yMinRenderValue = 0;

        if (yMaxRenderValue > worldGeneration.yHeight)
            yMaxRenderValue = worldGeneration.yHeight;

        print(xMinRenderValue+" "+ xMaxRenderValue+" "+ yMinRenderValue+" "+ yMaxRenderValue);

        // Checking if within bounds of render distance on xCord
        for (int x = xMinRenderValue; x <= xMaxRenderValue; x++) {
            // Checking if within bounds of render distance on yCord
            for (int y = yMinRenderValue; y <= yMaxRenderValue; y++) {
                // Checking to make sure the area is within the map
                if (worldGeneration.tileMap.GetLength(0) > x && worldGeneration.tileMap.GetLength(1) > y) {
                    // Check to make sure tile has not already loaded
                    if (loadedTiles[x, y] == null) {
                        // If the tile exists (isn't a 0 tile)
                        if (worldGeneration.tileMap[x, y] != 0) {
                            foreach (GeneratableTile t in worldGeneration.generatableTiles) {
                                if (t.generationID == worldGeneration.tileMap[x, y]) {
                                    print(worldGeneration.tileMap[x, y]);
                                    GameObject tile = Instantiate(t.prefab.prefab, worldGeneration.transform);
                                    tile.transform.position = new Vector2(x + 1, y + 1);
                                    loadedTiles[x, y] = tile.GetComponent<Tile>();
                                }
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