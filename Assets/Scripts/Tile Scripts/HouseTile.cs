using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseTile : MonoBehaviour
{

    [SerializeField] private DayManager dayManagerClass;

    private void OnTriggerEnter(Collider other) {
        // load the house ui?
        // ui will have a screen that asks if the player would like to go inside which will reset: stamina and the current day
        // also tells the player whether they will get a forfeit for this.
    }

    internal void OnHouseEnter() {
        // house has been entered
        dayManagerClass.CalculateSleep();
    }

    private void Update() {
        // debug
        if (Input.GetKeyDown(KeyCode.Numlock)) {
            OnHouseEnter();
        }
    }

}
