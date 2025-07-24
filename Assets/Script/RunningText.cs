using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class RunningText : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Objek TextMeshPro yang ingin dipakai
    public List<Button> continueButtons; // Daftar tombol yang akan dimunculkan setelah teks selesai
    public float revealSpeed = 0.05f;    // Waktu delay antar karakter (semakin kecil semakin cepat)

    private string fullText;             // Teks lengkap yang akan muncul

    void Start()
    {
        // Ambil teks penuh dari TextMeshPro
        fullText = textMeshPro.text;
        
        // Kosongkan teks di awal
        textMeshPro.text = "";

        // Sembunyikan semua tombol di awal
        foreach (var button in continueButtons)
        {
            button.gameObject.SetActive(false);
        }

        // Mulai coroutine untuk menampilkan teks satu per satu
        StartCoroutine(RevealCharacters());
    }

    IEnumerator RevealCharacters()
    {
        // Iterasi melalui setiap huruf
        for (int i = 0; i <= fullText.Length; i++)
        {
            // Set jumlah karakter yang ingin ditampilkan sesuai dengan indeks loop
            textMeshPro.text = fullText.Substring(0, i);
            
            // Tunggu beberapa saat sebelum menampilkan huruf berikutnya
            yield return new WaitForSeconds(revealSpeed);
        }

        // Setelah selesai menampilkan teks, munculkan semua tombol
        foreach (var button in continueButtons)
        {
            button.gameObject.SetActive(true);
        }
    }
}
