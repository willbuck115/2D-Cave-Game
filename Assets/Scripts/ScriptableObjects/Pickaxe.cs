using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pickaxe", menuName = "ScriptableObjects/Pickaxe", order = 1)]
public class Pickaxe : ScriptableObject
{

    //public Sprite pickaxeTexture;
    //public 

    public int miningLevel = 0;
    public int numberOfMaterialsRequired = 0;
    private int numberInInventory = 0;

    public InventoryItem craftingItem;
    private List<int> itemsIndexesToRemove = new List<int>();

    public bool CanCraft(PlayerBase p) {
        numberInInventory = 0;
        foreach (InventoryItem item in p.PlayerInventoryClass.items) {
            if (item == craftingItem) {
                numberInInventory = numberInInventory + 1;
            }
        }

        if (numberInInventory >= numberOfMaterialsRequired) {
            return true;
        }

        numberInInventory = 0;
        return false;
    }

    public Pickaxe Craft(PlayerBase p) {
        Debug.Log(numberInInventory);
        numberInInventory = 0;
        itemsIndexesToRemove = new List<int>();
        Debug.Log(p.PlayerInventoryClass.items.Count);
        for (int i = 0; i < p.PlayerInventoryClass.items.Count; i++) {
            if (p.PlayerInventoryClass.items[i].numericID == craftingItem.numericID && numberInInventory < numberOfMaterialsRequired) {
                numberInInventory = numberInInventory + 1;
                itemsIndexesToRemove.Add(i);
                Debug.Log(i);
            }
        }

        for(int i = 0; i < itemsIndexesToRemove.Count; i++) {
            p.PlayerInventoryClass.RemoveItemFromInventory(itemsIndexesToRemove.Count - i - 1);
        }

        return this;
    }
}
