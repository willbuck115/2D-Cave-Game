using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forge : MonoBehaviour
{
    public PlayerBase PlayerBaseClass;

    public void BuildPickaxe(Pickaxe pickaxe) {
        // if owned
        foreach(Pickaxe p in PlayerBaseClass.playerMiningClass.ownedPickaxes) {
            if(p == pickaxe) {
                PlayerBaseClass.playerMiningClass.currentPickaxe = pickaxe;
            }
        }
        // if not owned
        if (pickaxe.CanCraft(PlayerBaseClass)) {
            pickaxe.Craft(PlayerBaseClass);
            PlayerBaseClass.playerMiningClass.currentPickaxe = pickaxe;
            PlayerBaseClass.UpdatePickaxeSprite();
        } else {
            Debug.Log("You cannot craft this");
        }
    }


}

