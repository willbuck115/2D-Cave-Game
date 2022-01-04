using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
    [HideInInspector] public int numericID;
    public float monetaryValue;
    public GameObject prefab;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.name == "Player") {
            PlayerInventory inventory = collision.GetComponent<PlayerInventory>();
            inventory.PickUpItem(this);
        }
    }

}
