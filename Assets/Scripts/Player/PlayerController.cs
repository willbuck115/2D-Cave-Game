using System;
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

    [SerializeField] internal WorldGeneration worldGeneratorClass;
    private Tile[,] loadedTiles;
    [SerializeField] private LayerMask layerMask;

    private PlayerBase playerBaseClass;
    [SerializeField] private Transform collectables;

    // needs to be removed
    public bool debugworld;
    public GameObject mapSprite;

    private void Start() {
        playerBaseClass = GetComponent<PlayerBase>();
        loadedTiles = new Tile[worldGeneratorClass.xWidth, worldGeneratorClass.yHeight];
        if (debugworld)
            LoadDebugWorld();
        else
            LoadNewTiles();

    }

    private void LoadDebugWorld() {
        // create a texture for the map
        Texture2D mapTexture = new Texture2D(worldGeneratorClass.xWidth, worldGeneratorClass.yHeight);
        Color[] colours = new Color[worldGeneratorClass.xWidth * worldGeneratorClass.yHeight];
        
        for(int x = 0; x < worldGeneratorClass.xWidth; x++) {
            for(int y = 0; y < worldGeneratorClass.yHeight; y++) {
                if (worldGeneratorClass.tileMap[x, y] == 0) {
                    colours[x + worldGeneratorClass.xWidth * y] = Color.black;
                    //mapTexture.SetPixel(x, y, Color.black);
                } else {
                    colours[x + worldGeneratorClass.xWidth * y] = worldGeneratorClass.generatableTiles[worldGeneratorClass.tileMap[x, y] - 1].colour;
                    //mapTexture.SetPixel(x, y, worldGeneratorClass.generatableTiles[worldGeneratorClass.tileMap[x, y] - 1].colour);
                }
            }
        }
        mapTexture.SetPixels(colours);
        mapTexture.filterMode = FilterMode.Point;
        mapTexture.Apply();
        SpriteRenderer rend = mapSprite.GetComponent<SpriteRenderer>();
        Sprite sprite = Sprite.Create(mapTexture, new Rect(0, 0, worldGeneratorClass.xWidth, worldGeneratorClass.yHeight), Vector2.zero, 1f);
        rend.sprite = sprite;
    }

    private void Update() {
        if (!isAtTarget) {
            // skip the rest of update
            UpdatePosition();
            return;
        }

        if (!isRunningCoroutine) {
            if (Input.GetKey(left) && (int)transform.position.x > 49) {
                // go left
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.left));
            } else if (Input.GetKey(right) && (int)transform.position.x <= 149) {
                // go right
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.right));
            } else if (Input.GetKey(down) && (int)transform.position.y > 50) {
                // go downwards
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.down));
            } else if (Input.GetKey(up) && (int)transform.position.y <= 550) { // only go up if not at top end of world
                isRunningCoroutine = true;
                StartCoroutine(TriggerMovement(Vector2.up));
            }
        }
    }

    private void UpdatePosition() {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        if ((Vector2)transform.position == targetPosition) {
            isAtTarget = true;
            float x = playerBaseClass.playerLimitClass.CurrentHeat;
            if (!debugworld) {
                LoadNewTiles();
            }
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
            playerBaseClass.playerLimitClass.OnStaminaUpdate(-1);
            yield break;
        } else if (hit.collider.tag == "MinableTile") {
            Tile t = hit.collider.GetComponent<Tile>();
            if (playerBaseClass.playerMiningClass.AttemptToMineTile(t)) {
                yield return new WaitWhile(() => playerBaseClass.playerMiningClass.isMiningTile);
                    worldGeneratorClass.tileMap[(int)t.indexInNoiseArray.x, (int)t.indexInNoiseArray.y] = 0;
                    t.Mined(collectables);
                    playerBaseClass.playerLimitClass.OnStaminaUpdate(-(int)t.mineTime);
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

        if (xMaxRenderValue > worldGeneratorClass.xWidth)
            xMaxRenderValue = worldGeneratorClass.xWidth;

        if (yMinRenderValue < 0)
            yMinRenderValue = 0;

        if (yMaxRenderValue > worldGeneratorClass.yHeight)
            yMaxRenderValue = worldGeneratorClass.yHeight;

        print(yMinRenderValue);
        print(yMaxRenderValue);

        // Checking if within bounds of render distance on xCord
        for (int x = xMinRenderValue; x <= xMaxRenderValue; x++) {
            // Checking if within bounds of render distance on yCord
            for (int y = yMinRenderValue; y <= yMaxRenderValue; y++) {
                // Checking to make sure the area is within the map
                if (worldGeneratorClass.tileMap.GetLength(0) > x && worldGeneratorClass.tileMap.GetLength(1) > y) {
                    // Check to make sure tile has not already loaded
                    if (loadedTiles[x, y] == null) {
                        // If the tile exists (isn't a 0 tile)
                        if (worldGeneratorClass.tileMap[x, y] != 0) {
                            foreach (GeneratableTile t in worldGeneratorClass.generatableTiles) {
                                if (t.tileIndex == worldGeneratorClass.tileMap[x, y]) {
                                    GameObject tile = Instantiate(t.prefab, worldGeneratorClass.transform);
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
