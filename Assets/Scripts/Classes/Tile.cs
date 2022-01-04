using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int generationID;
    public GameObject prefab;
    public Vector2 indexInNoiseArray;

    public bool isLoaded = true;

    public int hardness;
    public float mineTime;
    // Not applicable to all tiles
    public InventoryItem dropItem = null;
    // How many items to drop
    public int drops = 0;

    private void Start() {
        if(dropItem!= null) {
            dropItem.numericID = generationID;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Destroy(this.gameObject);
    }

    public void Mined(Transform p) {
        if(dropItem != null) {
            // give the player the item or drop on ground to be picked up
            GameObject obj = Instantiate(dropItem.prefab, p);
            obj.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}
