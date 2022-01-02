using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningMechanic : MonoBehaviour
{

    public int toolHardness = 0;
    private PlayerController player;
    public bool isMiningTile;

    private Tile tile;

    private void Start() {
        player = this.GetComponent<PlayerController>();
    }

    public bool AttemptToMineTile(Tile t) {
        tile = t;
        if(t.hardness <= toolHardness) {
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
