using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // can be upgraded this is default from start of game
    [SerializeField] private int inventorySpace = 50;
    [SerializeField] internal List<InventoryItem> items = new List<InventoryItem>();

    [SerializeField] internal InventoryItem[] possibleItems;

    internal void PickUpItem(InventoryItem pickedUp) {
        if(items.Count < inventorySpace) {
            // pickup item
            foreach(InventoryItem i in possibleItems) {
                if (i.numericID == pickedUp.numericID) {
                    items.Add(i);
                }
            }
            Destroy(pickedUp.gameObject);
        } else {
            // inventory too full to carry
            Debug.Log("not implemented");
        }
    }

    internal void RemoveItemFromInventory(int index) {
        Debug.Log("Remove");
        items.Remove(items[index]);
    }
}
