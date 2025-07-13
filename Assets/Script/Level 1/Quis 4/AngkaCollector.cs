using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngkaCollector : MonoBehaviour
{
    public static AngkaCollector instance;
    public GameObject feedbackBenarUI;
    public GameObject feedbackSalahUI;

    private AngkaItem currentAngka;
    private List<int> collectedIDs = new List<int>();

    public GameObject collectButton;

    [Header("UI Tampilan Angka yang Diambil")]
    public Image[] angkaSlotUI;            // Image slot untuk menampilkan angka
    public Sprite[] angkaSprites;          // Sprite angka dari 1-10

    private bool misiSelesai = false;

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

            if (collectedIDs.Count >= 10 && !misiSelesai)
            {
                bool benar = CekUrutan();
                PlayerPrefs.SetInt("L1M4", benar ? 100 : 0);
                PlayerPrefs.Save();
                misiSelesai = true;

                // Panggil fungsi di NPC
                FindObjectOfType<PaAsepController>()?.NotifyMisiSelesai(benar);
            }
        }
    }

    private bool CekUrutan()
    {
        for (int i = 0; i < collectedIDs.Count; i++)
        {
            if (collectedIDs[i] != i + 1)
            {
                return false;
            }
        }
        return true;
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

    public void ResetCollectedAngkaUI()
    {
        collectedIDs.Clear();
        misiSelesai = false;
        ClearSlotUI();
        HideCollectButton();
        Debug.Log("🔁 Semua angka di-reset.");
    }

    public bool IsMissionComplete()
    {
        return misiSelesai;
    }
}
