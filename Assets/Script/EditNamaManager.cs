using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditNamaManager : MonoBehaviour
{
    [Header("Text yang Menampilkan Nama (bisa banyak)")]
    public TMP_Text[] namaUserTexts;

    [Header("Input dan Panel Edit")]
    public TMP_InputField inputFieldEditNama;
    public GameObject panelEditNama;

    private const string prefsKey = "NamaUser";

    void Start()
    {
        LoadAndDisplayNama();
        if (panelEditNama != null)
            panelEditNama.SetActive(false);
    }

    public void LoadAndDisplayNama()
    {
        string savedName = PlayerPrefs.GetString(prefsKey, "Guest");

        foreach (var teks in namaUserTexts)
        {
            if (teks != null)
                teks.text = savedName;
        }

        if (inputFieldEditNama != null)
            inputFieldEditNama.text = savedName;
    }

    public void OpenEditNamaPanel()
    {
        if (panelEditNama != null)
            panelEditNama.SetActive(true);

        LoadAndDisplayNama();
    }

    public void SimpanNamaBaru()
    {
        string newName = inputFieldEditNama.text.Trim();

        if (!string.IsNullOrEmpty(newName))
        {
            PlayerPrefs.SetString(prefsKey, newName);
            PlayerPrefs.Save();

            LoadAndDisplayNama();

            if (panelEditNama != null)
                panelEditNama.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Nama tidak boleh kosong.");
        }
    }

    public void BatalEditNama()
    {
        if (panelEditNama != null)
            panelEditNama.SetActive(false);
    }
}
