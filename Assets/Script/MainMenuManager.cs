using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelUnlockData
    {
        public string playerPrefsKey;           // Nama key yang dicek
        public Button levelButton;              // Tombol level
        public GameObject imageLock;            // Gambar kunci
        public GameObject[] imageStars = new GameObject[3];  // Bintang (maks 3)
    }

    [Header("Panel UI")]
    public GameObject judulGame;
    public GameObject BGprofile;
    public GameObject panelButton;
    public GameObject panelInputNama;
    public GameObject panelLevel;
    public GameObject panelSetting;
    public GameObject panelAbout;
    public GameObject panelProfile;
    public GameObject panelWelcome;

    [Header("Panel UI")]
    public GameObject panelProgres;

    [Header("Input & Display Nama")]
    public TMP_InputField inputFieldNama;       
    public TMP_Text textTmproNamaUser;          

    [Header("Level Unlock Settings")]
    public LevelUnlockData[] levelUnlockList;   // Daftar level yang ingin dicek

    private const string playerPrefsKey = "NamaUser";

    void Start()
    {
        if (panelWelcome != null)
            panelWelcome.SetActive(false);

        CekNamaUser();
        CekUnlockLevel();

        string targetPanel = PlayerPrefs.GetString("MainMenuTargetPanel", "");
        if (targetPanel == "Level")
        {
            BukaPanelLevel();
            PlayerPrefs.DeleteKey("MainMenuTargetPanel"); // Hapus agar tidak berulang
        }
    }

    void CekNamaUser()
    {
        string savedName = PlayerPrefs.GetString(playerPrefsKey, "");

        if (string.IsNullOrEmpty(savedName))
        {
            panelInputNama?.SetActive(true);
        }
        else
        {
            if (textTmproNamaUser != null)
                textTmproNamaUser.text = savedName;

            panelInputNama?.SetActive(false);
        }
    }

    public void SimpanNama()
    {
        string nama = inputFieldNama.text.Trim();

        if (!string.IsNullOrEmpty(nama))
        {
            PlayerPrefs.SetString(playerPrefsKey, nama);
            PlayerPrefs.Save();

            if (textTmproNamaUser != null)
                textTmproNamaUser.text = nama;

            panelInputNama?.SetActive(false);
            if (panelWelcome != null)
                panelWelcome.SetActive(true); // Tampilkan panel selamat datang
        }
        else
        {
            Debug.LogWarning("Nama tidak boleh kosong.");
        }
    }

    void CekUnlockLevel()
    {
        foreach (LevelUnlockData data in levelUnlockList)
        {
            int score = PlayerPrefs.GetInt(data.playerPrefsKey, 0); // default 0

            if (data.levelButton != null)
                data.levelButton.interactable = false;

            if (data.imageLock != null)
                data.imageLock.SetActive(true);

            // Matikan semua bintang dulu
            if (data.imageStars != null)
            {
                foreach (var star in data.imageStars)
                {
                    if (star != null)
                        star.SetActive(false);
                }
            }

            if (score >= 100)
            {
                if (data.levelButton != null)
                    data.levelButton.interactable = true;

                if (data.imageLock != null)
                    data.imageLock.SetActive(false);
            }

            // Aktifkan bintang berdasarkan nilai
            if (data.imageStars != null)
            {
                if (score >= 100 && data.imageStars.Length > 0 && data.imageStars[0] != null)
                    data.imageStars[0].SetActive(true);
                if (score >= 200 && data.imageStars.Length > 1 && data.imageStars[1] != null)
                    data.imageStars[1].SetActive(true);
                if (score > 300 && data.imageStars.Length > 2 && data.imageStars[2] != null)
                    data.imageStars[2].SetActive(true);
            }
        }
    }

    // Panel navigasi
    public void BukaPanelLevel() => ShowOnly(panelLevel);
    public void BukaPanelSetting() => ShowOnly(panelSetting);
    public void BukaPanelAbout() => ShowOnly(panelAbout);
    public void BukaPanelProfile() => ShowOnly(panelProfile);

    private void ShowOnly(GameObject panelToShow)
    {
        judulGame?.SetActive(false);
        BGprofile?.SetActive(false);
        panelButton?.SetActive(false);
        panelLevel?.SetActive(false);
        panelSetting?.SetActive(false);
        panelAbout?.SetActive(false);
        panelProfile?.SetActive(false);

        panelToShow?.SetActive(true);
    }

    public void TutupPanelWelcome()
    {
        if (panelWelcome != null)
            panelWelcome.SetActive(false);
    }

    public void KembaliKeMenuUtama()
    {
        judulGame?.SetActive(true);
        BGprofile?.SetActive(true);
        panelButton?.SetActive(true);
        panelLevel?.SetActive(false);
        panelSetting?.SetActive(false);
        panelAbout?.SetActive(false);
        panelProfile?.SetActive(false);
    }

    public void PanelProgres()
    {
        if (panelProgres != null )
        {
            panelProgres.SetActive(!panelProgres.activeSelf);
        }

    }

    public void LoadSceneName(string sceneName)
    {
        if(!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
