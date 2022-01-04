using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableTile : MonoBehaviour {

    [SerializeField] private GameObject uiElement = null;

    public void Interact() {
        if(uiElement != null)
            uiElement.SetActive(true);
    }
}
