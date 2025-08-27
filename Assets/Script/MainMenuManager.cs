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
    public GameObject panelKeluar;
    public GameObject panelResetConfirmasi;
    public GameObject panelResetSukses;

    [Header("Panel UI")]
    public GameObject panelProgres;
    public GameObject panelAngka;

    [Header("Input & Display Nama")]
    public TMP_InputField inputFieldNama;       
    public TMP_Text textTmproNamaUser;          

    [Header("Level Unlock Settings")]
    public LevelUnlockData[] levelUnlockList;

    private const string playerPrefsKey = "NamaUser";
    private bool panelSelamatSiap = false;

    [Header("Resume Settings")]
    public string[] playerPrefsScoreFinalKey; 
    public string[] sceneNameList;              

    AudioManager audioManager;

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
        // BGprofile?.SetActive(false);
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

            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                // Langsung resume ke scene terakhir
                SceneManager.LoadScene(sceneToLoad);
                return;
            }
        }

        // Jika LastScenePlayed kosong atau tidak valid → cari scene terakhir yang sudah memiliki skor >= 100
        for (int i = playerPrefsScoreFinalKey.Length - 1; i >= 0; i--)
        {
            int score = PlayerPrefs.GetInt(playerPrefsScoreFinalKey[i], 0);

            if (score >= 100)
            {
                // Jika sudah lulus level ini, maka lanjut ke level berikutnya
                int nextIndex = i + 1;

                if (nextIndex < sceneNameList.Length)
                {
                    PlayerPrefs.SetFloat("PlayerPosX", -5.5f);
                    PlayerPrefs.SetFloat("PlayerPosY", -0.6f);
                    PlayerPrefs.SetFloat("PlayerPosZ", 0f);
                    PlayerPrefs.SetString("LastScenePlayed", sceneNameList[nextIndex]);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(sceneNameList[nextIndex]);
                    return;
                }
                else
                {
                    // Semua level sudah selesai, bisa load ulang level terakhir jika mau
                    SceneManager.LoadScene(sceneNameList[i]); // ulang level terakhir
                    return;
                }
            }
        }

        // Jika semua skor 0 atau belum main → mulai dari awal
        SceneManager.LoadScene("Level 1");
    }


    public void TutupPanelSelamat()
    {
        panelSelamat.SetActive(false);
        audioManager.PlaySFX(audioManager.button);
    }

    public void PanelReset()
    {
        if(panelResetConfirmasi != null)
        panelResetConfirmasi.SetActive(!panelResetConfirmasi.activeSelf);
        audioManager.PlaySFX(audioManager.button);
    }

    public void PanelKeluar()
    {
        if(panelKeluar != null)
        panelKeluar.SetActive(!panelKeluar.activeSelf);
        audioManager.PlaySFX(audioManager.button);
    }


    public void PanelKonfirmasiReset()
    {
        ResetDataPermainan();

        if(panelResetConfirmasi != null)
        panelResetConfirmasi.SetActive(false);

        if(panelResetSukses != null)
        panelResetSukses.SetActive(true);
        audioManager.PlaySFX(audioManager.button);

    }

    public void ResetDataPermainan()
    {
        // Hapus semua PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        // Reset warna semua imageArchiveProfile ke semula
        foreach (var data in levelUnlockList)
        {
            if (data.imageArchiveProfile != null)
            {
                for (int i = 0; i < data.imageArchiveProfile.Length; i++)
                {
                    if (data.imageArchiveProfile[i] != null)
                    {
                        Image img = data.imageArchiveProfile[i];
                        img.color = new Color(1f, 1f, 1f, 0f); // alpha 0 (transparan)
                    }
                }
            }

            // Nonaktifkan angka archive
            if (data.imageArchiveAngka != null)
            {
                foreach (var angka in data.imageArchiveAngka)
                {
                    if (angka != null)
                        angka.SetActive(false);
                }
            }
        }

        // Nonaktifkan panel-panel
        if (panelArchive != null) panelArchive.SetActive(false);
        if (panelSelamat != null) panelSelamat.SetActive(false);

        // (Opsional) Tampilkan log
        Debug.Log("Semua data permainan telah direset.");
    }

    public void KeluarGame()
    {
        Application.Quit();
    }

    // public void ResumePermainan()
    // {
    //     if (PemainLanjutan())
    //     {
    //         string sceneToLoad = PlayerPrefs.GetString("LastScenePlayed");
    //         SceneManager.LoadScene(sceneToLoad);
    //         audioManager.PlaySFX(audioManager.button);
    //     }
    //     else
    //     {
    //         SceneManager.LoadScene("Level1"); // default scene awal
    //         audioManager.PlaySFX(audioManager.button);
    //     }
    // }

    // private bool PemainLanjutan()
    // {
    //     return PlayerPrefs.HasKey("LastScenePlayed") &&
    //         PlayerPrefs.HasKey("PlayerPosX") &&
    //         PlayerPrefs.HasKey("PlayerPosY") &&
    //         PlayerPrefs.HasKey("PlayerPosZ");
    // }


}
