using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons;
    private string unlockPrefsKey = "UnlockedLevel";

    void Start()
    {
        RestoreUnlockedLevels(); // langsung dipanggil saat scene dimulai
    }

    // Fungsi untuk menyimpan level yang berhasil dibuka
    public void UnlockLevel(int levelIndex)
    {
        int highestLevel = PlayerPrefs.GetInt(unlockPrefsKey, 1);

        if (levelIndex > highestLevel)
        {
            PlayerPrefs.SetInt(unlockPrefsKey, levelIndex);
            PlayerPrefs.Save();
        }
    }

    // Fungsi untuk mengembalikan status level yang sudah di-unlock
    public void RestoreUnlockedLevels()
    {
        int highestLevel = PlayerPrefs.GetInt(unlockPrefsKey, 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            if (i < highestLevel)
                levelButtons[i].interactable = true; // level sudah terbuka
            else
                levelButtons[i].interactable = false; // level terkunci
        }
    }
}
