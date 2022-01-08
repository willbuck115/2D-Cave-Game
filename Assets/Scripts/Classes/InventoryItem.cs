using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {
    [HideInInspector] public int numericID;
    public float monetaryValue;
    public GameObject prefab;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.transform.name == "Player") {
            PlayerInventory inventory = collision.gameObject.GetComponent<PlayerInventory>();
            inventory.PickUpItem(this);
        }
    }

}
