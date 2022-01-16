using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneratableTile", menuName = "ScriptableObjects/GeneratableTile", order = 1)]
public class GeneratableTile : ScriptableObject {
    public GameObject prefab;
    public AnimationCurve generationGradient;
    public int tileIndex;
    public bool commit = true;
    public Color colour;

    public int ChooseTile(int y) {
        // checks if tile is grass or dirt before continuing
        if (tileIndex == 1 || tileIndex == 2 && commit) {
            return 0;
        } else {
            float rand = Random.value; // between 0 and 1

            if(rand > (1 - generationGradient.Evaluate(y))) {
                return tileIndex;
            } else {
                return 0;
            }
        }
    }
}
