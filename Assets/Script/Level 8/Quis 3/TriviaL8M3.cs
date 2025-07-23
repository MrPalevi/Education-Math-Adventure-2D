using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaL8M3 : MonoBehaviour
{
    [Header("Konfigurasi")]
    public Button[] tombolJawaban;         // Jumlah harus kelipatan 2 (1 grup = 2 tombol)
    public int[] jawabanBenarPerGrup;      // Index tombol yang benar dalam tiap grup (global index)

    [Header("UI")]
    public Button cekButton;               // Tombol untuk cek jawaban

    [Header("NPC Controller")]
    public NpcControllerL8M3 npcController;

    private int[] pilihanPengguna;         // Menyimpan pilihan index tombol user per grup
    private bool sudahCek = false;
    private int jumlahGrup;

    void Start()
    {
        jumlahGrup = tombolJawaban.Length / 2;
        pilihanPengguna = new int[jumlahGrup];
        for (int i = 0; i < jumlahGrup; i++)
        {
            pilihanPengguna[i] = -1; // belum dipilih
        }

        if (jawabanBenarPerGrup.Length != jumlahGrup)
        {
            Debug.LogError("Jumlah jawabanBenarPerGrup harus sama dengan jumlah grup!");
            return;
        }

        for (int i = 0; i < tombolJawaban.Length; i++)
        {
            int index = i;
            tombolJawaban[i].onClick.AddListener(() => PilihTombol(index));
            tombolJawaban[i].interactable = true;
        }

        cekButton.onClick.AddListener(CekJawaban);
        cekButton.interactable = false;
    }

    void PilihTombol(int index)
    {
        if (sudahCek) return;

        int grupIndex = index / 2;

        // Reset warna dalam grup
        for (int i = grupIndex * 2; i < grupIndex * 2 + 2; i++)
        {
            tombolJawaban[i].image.color = Color.white;
        }

        // Tandai pilihan dan highlight tombol
        pilihanPengguna[grupIndex] = index;
        tombolJawaban[index].image.color = new Color(0f, 0f, 0f, 0.2f);

        UpdateCekButtonState();
    }

    void UpdateCekButtonState()
    {
        foreach (int pilihan in pilihanPengguna)
        {
            if (pilihan == -1)
            {
                cekButton.interactable = false;
                return;
            }
        }
        cekButton.interactable = true;
    }

    void CekJawaban()
    {
        if (sudahCek) return;

        bool semuaBenar = true;

        for (int i = 0; i < jumlahGrup; i++)
        {
            if (pilihanPengguna[i] != jawabanBenarPerGrup[i])
            {
                semuaBenar = false;
                break;
            }
        }

        npcController.OnPuzzleCheckResult(semuaBenar);
        Debug.Log(semuaBenar ? "✅ Semua jawaban benar!" : "❌ Ada jawaban salah!");

        sudahCek = true;
        cekButton.interactable = false;
    }

    public void ResetTrivia()
    {
        sudahCek = false;
        cekButton.interactable = false;

        for (int i = 0; i < jumlahGrup; i++)
        {
            pilihanPengguna[i] = -1;
        }

        foreach (Button btn in tombolJawaban)
        {
            btn.image.color = Color.white;
            btn.interactable = true;
        }
    }
}
