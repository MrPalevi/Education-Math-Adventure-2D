using System.Collections;
using UnityEngine;
using TMPro;

[System.Serializable]
public class ItemTarget
{
    public string itemName;       
    public int requiredAmount;    
    public int collectedAmount; 
}

public class ItemCollector : MonoBehaviour
{
    public static ItemCollector instance;

    [Header("Target Item")]
    public ItemTarget[] itemTargets;    // Semua item yang harus dikumpulkan

    [Header("UI")]
    public TMP_Text[] countTexts;       
    public Color normalTextColor = Color.white; // Warna default
    public Color completedTextColor = Color.green; // Warna saat item sudah lengkap

    [Header("UI Tombol & NPC")]
    public GameObject collectButton;
    public GameObject returnToNPCText;

    private GameObject currentItem;
    private string currentItemType;
    private bool canCollect = false;
    private bool missionStarted = false;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        instance = this;
    }

    public void StartCollecting()
    {
        missionStarted = true;

        // Reset semua item
        foreach (var item in itemTargets)
        {
            item.collectedAmount = 0;
        }

        UpdateUI();
    }

    void Update()
    {
        if (canCollect && Input.GetKeyDown(KeyCode.E))
        {
            TakeItem();
        }
    }

    public void ShowCollectButton(GameObject itemObject, string itemType)
    {
        currentItem = itemObject;
        currentItemType = itemType;
        canCollect = true;
        collectButton.SetActive(true);
    }

    public void HideCollectButton()
    {
        currentItem = null;
        currentItemType = "";
        canCollect = false;
        collectButton.SetActive(false);
    }

    public void TakeItem()
    {
        if (currentItem != null)
        {
            bool foundMatch = false;

            for (int i = 0; i < itemTargets.Length; i++)
            {
                // ✅ Pengecekan EXACT MATCH, case-sensitive
                if (string.Equals(itemTargets[i].itemName, currentItemType, System.StringComparison.Ordinal))
                {
                    audioManager.PlaySFX(audioManager.Take);
                    itemTargets[i].collectedAmount++;
                    foundMatch = true;
                    break; // ✅ Hentikan perulangan setelah ketemu
                }
            }

            if (!foundMatch)
            {
                Debug.LogWarning($"❌ Tidak ditemukan target untuk item: {currentItemType}");
            }

            Destroy(currentItem);
            UpdateUI();
            HideCollectButton();

            if (HasCompletedMission())
            {
                returnToNPCText.SetActive(true);
            }
        }
    }

    public bool HasCompletedMission()
    {
        foreach (var item in itemTargets)
        {
            if (item.collectedAmount < item.requiredAmount)
                return false;
        }
        return true;
    }

    public bool IsMissionStarted()
    {
        return missionStarted;
    }

    void UpdateUI()
    {
        for (int i = 0; i < itemTargets.Length; i++)
        {
            if (i < countTexts.Length && countTexts[i] != null)
            {
                countTexts[i].text = $"{itemTargets[i].collectedAmount} / {itemTargets[i].requiredAmount}";

                if (itemTargets[i].collectedAmount >= itemTargets[i].requiredAmount)
                    countTexts[i].color = completedTextColor;
                else
                    countTexts[i].color = normalTextColor;
            }
        }
    }
}
