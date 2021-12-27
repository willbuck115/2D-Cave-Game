using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// First Iteration Map Generator
// Potential for caves etc but this is a game tech test
public class MapGenerator : MonoBehaviour
{
    public Tile[,] map;

    [SerializeField] private int xWidth;
    [SerializeField] private int yHeight;
    [SerializeField] public MapLayer[] mapLayers;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform chunk1, chunk2, chunk3, chunk4, chunk5;

    // Executed if this is new game
    private void Awake() {
        map = new Tile[xWidth + 1, yHeight + 1];

        for (int x = 0; x < xWidth; x++){
            for(int y = yHeight; y > 0; y--) {
                // Generate the unit with parameters;
                MapLayer currentLayer = GetLayer(y);
                Tile appendTile = currentLayer.GenerateTile(x, y);
                // add the tile to a list
                map[x, y] = appendTile;

                if(y > 800 && y <= 1000)
                    map[x, y].Instantiate(chunk1);
                if (y > 600 && y <= 800)
                    map[x, y].Instantiate(chunk2);
                if(y > 400 && y <= 600)
                    map[x, y].Instantiate(chunk3);
                if(y > 200 && y <= 400)
                    map[x, y].Instantiate(chunk4);
                if(y >= 0 && y <= 200)
                    map[x, y].Instantiate(chunk5);
            }
        }

        chunk3.gameObject.SetActive(false);
        chunk4.gameObject.SetActive(false);
        chunk5.gameObject.SetActive(false);
    }

    private MapLayer GetLayer(int yLevel) {
        MapLayer layer = null;
        for(int i = 0; i < mapLayers.Length; i++) {
            // goes through layers
            if (yLevel < mapLayers[i].yStart && yLevel > mapLayers[i].yEnd) {
                layer = mapLayers[i];
            }
            if (yLevel == mapLayers[i].yStart) {
                layer = mapLayers[i];
            }
        }
        return layer;
    }
}

[System.Serializable]
public class Unit {

    public string unitID;
    public GameObject prefab;

    // any other unit logic

    // hardness, valuable etc
}

[System.Serializable]
public class MapLayer {
    public string layerName;
    public int yStart;
    public int yEnd;
    public Unit[] possibleLayerUnit;

    public Tile GenerateTile(int x, int y) {
        int random = UnityEngine.Random.Range(0, possibleLayerUnit.Length);

        // create a tile for the list
        Tile t = new Tile(possibleLayerUnit[random], new Vector2(x, y));
        return t;
    }
}

public class Tile{
    public Unit unit;
    public GameObject prefab;
    public Vector2 position;


    public Tile(Unit u, Vector2 pos) {
        unit = u;
        prefab = u.prefab;
        position = pos;
    }

    public void Instantiate(Transform parent) {
        GameObject obj = GameObject.Instantiate(prefab);
        obj.transform.position = position;
        BoxCollider2D bc = obj.AddComponent<BoxCollider2D>();
        obj.layer = LayerMask.NameToLayer("Tiles");
        obj.transform.SetParent(parent);
    }
}

/*
 * 
 *         // init gameobject
        GameObject obj = GameObject.Instantiate(possibleLayerUnit[random].prefab);
        obj.transform.position = new Vector2(x, -y);
        BoxCollider2D bc = obj.AddComponent<BoxCollider2D>();
        obj.layer = LayerMask.NameToLayer("Tiles");
 * 
 * 
 */