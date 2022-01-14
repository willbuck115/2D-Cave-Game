using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {
    internal decimal luck;
    internal Sky skyClass;

    internal int dayCount;
    // needs to be saved with playerdata

    [SerializeField] private GameObject transitionUI;

    // this is debug
    private void Start() {
        StartNewDay(true);
    }

    internal void StartNewDay(bool isStart) {
        if(!isStart)
            StartCoroutine(DayTransition());
        else {
            StartDay();
        }
    }

    void StartDay() {
        luck = GenerateLuck();
        skyClass = GetComponent<Sky>();
        skyClass.GenerateDailyWeather(luck / 2);

    }

    private IEnumerator DayTransition() {
        Time.timeScale = 4;
        transitionUI.SetActive(true);
        Debug.Log("NOTE: Disable Player Movement");

        yield return new WaitForSecondsRealtime(5);

        Time.timeScale = 1;
        transitionUI.SetActive(false);
        StartDay();
    }

    private decimal GenerateLuck() {
        luck = (decimal)(2 * Random.value);
        // round to 2 decimal places
        decimal.Round(luck, 2);
        return luck;
    }
}
