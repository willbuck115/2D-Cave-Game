using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private KeyCode up, down, left, right;
    [SerializeField] private float speed;
    private Vector2 targetPosition;
    private bool isAtTarget = true;
    private bool isRunningCoroutine = false;

    [SerializeField] private int renderDistanceInclusive;
    [Range(0, 200)] private int xMinRenderValue, xMaxRenderValue;
    [Range(0, 550)] private int yMinRenderValue, yMaxRenderValue;

    [SerializeField] private LayerMask layerMask;
    
    
    [SerializeField] private float jumpForce;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    public ClassicWorldGeneration worldGeneration;
    private MiningMechanic miningMechanic;

    private Tile[,] loadedTiles;



    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        miningMechanic = this.GetComponent<MiningMechanic>();
        loadedTiles = new Tile[worldGeneration.xWidth, worldGeneration.yHeight];

        LoadNewTiles();

    }

    private void Update() {
        if (!isAtTarget) {
            // skip the rest of update
            UpdatePosition();
            return;
        }

        if (!isRunningCoroutine) {
            if (Input.GetKey(left)) {
                // go left
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.left));
            } else if (Input.GetKey(right)) {
                // go right
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.right));
            } else if (Input.GetKey(down)) {
                // go downwards
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.down));
            } else if (Input.GetKey(up) && transform.position.y <= 550) { // only go up if not at top end of world
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.up));
            }
        }

        /*if (Input.GetKeyDown(jump) && IsGrounded()) {
            rb.velocity = Vector2.up * jumpForce;
        }
        */
    }

    private void UpdatePosition() {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        if((Vector2)transform.position == targetPosition) {
            isAtTarget = true;
            LoadNewTiles();
        }
    }

    private IEnumerator TriggerMovement(Vector3 dir) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, .52f, layerMask);
        print(hit.collider);
        if (hit.collider == null) {
            // move
            //transform.position += dir * speed * Time.deltaTime;
            targetPosition = transform.position + dir;
            isAtTarget = false;
            isRunningCoroutine = false;
            yield break;
        } else if (hit.collider.tag == "MinableTile") {
            Tile t = hit.collider.GetComponent<Tile>();
            if (miningMechanic.AttemptToMineTile(t)) {
                Debug.Log("waiting for mine");
                yield return new WaitWhile(() => miningMechanic.isMiningTile);
                    worldGeneration.tileMap[(int)t.indexInNoiseArray.x, (int)t.indexInNoiseArray.y] = 0;
                    Destroy(hit.collider.GetComponent<Tile>());
                    Destroy(hit.collider.gameObject);
            }
        }

        isRunningCoroutine = false;
        yield break;
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
                                    GameObject tile = Instantiate(t.prefab.prefab, worldGeneration.transform);
                                    tile.transform.position = new Vector2(x + 1, y + 1);
                                    Tile tileComponent = tile.GetComponent<Tile>();
                                    loadedTiles[x, y] = tileComponent;
                                    tileComponent.indexInNoiseArray = new Vector2(x, y);
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