using UnityEngine;
using TMPro;

public class GetUserName : MonoBehaviour
{
    [Tooltip("Semua teks yang ingin menampilkan nama user.")]
    public TMP_Text[] teksNamaUserArray;

    private const string playerPrefsKey = "NamaUser";

    void Start()
    {
        UpdateUserName();
    }

    void Update()
    {
        // Opsional: periksa apakah nama user telah berubah dan update jika perlu
        string currentName = PlayerPrefs.GetString(playerPrefsKey, "Guest");

        foreach (var teks in teksNamaUserArray)
        {
            if (teks != null && teks.text != currentName)
            {
                teks.text = currentName;
            }
        }
    }

    public void UpdateUserName()
    {
        string namaUser = PlayerPrefs.GetString(playerPrefsKey, "Guest");

        foreach (var teks in teksNamaUserArray)
        {
            if (teks != null)
                teks.text = namaUser;
        }
    }
}