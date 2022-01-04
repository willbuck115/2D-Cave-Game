using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public PlayerMining playerMiningClass;
    public PlayerController PlayerControllerClass;
    public PlayerInventory PlayerInventoryClass;

    public LayerMask layerMask;

    private void Awake() {
        playerMiningClass = GetComponent<PlayerMining>();
        PlayerControllerClass = GetComponent<PlayerController>();
        PlayerInventoryClass = GetComponent<PlayerInventory>();
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

    public void UpdatePickaxeSprite() {
        //throw new NotImplementedException();
    }
}
