using UnityEngine;

public class WorldGeneration : MonoBehaviour {
    // Array of integers that correlate to what tile should be loaded there.
    // If zero, no tile will be loaded

    // For Debug purposes
    [SerializeField] public bool shouldLoadFromFile = true;

    public int[,] tileMap = null;
    public GeneratableTile[] generatableTiles; // stone not kept here
    public GeneratableTile stoneTile;
    [SerializeField] private AssetSaveManager assetSaveManager;

    public int xWidth, yHeight;
    [SerializeField] private GameObject player;

    private void Start() {
        if (shouldLoadFromFile) {
            assetSaveManager.InitaliseLoad("/TileMap");
            tileMap = assetSaveManager.loadedTileMap;
        }

        if (!shouldLoadFromFile || tileMap == null) {
            tileMap = new int[xWidth, yHeight];
            // set default values
            // sets grass layer
            for (int x = 0; x < xWidth; x++) {
                int y = 1049;
                Debug.Log(x + " " + y);
                tileMap[x, y] = 1;
            }
            // sets dirt layer
            for (int x = 0; x < xWidth; x++) {
                for (int y = 1000; y < 1049; y++) {
                    tileMap[x, y] = new int();
                    tileMap[x, y] = 2;
                }
            }
            // sets stone layer
            for (int x = 0; x < xWidth; x++) {
                for (int y = 0; y < yHeight; y++) {
                    int index = stoneTile.ChooseTile(y);
                    if (index != 0) {
                        tileMap[x, y] = index;
                    }
                }
            }


            // Generate the map changes

            for (int x = 0; x < xWidth; x++) {
                for (int y = 0; y < yHeight; y++) {
                    int index = 0;
                    foreach (GeneratableTile t in generatableTiles) {
                        index = t.ChooseTile(y);
                        if (index != 0) {
                            tileMap[x, y] = index;
                        }
                    }
                }
            }
        }

        player.SetActive(true);
    }

    // this is debug
    private void Update() {
        if (Input.GetKeyDown(KeyCode.L)) {
            assetSaveManager.Save(tileMap);
            Debug.Log("REMOVE ME!");
        }
    }
}
