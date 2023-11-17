using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;


public class TimerScript : MonoBehaviour
{
    public float timeInSeconds; // Total time in seconds.
    private float currentTime;
    private bool isTimerRunning;
    public TimerScript timerScript;
    private bool isPaused = true; // Pause the game after clicking an option

    public Text timerText; // Reference to the UI Text component to display the timer.

    private void Start()
    {
        currentTime = timeInSeconds;
        UpdateTimerText();
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        StartCoroutine(UpdateTimer());
    }

    public void PauseTimer()
    {
        isPaused = true; // Pause the game after clicking an option
    }

    private IEnumerator UpdateTimer()
{
    while (isTimerRunning)
    {
        if (!isPaused)
        {
            yield return new WaitForSeconds(1f);
            currentTime -= 1f;

            if (currentTime <= 0f)
            {
                // Handle timer completion here if needed.
            }

            UpdateTimerText();
        }
        else
        {
            // If paused, simply yield without updating the timer.
            yield return null;
        }
    }
}


    private void UpdateTimerText()
    {
        // Convert time to TimeSpan format to display minutes and seconds.
        TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);
        timerText.text = timeSpan.ToString("mm':'ss");
    }
}
