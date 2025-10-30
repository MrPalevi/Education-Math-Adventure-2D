using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoinCollector : MonoBehaviour
{
    [Header("Daftar Coin di Scene")]
    public GameObject[] coins;

    [Header("UI")]
    public TextMeshProUGUI coinText;
    public GameObject pickupButtonUI;

    [Header("PlayerPrefs Key")]
    public string coinPrefsKey = "TotalCoins";

    private GameObject currentCoin;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();
    }

    void Start()
    {
        if (pickupButtonUI != null) pickupButtonUI.SetActive(false);
        RefreshCoinUI(); // Selalu sinkron dengan PlayerPrefs saat start
    }

    // --- Panggil ini setiap kali koin berubah dari mana pun (Shop, Collect, dsb)
    public void RefreshCoinUI()
    {
        int coins = PlayerPrefs.GetInt(coinPrefsKey, 0);
        if (coinText != null)
            coinText.text = "x" + coins;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsCoin(other.gameObject))
        {
            currentCoin = other.gameObject;
            if (pickupButtonUI != null) pickupButtonUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == currentCoin)
        {
            currentCoin = null;
            if (pickupButtonUI != null) pickupButtonUI.SetActive(false);
        }
    }

    public void CollectCoin()
    {
        if (currentCoin == null) return;

        // Tambah coin langsung ke PlayerPrefs agar sumber kebenaran satu tempat
        int current = PlayerPrefs.GetInt(coinPrefsKey, 0) + 5;
        PlayerPrefs.SetInt(coinPrefsKey, current);
        PlayerPrefs.Save();

        // Update UI realtime
        RefreshCoinUI();

        // Hapus/disable coin
        currentCoin.SetActive(false);
        audioManager?.PlaySFX(audioManager.TakeCoin);
        currentCoin = null;
        if (pickupButtonUI != null) pickupButtonUI.SetActive(false);
    }

    private bool IsCoin(GameObject obj)
    {
        foreach (var coin in coins)
            if (coin == obj) return true;
        return false;
    }
}
