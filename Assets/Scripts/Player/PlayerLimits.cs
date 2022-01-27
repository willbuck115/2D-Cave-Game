using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLimits : MonoBehaviour {
    
    [Range(0, 100000)] private int currentStamina;
    [Range(37, 87)] private float currentHeat = 37;
    [Range(0, 1000000)] private float currentBank = 0;

    private float[] temperatureToYMap;

    internal int CurrentStamina { get { return currentStamina; }
        set { currentStamina = value; if (value <= 0) { OnStaminaDepleted(); } } }

    // possibly add fuel value here?
    internal int CurrentBank { get; set; }

    internal float CurrentHeat { get { currentHeat = OnHeatUpdate(); return currentHeat; }
        set { currentHeat = value; if (value >= currentMaximumHeat) { OnHeatThresholdHit(); } } }

    [SerializeField] internal int currentMaximumStamina = 200;
    [SerializeField] internal int currentMaximumHeat = 42;

    [SerializeField] private WorldGeneration worldGeneratorClass;

    private void Start() {
        temperatureToYMap = new float[worldGeneratorClass.yHeight];
        for(int y = 0; y < temperatureToYMap.Length; y++) {
            temperatureToYMap[y] = y * .05f;
        }

        currentStamina = currentMaximumStamina;
    }

    private void OnStaminaDepleted() { 
        // Game over?
        // Or just stardew valley escque consequences such as loss of items or money?
    }

    private void OnHeatThresholdHit() {
        // Game over?
        // Or just stardew valley escque consequences such as loss of items or money?
    }

    internal float OnHeatUpdate() {
        float h = 37;
        int unitsBelowGround = (worldGeneratorClass.yHeight - (int)transform.position.y) + 1;
        if (unitsBelowGround > -1) {
            h = h + temperatureToYMap[unitsBelowGround];
        }

        return h;
    }

    internal void OnStaminaUpdate(int v) {
        CurrentStamina = CurrentStamina + v;
    }

    internal bool OnBankUpdate(float m) {
        float money = currentBank;
        if(m < 0) {
            // Wants to subtract money for purchase
            if(m <= money) {
                // Can be subtracted
                currentBank = currentBank - m;
                return true;
            } else {
                return false;
            }
        } else {
            // Wants to add money
            currentBank = currentBank + m;
            return true;
        }
    }

    internal void OnNewDayStart(int poorSleepCount) {
        if(poorSleepCount > 0)
            currentStamina = currentMaximumStamina / poorSleepCount;
    }
}
