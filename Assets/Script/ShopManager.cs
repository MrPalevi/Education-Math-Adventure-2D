using System.Collections;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Referensi Manager")]
    public GameObject shopPanel;
    public HealtManager healtManager; 
    public CoinCollector coinCollector; 
    public PlayerMovement playerMovement; 

    [Header("UI Feedback")]
    public GameObject fullHeartUI; 
    public GameObject coinEmptyUI;
    public GameObject maxUI;
    public GameObject suksesUI;

    [Header("Shop Settings")]
    public int heartPrice = 1;
    public int runPrice = 3;
    public int maxHealth = 3;
    public int speedIncrease = 20;
    public int maxRunPurchase = 5;

    private string coinPrefsKey;
    private string speedPrefsKey = "PlayerSpeed";
    private string runBuyCountKey = "RunBuyCount";

    void Start()
    {
        if (coinCollector != null)
            coinPrefsKey = coinCollector.coinPrefsKey;

        // Load data speed dari PlayerPrefs
        if (playerMovement != null)
        {
            int savedSpeed = PlayerPrefs.GetInt(speedPrefsKey, (int)playerMovement.speed);
            playerMovement.speed = savedSpeed;
        }

        // Pastikan semua UI feedback mati saat awal
        if (fullHeartUI != null) fullHeartUI.SetActive(false);
        if (coinEmptyUI != null) coinEmptyUI.SetActive(false);
        if (maxUI != null) maxUI.SetActive(false);
        if (suksesUI != null) suksesUI.SetActive(false);
    }

    public void BuyHeart()
    {
        int currentCoins = PlayerPrefs.GetInt(coinPrefsKey, 0);

        // cek apakah sudah full health
        if (healtManager != null && healtManager.GetCurrentHealth() >= maxHealth)
        {
            StartCoroutine(ShowUI(fullHeartUI, 2f));
            return;
        }

        // cek apakah cukup coin
        if (currentCoins < heartPrice)
        {
            StartCoroutine(ShowUI(coinEmptyUI, 2f));
            return;
        }

        // Kurangi coin
        currentCoins -= heartPrice;
        PlayerPrefs.SetInt(coinPrefsKey, currentCoins);
        PlayerPrefs.Save();

        // Update coin UI
        if (coinCollector != null)
            coinCollector.RefreshCoinUI();

        // Tambah heart via HealtManager
        if (healtManager != null)
            healtManager.AddHealth(1);  // fungsi ini dari HealtManager

        StartCoroutine(ShowUI(suksesUI, 2f));
    }

    public void BuyRun()
    {
        int currentCoins = PlayerPrefs.GetInt(coinPrefsKey, 0);
        int runBuyCount = PlayerPrefs.GetInt(runBuyCountKey, 0);

        if (runBuyCount >= maxRunPurchase)
        {
            StartCoroutine(ShowUI(maxUI, 2f));
            return;
        }

        if (currentCoins < runPrice)
        {
            StartCoroutine(ShowUI(coinEmptyUI, 2f));
            return;
        }

        // Pembelian berhasil
        currentCoins -= runPrice;
        PlayerPrefs.SetInt(coinPrefsKey, currentCoins);

        runBuyCount++;
        PlayerPrefs.SetInt(runBuyCountKey, runBuyCount);

        if (playerMovement != null)
        {
            playerMovement.speed += speedIncrease;
            PlayerPrefs.SetInt(speedPrefsKey, (int)playerMovement.speed);
        }

        PlayerPrefs.Save();

        // Update coin UI
        if (coinCollector != null)
            coinCollector.RefreshCoinUI();

        StartCoroutine(ShowUI(suksesUI, 2f));
    }

    private IEnumerator ShowUI(GameObject ui, float duration)
    {
        if (ui == null) yield break;

        ui.SetActive(true);
        yield return new WaitForSeconds(duration);
        ui.SetActive(false);
    }

    public void ShopPanel()
    {
        if (shopPanel != null)
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }
}
