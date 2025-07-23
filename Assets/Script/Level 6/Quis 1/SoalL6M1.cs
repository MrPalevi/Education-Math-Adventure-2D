using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SoalL6M1 : MonoBehaviour
{
    [Header("Input & Button")]
    public TMP_InputField inputJawaban;
    public Button buttonJawaban;            

    [Header("Gambar Soal")]
    public GameObject[] gambarSoal;         

    [Header("Jawaban Benar")]
    public int expectedJawaban;             

    [Header("Pengaturan Feedback")]
    public float delayNonaktifGambar = 1f; 

    private static bool hasChecked = false;

    void Start()
    {
        if (buttonJawaban != null)
        {
            buttonJawaban.onClick.AddListener(CekJawaban);
        }
    }

    public void CekJawaban()
    {
        if (hasChecked) return;

        // ✅ Validasi input angka
        int jawabanPlayer;
        if (!int.TryParse(inputJawaban.text, out jawabanPlayer))
        {
            Debug.LogWarning("❌ Jawaban bukan angka!");
            KirimFeedback(false);
            return;
        }

        hasChecked = true;

        if (jawabanPlayer == expectedJawaban)
        {
            // ✅ Jika benar → nonaktifkan gambar dulu baru kirim feedback
            StartCoroutine(NonaktifkanGambarLaluFeedback());
        }
        else
        {
            // ✅ Jika salah langsung feedback salah
            KirimFeedback(false);
        }
    }

    private IEnumerator NonaktifkanGambarLaluFeedback()
    {
        // ✅ Nonaktifkan semua gambar soal
        foreach (GameObject g in gambarSoal)
        {
            if (g != null) g.SetActive(false);
        }

        // ✅ Tunggu sebelum tampilkan feedback
        yield return new WaitForSeconds(delayNonaktifGambar);

        KirimFeedback(true);
    }

    private void KirimFeedback(bool isCorrect)
    {
        Debug.Log(isCorrect ? "✅ Jawaban Benar" : "❌ Jawaban Salah");

        // ✅ Panggil controller feedback dari NPC
        NpcControllerL6M1 controller = FindObjectOfType<NpcControllerL6M1>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(isCorrect);
        }
    }

    public void ResetSoal()
    {
        hasChecked = false;
        inputJawaban.text = "";
        foreach (GameObject g in gambarSoal)
        {
            if (g != null) g.SetActive(true);
        }
    }

    private void OnDisable()
    {
        hasChecked = false;
    }
}
