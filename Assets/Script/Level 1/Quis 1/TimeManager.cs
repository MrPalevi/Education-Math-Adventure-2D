using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TimeManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float totalTimeInSeconds = 120f;
    public float timeRemaining;
    private bool timerRunning = false;

    [Header("UI Components")]
    public Slider timeSlider;
    public TMP_Text timeText;

    public Image fillImage;
    public Color fillColor1 = Color.green;
    public Color fillColor2 = Color.yellow;
    public Color fillColor3 = Color.red;

    public GameObject panelTimeOver;

    public Action OnTimeOut; // Tambahkan event waktu habis

    private void Start()
    {
        ResetTimer();
        HideAll();
    }

    private void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateUI();
        }
        else
        {
            timeRemaining = 0;
            timerRunning = false;
            TimeOut();
        }
    }

    public void StartTimer()
    {
        timeRemaining = totalTimeInSeconds;
        timerRunning = true;
        timeSlider.maxValue = totalTimeInSeconds;
        timeSlider.value = totalTimeInSeconds;

        timeSlider.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        panelTimeOver?.SetActive(false);
    }

    public void StopTimer()
    {
        timerRunning = false;
        HideAll();
    }

    void TimeOut()
    {
        Debug.Log("Waktu habis!");
        panelTimeOver?.SetActive(true);
        HideAll();

        OnTimeOut?.Invoke(); // Panggil event
    }

    void UpdateUI()
    {
        timeSlider.value = timeRemaining;

        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        float percentage = (timeRemaining / totalTimeInSeconds) * 100f;

        if (percentage > 60f)
            fillImage.color = fillColor1;
        else if (percentage > 10f)
            fillImage.color = fillColor2;
        else
            fillImage.color = fillColor3;
    }

    void HideAll()
    {
        timeSlider.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        timeRemaining = totalTimeInSeconds;
    }
}
