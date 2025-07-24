using UnityEngine;
using TMPro;

public class GetUserName : MonoBehaviour
{
    public TMP_Text[] teksNamaUserArray;

    void Start()
    {
        string namaUser = PlayerPrefs.GetString("NamaUser", "Guest");

        foreach (var teks in teksNamaUserArray)
        {
            if (teks != null)
                teks.text = namaUser;
        }
    }
}
