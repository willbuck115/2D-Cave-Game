using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    internal PlayerMining playerMiningClass;
    internal PlayerController playerControllerClass;
    internal PlayerInventory playerInventoryClass;
    internal PlayerLimits playerLimitClass;

    [SerializeField] internal LayerMask layerMask;

    private void Awake() {
        playerMiningClass = GetComponent<PlayerMining>();
        playerControllerClass = GetComponent<PlayerController>();
        playerInventoryClass = GetComponent<PlayerInventory>();
        playerLimitClass = GetComponent<PlayerLimits>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(1)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 10f, layerMask);
            if (hit.collider != null) {
                print(hit.collider.name);
                if (hit.collider.GetComponent<InteractableTile>()) {
                    hit.collider.GetComponent<InteractableTile>().Interact();
                }
            }   
        }
    }

    internal void UpdatePickaxeSprite() {
        //throw new NotImplementedException();
    }
}
