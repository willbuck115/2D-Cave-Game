using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sky : MonoBehaviour {

    private Weather currentWeather = Weather.Sunny;

    private int cloudsToGenerate = 0;
    private int baseCloudsToGenerate = 20;
    [SerializeField] private float minY, maxY;
    private float xDifferential;
    [SerializeField] private Transform cloudParent;
    private decimal chanceValue;

    private Color cloudColor;
    [SerializeField] private GameObject[] cloudPrefabs;
    [SerializeField] private Material cloudMaterial;

    private float cloudMoveSpeed;
    [SerializeField] private float baselineCloudMoveSpeed = 1;

    internal void GenerateDailyWeather(decimal luckValue) {
        float floatChance = (float)luckValue;
        bool shouldLuckImpact = RandomBool();

        if(currentWeather == Weather.Thunder) {
            currentWeather = Weather.Sunny;
        }

        // Luck will have an impact
        if (shouldLuckImpact) {
            floatChance = Random.value;
            if (floatChance <= 0.15f) {
                currentWeather = Weather.Thunder;
            } else if (floatChance <= 0.35) {
                currentWeather = Weather.Rain;
            } else if (floatChance <= 0.75f) {
                currentWeather = Weather.Cloudy;
            } else if (floatChance <= 1) {
                currentWeather = Weather.Sunny;
            }
        } else {
            currentWeather = Weather.Sunny;
        }


        ActionWeather(floatChance);
    }

    private void ActionWeather(float floatChance) {
        if (cloudParent.childCount > 0) { 
            for(int i = 0; i < cloudParent.childCount; i++) {
                Destroy(cloudParent.GetChild(i).gameObject);
            }
        }

        // calculates clouds to generate trippling the value of 2 - luck (will be a number in the range 1, 2)
        cloudsToGenerate = (int)(baseCloudsToGenerate * (2*(2 - floatChance)));

        // clamp so not too dark
        if (floatChance > 0.17)
            cloudColor = new Color(floatChance, floatChance, floatChance);
        else
            cloudColor = new Color(0.17f, 0.17f, 0.17f);

        cloudMaterial.color = cloudColor;
        cloudMoveSpeed = baselineCloudMoveSpeed * (0.001f*(2 - floatChance));

        float x = 25; // edge of map on left side
        float xMax = 150; // edge of map on right
        xDifferential = (x + xMax) / cloudsToGenerate;
        // to make it balance
        x = x - xDifferential;

        for (int i = 0; i < cloudsToGenerate; i++) {
            // instantiate and edit object properties
            x = x + xDifferential;
            float yValue = Random.Range(minY, maxY);
            int selPrefab = Random.Range(0, cloudPrefabs.Length - 1);
            GameObject cloud = Instantiate(cloudPrefabs[selPrefab], cloudParent);
            cloud.transform.position = new Vector2(x, yValue);
            CloudMovement movement = cloud.AddComponent<CloudMovement>();
            movement.speed = cloudMoveSpeed;
        }
    }

    private bool RandomBool() {
        int rand = Random.Range(0, 1);

        return rand == 1;
    }
}

public enum Weather {
    Sunny = 0,
    Cloudy = 1,
    Rain = 2,
    Thunder = 3,
    // maybe snow??
}
