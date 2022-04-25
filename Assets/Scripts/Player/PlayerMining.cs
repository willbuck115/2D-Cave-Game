using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMining : MonoBehaviour
{

    public Pickaxe currentPickaxe;
    public Pickaxe[] ownedPickaxes;
    public GameObject pickaxeObject;
    private SpriteRenderer pickaxeRend;

    private PlayerBase playerClass;
    public bool isMiningTile;

    private Tile tile;

    private void Start() {
        playerClass = this.GetComponent<PlayerBase>();
        pickaxeRend = pickaxeObject.GetComponent<SpriteRenderer>();
        pickaxeRend.sprite = currentPickaxe.pickaxeTexture;
    }

    public bool AttemptToMineTile(Tile t) {
        tile = t;
        if(t.hardness <= currentPickaxe.miningLevel) {
            isMiningTile = true;
            StartCoroutine(MineTile());
            return true;
        }
        return false;
    }

    private IEnumerator MineTile() {
        yield return new WaitForSecondsRealtime(tile.mineTime);
        isMiningTile = false;
    }

}
