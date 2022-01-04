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

    public ClassicWorldGeneration IWorldGeneratorClass;
    private Tile[,] loadedTiles;
    [SerializeField] private LayerMask layerMask;

    private PlayerBase playerBaseClass;
    [SerializeField] private Transform collectables;



    private void Start() {
        playerBaseClass = GetComponent<PlayerBase>();
        loadedTiles = new Tile[IWorldGeneratorClass.xWidth, IWorldGeneratorClass.yHeight];

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
        if (hit.collider == null) {
            // move
            //transform.position += dir * speed * Time.deltaTime;
            targetPosition = transform.position + dir;
            isAtTarget = false;
            isRunningCoroutine = false;
            yield break;
        } else if (hit.collider.tag == "MinableTile") {
            Tile t = hit.collider.GetComponent<Tile>();
            if (playerBaseClass.playerMiningClass.AttemptToMineTile(t)) {
                yield return new WaitWhile(() => playerBaseClass.playerMiningClass.isMiningTile);
                    IWorldGeneratorClass.tileMap[(int)t.indexInNoiseArray.x, (int)t.indexInNoiseArray.y] = 0;
                    t.Mined(collectables);
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

        if (xMaxRenderValue > IWorldGeneratorClass.xWidth)
            xMaxRenderValue = IWorldGeneratorClass.xWidth;

        if (yMinRenderValue < 0)
            yMinRenderValue = 0;

        if (yMaxRenderValue > IWorldGeneratorClass.yHeight)
            yMaxRenderValue = IWorldGeneratorClass.yHeight;

        // Checking if within bounds of render distance on xCord
        for (int x = xMinRenderValue; x <= xMaxRenderValue; x++) {
            // Checking if within bounds of render distance on yCord
            for (int y = yMinRenderValue; y <= yMaxRenderValue; y++) {
                // Checking to make sure the area is within the map
                if (IWorldGeneratorClass.tileMap.GetLength(0) > x && IWorldGeneratorClass.tileMap.GetLength(1) > y) {
                    // Check to make sure tile has not already loaded
                    if (loadedTiles[x, y] == null) {
                        // If the tile exists (isn't a 0 tile)
                        if (IWorldGeneratorClass.tileMap[x, y] != 0) {
                            foreach (GeneratableTile t in IWorldGeneratorClass.generatableTiles) {
                                if (t.generationID == IWorldGeneratorClass.tileMap[x, y]) {
                                    GameObject tile = Instantiate(t.prefab.prefab, IWorldGeneratorClass.transform);
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
}
