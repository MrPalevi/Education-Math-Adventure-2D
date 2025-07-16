using UnityEngine;
using TMPro;

public class GlobalLevelTimer : MonoBehaviour
{
    [Header("UI Timer")]
    public TextMeshProUGUI timerText;

    [Header("Control Panel")]
    public GameObject resultPanel; // Panel hasil untuk hentikan timer

    private float elapsedTime = 0f;
    private bool isRunning = true;

    void Start()
    {
        isRunning = true;
    }

    void Update()
    {
        // Jangan lanjut kalau timer berhenti secara manual
        if (!isRunning) return;

        // Jangan jalan saat game dijeda (misalnya Time.timeScale == 0)
        if (Time.timeScale == 0f) return;

        // Tambahkan waktu
        elapsedTime += Time.deltaTime;
        UpdateTimerDisplay();

        // Hentikan timer kalau result panel aktif
        if (resultPanel != null && resultPanel.activeSelf)
        {
            StopTimer();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public float GetFinalTime()
    {
        return elapsedTime;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerDisplay();
        isRunning = true;
    }

    public void SaveTime(string playerPrefsKey)
    {
        PlayerPrefs.SetFloat(playerPrefsKey, elapsedTime);
        PlayerPrefs.Save();
    }
}
