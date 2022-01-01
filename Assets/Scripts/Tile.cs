using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int generationID;
    public string id;
    public GameObject prefab;
    public bool isTile = false;
    public bool neverDestroy = false;
    public int hardness;

    private void OnTriggerExit2D(Collider2D collision) {
        if(!neverDestroy)
            Destroy(this.gameObject);
    }
}
