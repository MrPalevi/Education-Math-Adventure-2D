using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngkaCollector : MonoBehaviour
{
    public static AngkaCollector instance;

    private AngkaItem currentAngka;
    private List<int> collectedIDs = new List<int>();

    public GameObject collectButton;
    public GameObject feedbackBenarUI;
    public GameObject feedbackSalahUI;

    [Header("UI Tampilan Angka yang Diambil")]
    public Image[] angkaSlotUI;            // Image slot untuk menampilkan angka
    public Sprite[] angkaSprites;          // Sprite angka dari 1-10

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        collectButton.SetActive(false);
        feedbackBenarUI.SetActive(false);
        feedbackSalahUI.SetActive(false);

        ClearSlotUI(); // Pastikan UI kosong saat mulai
    }

    public void ShowCollectButton(AngkaItem angka)
    {
        currentAngka = angka;
        collectButton.SetActive(true);
    }

    public void HideCollectButton()
    {
        currentAngka = null;
        collectButton.SetActive(false);
    }

    public void TakeAngka()
    {
        if (currentAngka != null)
        {
            int id = currentAngka.angkaID;
            collectedIDs.Add(id);
            ShowAngkaInUI(id);
            Destroy(currentAngka.gameObject);
            HideCollectButton();

            if (collectedIDs.Count >= 10)
            {
                CheckAngkaOrder();
            }
        }
    }

    private void ShowAngkaInUI(int id)
    {
        if (collectedIDs.Count <= angkaSlotUI.Length && id > 0 && id <= angkaSprites.Length)
        {
            int index = collectedIDs.Count - 1;
            angkaSlotUI[index].sprite = angkaSprites[id - 1];
            angkaSlotUI[index].enabled = true;
        }
    }

    private void ClearSlotUI()
    {
        foreach (Image img in angkaSlotUI)
        {
            img.sprite = null;
            img.enabled = false;
        }
    }

    private void CheckAngkaOrder()
    {
        for (int i = 0; i < collectedIDs.Count; i++)
        {
            if (collectedIDs[i] != i + 1)
            {
                ShowFeedback(false);
                PlayerPrefs.SetInt("L1M4", 0);
                PlayerPrefs.Save();
                return;
            }
        }

        ShowFeedback(true);
        PlayerPrefs.SetInt("L1M4", 100);
        PlayerPrefs.Save();
    }

    private void ShowFeedback(bool isCorrect)
    {
        if (isCorrect)
        {
            feedbackBenarUI.SetActive(true);
            Debug.Log("✅ Jawaban Benar, skor 100 disimpan ke L1M4");
        }
        else
        {
            feedbackSalahUI.SetActive(true);
            Debug.Log("❌ Jawaban Salah, skor 0 disimpan ke L1M4");
        }

        StartCoroutine(HideFeedbackAfterDelay());
    }

    private IEnumerator HideFeedbackAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        feedbackBenarUI.SetActive(false);
        feedbackSalahUI.SetActive(false);
    }

    // 🔁 Fungsi reset untuk tes ulang
    public void ResetCollectedAngkaUI()
    {
        collectedIDs.Clear();
        ClearSlotUI();
        HideCollectButton();
        Debug.Log("🔁 Semua angka di-reset.");
    }
}
