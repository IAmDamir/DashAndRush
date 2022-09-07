using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TMPro.TextMeshProUGUI timeText;
    public float time = 0f;
    private bool isPlayerAlive = true;

    // Update is called once per frame
    private void Update()
    {
        if (isPlayerAlive)
        {
            time += Time.deltaTime;
            DisplayTime(time);
        }
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isPlayerAlive = false;
    }

    public float GetTime()
    {
        return time;
    }
}