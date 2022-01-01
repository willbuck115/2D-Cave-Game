using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicWorldGeneration : MonoBehaviour
{
    // Array of integers that correlate to what tile should be loaded there.
    // If zero, no tile will be loaded
    public int[,] tileMap = null;
    public GeneratableTile[] generatableTiles;
    public Layer[] generationLayers;

    public int xWidth, yHeight;
    [SerializeField] private GameObject player;

    private void Start() {

        foreach (Layer l in generationLayers) {
            l.LoadValues();
        }
        foreach (GeneratableTile g in generatableTiles) {
            g.LoadValues();
        }

        if (tileMap == null) {
            tileMap = new int[xWidth, yHeight];
            // Generate a new map
            for(int x = 0; x < xWidth; x++) {
                for(int y = 0; y < yHeight; y++) {
                    foreach(Layer l in generationLayers) {
                        if (l.IsWithinLayer(y)) {
                            tileMap[x, y] = l.ChooseTile();
                        }
                    }
                }
            }
        }

        // Enable player after array has been generated so tiles can be instantiated
        player.SetActive(true);
    }
}

[System.Serializable]
public class GeneratableTile {
    [HideInInspector] public int generationID;
    [HideInInspector] public PrefabIDLink prefab;

    public Tile tile;

    public void LoadValues() {
        generationID = tile.generationID;
        prefab.prefab = tile.prefab;
        prefab.tile = tile;
    }
    /*public GeneratableTile TileTypeCheck(int depth) {
        // Check if depth is within bounds of this tile selector
        if (depth < maxDepthExclusive - 1 && depth >= minDepthInclusive - 1) {
            return this;
        } else
            return null;
    }
    */
}

[System.Serializable]
public class Layer {
    [SerializeField] private string layerID; // inspector use only

    // prefabs[0] is the default, if no other chance is capitalized then it will use default
    [HideInInspector] public PrefabIDLink[] prefabs;

    public TileChance[] tile;
    public int maxDepthInclusive;
    public int minDepthInclusive;

    public void LoadValues() {
        prefabs = new PrefabIDLink[tile.Length];

        for(int i = 0; i < prefabs.Length; i++) {
            prefabs[i] = new PrefabIDLink();
            prefabs[i].tile = tile[i].tile;
            prefabs[i].generationID = tile[i].tile.generationID;
            prefabs[i].spawnChance = tile[i].spawnChance;
        }
    }
    public int ChooseTile() {
        int objIndex = prefabs[0].generationID;
        for (int i = 1; i < prefabs.Length; i++) {
            float chance = Random.value;
            if(chance < prefabs[i].spawnChance) {
                // Pick this one
                objIndex = prefabs[i].generationID;
            }
        }
        // if at the end the objIndex has not been modified it will default to the prefab[0] generationID
        return objIndex;
    }
    public bool IsWithinLayer(int yCord) {
        if(yCord <= maxDepthInclusive - 1 && yCord >= minDepthInclusive - 1) {
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class PrefabIDLink {
    [SerializeField] private string tileID; // inspector use only
    [HideInInspector] public Tile tile;

    [HideInInspector] public GameObject prefab;
    [HideInInspector] public int generationID;
    [Range(0,1)] public float spawnChance;
}

[System.Serializable]
public class TileChance {
    public Tile tile;
    [Range(0,1)] public float spawnChance;
}
