using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class SoalData
{
    public string namaItem;      // Contoh: "Kotak", "Segitiga"
    public Sprite gambarSoal;    // Bisa null
    public string teksSoal;      // Bisa kosong
}

public class RandomSoalL7M2 : MonoBehaviour
{
    [Header("Daftar Soal")]
    public List<SoalData> soalList = new List<SoalData>();
    private int currentIndex = 0;

    [Header("UI Soal")]
    public GameObject uiSoalPanel;
    public Image soalImage;
    public TextMeshProUGUI soalText;

    [Header("Feedback Jawaban (ambil item)")]
    public GameObject feedbackBenar;
    public GameObject feedbackSalah;

    private NpcControllerL7M2 npcController;
    private bool misiBerjalan = false;
    private bool adaJawabanSalah = false;

    void Start()
    {
        if (uiSoalPanel != null) uiSoalPanel.SetActive(false);
        if (feedbackBenar != null) feedbackBenar.SetActive(false);
        if (feedbackSalah != null) feedbackSalah.SetActive(false);

        npcController = FindObjectOfType<NpcControllerL7M2>();
    }

    public void MulaiMisi()
    {
        if (soalList.Count == 0)
        {
            Debug.LogWarning("Soal kosong, misi tidak dijalankan.");
            return;
        }

        misiBerjalan = true;
        adaJawabanSalah = false;
        currentIndex = 0;

        TampilkanSoal();
    }

    void TampilkanSoal()
    {
        if (currentIndex >= soalList.Count)
        {
            SelesaikanMisi();
            return;
        }

        if (uiSoalPanel != null) uiSoalPanel.SetActive(true);

        var soal = soalList[currentIndex];
        if (soalImage != null) soalImage.sprite = soal.gambarSoal;
        if (soalText != null) soalText.text = soal.teksSoal;
    }

    public void PeriksaJawaban(string namaItem)
    {
        if (!misiBerjalan) return;

        string targetNama = soalList[currentIndex].namaItem;

        if (namaItem == targetNama)
        {
            StartCoroutine(TampilkanFeedback(feedbackBenar));
        }
        else
        {
            adaJawabanSalah = true;
            StartCoroutine(TampilkanFeedback(feedbackSalah));
        }
    }

    IEnumerator TampilkanFeedback(GameObject feedbackObj)
    {
        if (feedbackObj != null)
        {
            feedbackObj.SetActive(true);
            yield return new WaitForSeconds(2f);
            feedbackObj.SetActive(false);
        }

        currentIndex++;
        yield return new WaitForSeconds(0.3f);

        TampilkanSoal();
    }

    void SelesaikanMisi()
    {
        misiBerjalan = false;
        if (uiSoalPanel != null) uiSoalPanel.SetActive(false);

        bool semuaBenar = !adaJawabanSalah;

        if (npcController != null)
        {
            npcController.OnMissionCompleted(semuaBenar);
        }
    }
}
