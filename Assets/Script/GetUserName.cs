using UnityEngine;
using TMPro;

public class GetUserName : MonoBehaviour
{
public TMP_Text[] teksNamaUserArray;
public string[] customSuffixes; // Misal: ["!", "さん", "님"]

void Start()
{
    string namaUser = PlayerPrefs.GetString("NamaUser", "Guest");

    for (int i = 0; i < teksNamaUserArray.Length; i++)
    {
        if (teksNamaUserArray[i] != null)
        teksNamaUserArray[i].text = customSuffixes[i] + "" + namaUser ;
    }
}
}
