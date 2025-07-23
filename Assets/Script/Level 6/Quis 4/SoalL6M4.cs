using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SoalL6M4 : MonoBehaviour
{
    [Header("Input & Button")]
    public TMP_InputField[] inputJawaban;   // ✅ Tiga input jawaban (bisa ditambah via Inspector)
    public Button buttonJawaban;            // ✅ Button untuk cek jawaban

    [Header("Jawaban Benar")]
    public int[] expectedJawaban;           // ✅ Jawaban yang benar sesuai urutan input

    private static bool hasChecked = false;

    void Start()
    {
        if (buttonJawaban != null)
        {
            buttonJawaban.onClick.AddListener(CekJawaban);
        }

        // ✅ Tambahkan listener agar button hanya aktif jika semua input terisi
        foreach (TMP_InputField input in inputJawaban)
        {
            input.onValueChanged.AddListener(delegate { CekIsiInput(); });
        }

        CekIsiInput(); // Pastikan di awal juga dicek
    }

    void CekIsiInput()
    {
        bool semuaTerisi = true;

        foreach (TMP_InputField input in inputJawaban)
        {
            if (string.IsNullOrWhiteSpace(input.text))
            {
                semuaTerisi = false;
                break;
            }
        }

        if (buttonJawaban != null)
        {
            buttonJawaban.interactable = semuaTerisi;
        }
    }

    public void CekJawaban()
    {
        if (hasChecked) return;
        hasChecked = true;

        // ✅ Cek jawaban satu per satu
        for (int i = 0; i < inputJawaban.Length; i++)
        {
            int jawabanPlayer;

            // ✅ Jika bukan angka → otomatis salah
            if (!int.TryParse(inputJawaban[i].text, out jawabanPlayer))
            {
                Debug.LogWarning($"❌ Jawaban ke-{i + 1} bukan angka!");
                KirimFeedback(false);
                return;
            }

            // ✅ Jika salah satu jawaban tidak sesuai → langsung feedback salah
            if (jawabanPlayer != expectedJawaban[i])
            {
                Debug.LogWarning($"❌ Jawaban ke-{i + 1} salah! Expected {expectedJawaban[i]}");
                KirimFeedback(false);
                return;
            }
        }

        // ✅ Semua benar
        KirimFeedback(true);
    }

    private void KirimFeedback(bool isCorrect)
    {
        Debug.Log(isCorrect ? "✅ Semua Jawaban Benar" : "❌ Ada Jawaban yang Salah");

        NpcControllerL6M4 controller = FindObjectOfType<NpcControllerL6M4>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(isCorrect);
        }
    }

    public void ResetSoal()
    {
        hasChecked = false;

        foreach (TMP_InputField input in inputJawaban)
        {
            input.text = "";
        }

        CekIsiInput(); // Reset status button juga
    }

    private void OnDisable()
    {
        hasChecked = false;
    }
}

