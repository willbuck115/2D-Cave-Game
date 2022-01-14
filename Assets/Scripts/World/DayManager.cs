using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {
    internal decimal luck;
    internal Sky skyClass;

    internal int dayCount;
    // needs to be saved with playerdata

    // this is debug
    private void Start() {
        StartNewDay();
    }

    internal void StartNewDay() {
        luck = GenerateLuck();
        skyClass = GetComponent<Sky>();
        skyClass.GenerateDailyWeather(luck / 2);

        // save the daycount, maybe player prefs?
    }

    private decimal GenerateLuck() {
        luck = (decimal)(2 * Random.value);
        // round to 2 decimal places
        decimal.Round(luck, 2);
        return luck;
    }
}
