using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetCoinCont : MonoBehaviour
{
    [Header("Teks untuk Coin")]
    [Tooltip("Semua teks yang ingin menampilkan jumlah coin.")]
    public TMP_Text[] teksCoinArray;

    private const string coinPrefsKey = "TotalCoins";
   
    void Start()
    {
        UpdateCoin();
    }

    // Update is called once per frame
    void Update()
    {
        int currentCoins = PlayerPrefs.GetInt(coinPrefsKey, 0);
        foreach (var teks in teksCoinArray)
        {
            if (teks != null && teks.text != currentCoins.ToString())
            {
                teks.text = "x" + currentCoins.ToString();
            }
        }
    }

    public void UpdateCoin()
    {
        int coins = PlayerPrefs.GetInt(coinPrefsKey, 0);

        foreach (var teks in teksCoinArray)
        {
            if (teks != null)
                teks.text = "x" + coins.ToString();
        }
    }
}
