using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaL4M3 : MonoBehaviour
{
    [Header("Konfigurasi")]
    public Button[] tombolJawaban;           // 8 tombol pilihan
    public int[] tombolID;                   // ID untuk tiap tombol
    public int expectedResult;            // Hasil yang diharapkan, contoh: "5 + 1 + 3"

    [Header("UI dan Feedback")]
    public TMP_Text hasilText;               // Tampilkan ID yang dipilih
    public Button cekButton;                 // Tombol untuk cek jawaban

    [Header("NPC Controller")]
    public NpcControllerL4M3 npcController;

    private List<int> jawabanDipilih = new List<int>();
    private bool sudahCek = false;

    void Start()
    {
        // Pastikan array tombol dan ID cocok
        if (tombolID.Length != tombolJawaban.Length)
        {
            Debug.LogError("Jumlah tombol dan ID tidak cocok!");
            return;
        }

        // Daftarkan listener ke semua tombol
        for (int i = 0; i < tombolJawaban.Length; i++)
        {
            int index = i; // Hindari bug closure
            tombolJawaban[i].onClick.AddListener(() => PilihTombol(index));
        }

        cekButton.onClick.AddListener(CekJawaban);
        hasilText.text = "0";
    }

    void PilihTombol(int index)
    {
        if (jawabanDipilih.Count >= 3 || sudahCek) return;
        if (jawabanDipilih.Contains(index)) return; // Tidak boleh klik tombol yang sama dua kali

        jawabanDipilih.Add(index);

       
        tombolJawaban[index].image.color = new Color(0f, 0f, 0f, 0.20f); // #000000BF

        
        UpdateHasilText();
    }

    void UpdateHasilText()
    {
        string hasilGabungan = "";
        for (int i = 0; i < jawabanDipilih.Count; i++)
        {
            int idx = jawabanDipilih[i];
            hasilGabungan += tombolID[idx].ToString();
            if (i < jawabanDipilih.Count - 1)
            {
                hasilGabungan += " + ";
            }
        }
        hasilText.text = hasilGabungan;
    }

    void CekJawaban()
    {
        if (sudahCek || jawabanDipilih.Count < 3) return;

        // Hitung jumlah total ID tombol yang dipilih
        int total = 0;
        foreach (int idx in jawabanDipilih)
        {
            total += tombolID[idx];
        }

        bool benar = (total == expectedResult);

        npcController.OnPuzzleCheckResult(benar);

        Debug.Log(benar ? "✅ Jawaban Benar!" : "❌ Jawaban Salah!");

        sudahCek = true;
    }


    public void ResetTrivia()
    {
        // Reset semua data
        jawabanDipilih.Clear();
        hasilText.text = "";
        sudahCek = false;

        // Reset warna tombol
        foreach (var tombol in tombolJawaban)
        {
            tombol.image.color = Color.white;
        }
    }
}
