using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelUnlockData
    {
        public string playerPrefsKey;              
        public Button levelButton;                 
        public GameObject imageLock;               
        public GameObject[] imageStars = new GameObject[3];

        public int scoreFor1Star = 100;            
        public int scoreFor2Stars = 200;           
        public int scoreFor3Stars = 300;

        public GameObject[] imageArchiveAngka = new GameObject[2];
        public Image[] imageArchiveProfile = new Image[2];
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
    public GameObject panelArchive;
    public GameObject panelSelamat;

    [Header("Panel UI")]
    public GameObject panelProgres;
    public GameObject panelAngka;

    [Header("Input & Display Nama")]
    public TMP_InputField inputFieldNama;       
    public TMP_Text textTmproNamaUser;          

    [Header("Level Unlock Settings")]
    public LevelUnlockData[] levelUnlockList;

    private const string playerPrefsKey = "NamaUser";
    AudioManager audioManager;
    private bool panelSelamatSiap = false;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        if (panelWelcome != null)
            panelWelcome.SetActive(false);

        if (panelSelamat != null)
            panelSelamat.SetActive(false);

        // Restore archive profile image color if needed
        for (int i = 0; i < levelUnlockList.Length; i++)
        {
            string colorKey = "ArchiveProfileUpdated_" + levelUnlockList[i].playerPrefsKey;
            if (PlayerPrefs.GetInt(colorKey, 0) == 1)
            {
                if (levelUnlockList[i].imageArchiveProfile != null)
                {
                    foreach (var img in levelUnlockList[i].imageArchiveProfile)
                    {
                        if (img != null)
                            img.color = new Color32(255, 255, 255, 255);
                    }
                }
            }
        }

        CekNamaUser();
        CekUnlockLevel();

        string targetPanel = PlayerPrefs.GetString("MainMenuTargetPanel", "");
        if (targetPanel == "Level")
        {
            BukaPanelLevel();
            PlayerPrefs.DeleteKey("MainMenuTargetPanel");
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
        audioManager.PlaySFX(audioManager.button);

        if (!string.IsNullOrEmpty(nama))
        {
            PlayerPrefs.SetString(playerPrefsKey, nama);
            PlayerPrefs.Save();

            if (textTmproNamaUser != null)
                textTmproNamaUser.text = nama;

            panelInputNama?.SetActive(false);
            if (panelWelcome != null)
                panelWelcome.SetActive(true);

            audioManager.PlaySFX(audioManager.Welcome);
        }
        else
        {
            Debug.LogWarning("Nama tidak boleh kosong.");
        }
    }

    void CekUnlockLevel()
    {
        for (int i = 0; i < levelUnlockList.Length; i++)
        {
            LevelUnlockData data = levelUnlockList[i];
            int score = PlayerPrefs.GetInt(data.playerPrefsKey, 0);

            if (data.levelButton != null)
                data.levelButton.interactable = false;

            if (data.imageLock != null)
                data.imageLock.SetActive(true);

            if (data.imageStars != null)
            {
                foreach (var star in data.imageStars)
                {
                    if (star != null)
                        star.SetActive(false);
                }
            }

            int bintang = 0;
            if (score >= data.scoreFor3Stars)
                bintang = 3;
            else if (score >= data.scoreFor2Stars)
                bintang = 2;
            else if (score >= data.scoreFor1Star)
                bintang = 1;

            for (int s = 0; s < bintang && s < data.imageStars.Length; s++)
            {
                if (data.imageStars[s] != null)
                    data.imageStars[s].SetActive(true);
            }

            if (i == 0 || PlayerPrefs.GetInt(levelUnlockList[i - 1].playerPrefsKey, 0) >= levelUnlockList[i - 1].scoreFor1Star)
            {
                if (data.levelButton != null)
                    data.levelButton.interactable = true;
                if (data.imageLock != null)
                    data.imageLock.SetActive(false);
            }

            string archiveKey = "ArchiveShown_" + data.playerPrefsKey;
            string colorKey = "ArchiveProfileUpdated_" + data.playerPrefsKey;

            if (score >= data.scoreFor1Star && PlayerPrefs.GetInt(archiveKey, 0) == 0)
            {
                if (panelArchive != null)
                    panelArchive.SetActive(true);

                if (data.imageArchiveAngka != null)
                {
                    foreach (var angka in data.imageArchiveAngka)
                    {
                        if (angka != null)
                            angka.SetActive(true);
                    }
                }

                if (data.imageArchiveProfile != null)
                {
                    foreach (var img in data.imageArchiveProfile)
                    {
                        if (img != null)
                            img.color = new Color32(255, 255, 255, 255);
                    }
                }

                PlayerPrefs.SetInt(archiveKey, 1);
                PlayerPrefs.SetInt(colorKey, 1);
                PlayerPrefs.Save();

                if (i == levelUnlockList.Length - 1 && PlayerPrefs.GetInt("SelamatDitampilkan", 0) == 0)
                {
                    panelSelamatSiap = true;
                }
            }
            else
            {
                if (panelArchive != null && !panelArchive.activeSelf)
                {
                    if (data.imageArchiveAngka != null)
                    {
                        foreach (var angka in data.imageArchiveAngka)
                        {
                            if (angka != null)
                                angka.SetActive(false);
                        }
                    }
                }
            }
        }
    }

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
        audioManager.PlaySFX(audioManager.button);
    }

    public void TutupPanelWelcome()
    {
        if (panelWelcome != null)
            panelWelcome.SetActive(false);

        audioManager.PlaySFX(audioManager.button);
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
        audioManager.PlaySFX(audioManager.button);
    }

    public void PanelProgres()
    {
        if (panelProgres != null && panelAngka != null)
        {
            bool isProgresActive = !panelProgres.activeSelf;
            panelProgres.SetActive(isProgresActive);

            panelAngka.SetActive(!isProgresActive);
            audioManager.PlaySFX(audioManager.button);
        }
    }

    public void LoadSceneName(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
            audioManager.PlaySFX(audioManager.button);
        }
    }

    public void TutupPanelArchive()
    {
        if (panelArchive != null)
            panelArchive.SetActive(false);

        foreach (var data in levelUnlockList)
        {
            if (data.imageArchiveAngka != null)
            {
                foreach (var angka in data.imageArchiveAngka)
                {
                    if (angka != null)
                        angka.SetActive(false);
                }
            }
        }

        if (panelSelamatSiap && panelSelamat != null && PlayerPrefs.GetInt("SelamatDitampilkan", 0) == 0)
        {
            panelSelamat.SetActive(true);
            PlayerPrefs.SetInt("SelamatDitampilkan", 1);
            PlayerPrefs.Save();
        }

        audioManager.PlaySFX(audioManager.button);
    }

    public void ResumePermainan()
    {
        if (PlayerPrefs.HasKey("LastScenePlayed"))
        {
            string sceneToLoad = PlayerPrefs.GetString("LastScenePlayed");
            SceneManager.LoadScene(sceneToLoad);
            audioManager.PlaySFX(audioManager.button);
        }
        else
        {
            Debug.Log("Belum ada data scene disimpan, mulai dari Level 1.");
            SceneManager.LoadScene("Level 1");
            audioManager.PlaySFX(audioManager.button);
        }
    }

    public void TutupPanelSelamat()
    {
        panelSelamat.SetActive(false);
    }
}
