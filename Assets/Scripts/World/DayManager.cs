using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour {
    internal decimal luck;
    internal Sky skyClass;
    [SerializeField] private PlayerLimits playerLimitClass;

    internal int dayCount;
    // needs to be saved with playerdata

    private float lastSleptTime;
    // seconds
    private float sleepTimeThreshold = 300;

    // amount of days where the player has gone to sleep before the threshold, this will be used to calculate the days stamina
    // for every day the player has good sleep this is minused by 1
    internal int daysOfPoorSleep;

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
        playerLimitClass.OnNewDayStart(daysOfPoorSleep);
        if (daysOfPoorSleep > 0)
            daysOfPoorSleep--;
        
        lastSleptTime = Time.time;
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

    internal void CalculateSleep() {
        if((Time.time - lastSleptTime) >= sleepTimeThreshold) {
            // player has slept in good time
        } else {
            // forfeit
            daysOfPoorSleep++;
        }

        DayTransition();
    }
}
