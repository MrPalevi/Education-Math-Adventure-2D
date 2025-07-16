using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultManager : MonoBehaviour
{
    [Header("Panel Hasil")]
    public GameObject panelResult;

    [Header("PlayerPrefs Score Keys")]
    public string[] playerPrefsKeys; // Misalnya: ["L1M1", "L1M2", "L1M3"]

    [Header("Key untuk Total Score")]
    public string playerPrefsScoreFinalKey = "TotalFinalScore";

    [Header("Tampilan Skor")]
    public TextMeshProUGUI scoreText;

    [Header("Skor & Bintang")]
    public int[] scoreThresholds; // Urutan menurun, contoh: [300, 200, 100, 0]

    public GameObject star1Object;
    public GameObject star2Object;
    public GameObject star3Object;

    public Image resultImage;
    public Sprite[] resultSprites; // [0 bintang, 1 bintang, 2 bintang, 3 bintang]

    private int totalScore = 0;

    public void ShowResult()
    {
        totalScore = 0;

        foreach (string key in playerPrefsKeys)
        {
            if (PlayerPrefs.HasKey(key))
                totalScore += PlayerPrefs.GetInt(key);
        }

        PlayerPrefs.SetInt(playerPrefsScoreFinalKey, totalScore);
        PlayerPrefs.Save();

        if (scoreText != null)
            scoreText.text = totalScore.ToString();

        int index = -1;
        for (int i = 0; i < scoreThresholds.Length; i++)
        {
            if (totalScore == scoreThresholds[i])
            {
                index = i;
                break;
            }
        }

        // Default ke index terakhir jika tidak cocok
        if (index == -1)
            index = scoreThresholds.Length - 1;

        // Set bintang berdasarkan index
        star1Object.SetActive(index <= 2);
        star2Object.SetActive(index <= 1);
        star3Object.SetActive(index <= 0);

        // Set ilustrasi hasil
        if (resultImage != null && resultSprites.Length > index)
        {
            resultImage.sprite = resultSprites[index];
        }

        panelResult?.SetActive(true);
    }
}
