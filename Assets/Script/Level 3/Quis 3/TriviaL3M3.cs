using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriviaL3M3 : MonoBehaviour
{
    [Header("Tombol Pilihan Jawaban")]
    public Button[] tombolJawaban; // Maks. 6 tombol
    public int indexJawabanBenar = 0; // 0 = A, 1 = B, dst

    [Header("Referensi NPC Controller")]
    public NpcControllerL3M3 npcController;

    private bool jawabanSudahDicek = false;

    void Start()
    {
        for (int i = 0; i < tombolJawaban.Length; i++)
        {
            int index = i; // Hindari closure bug
            tombolJawaban[i].onClick.AddListener(() => PilihJawaban(index));
        }
    }

    void PilihJawaban(int index)
    {
        if (jawabanSudahDicek) return;

        bool isCorrect = (index == indexJawabanBenar);

        npcController.OnPuzzleCheckResult(isCorrect);

        Debug.Log(isCorrect ? "✅ Jawaban Benar!" : "❌ Jawaban Salah!");

        jawabanSudahDicek = true;
    }

    public void ResetTrivia()
    {
        jawabanSudahDicek = false;
    }
}

