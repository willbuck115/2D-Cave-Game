using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int generationID;
    public string id;
    public GameObject prefab;
    public bool isLoaded = true;
    public bool neverDestroy = false;
    public int hardness;
    public float mineTime;
    public Vector2 indexInNoiseArray;

    private void OnTriggerExit2D(Collider2D collision) {
        if(!neverDestroy)
            Destroy(this.gameObject);
    }
}
