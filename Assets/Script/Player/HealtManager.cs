using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealtManager : MonoBehaviour
{
    public static int health = 3;
    public int maxHealth = 3;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite empetyHeart;

    private string healthPrefsKey = "PlayerHealth";

    void Awake()
    {
        // ✅ Load health dari PlayerPrefs
        if (PlayerPrefs.HasKey(healthPrefsKey))
        {
            health = PlayerPrefs.GetInt(healthPrefsKey);
        }
        else
        {
            health = maxHealth; // kalau pertama kali main → full health
            PlayerPrefs.SetInt(healthPrefsKey, health);
            PlayerPrefs.Save();
        }
    }

    void Update()
    {
        foreach (Image img in hearts)
        {
            img.sprite = empetyHeart;
        }

        for (int i = 0; i < health; i++)
        {
            hearts[i].sprite = fullHeart;
        }
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public void AddHealth(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        SaveHealth();
    }

    public void TakeDamage(int amount)
    {
        health = Mathf.Clamp(health - amount, 0, maxHealth);
        SaveHealth();
    }

    // ✅ Simpan nyawa terakhir
    private void SaveHealth()
    {
        PlayerPrefs.SetInt(healthPrefsKey, health);
        PlayerPrefs.Save();
    }

    public void ResetHealth()
    {
        health = maxHealth;
        SaveHealth();
    }

    // ✅ Pastikan tersimpan saat ganti scene
    private void OnDisable()
    {
        SaveHealth();
    }

    private void OnApplicationQuit()
    {
        SaveHealth();
    }
}
