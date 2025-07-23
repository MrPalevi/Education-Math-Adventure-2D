using System.Collections;
using UnityEngine;
using TMPro;

public class DropZoneL5M4 : MonoBehaviour
{
    private GameObject placedPuzzlePiece;

    [Header("List semua dropzone yang harus diperiksa")]
    public DropZoneL5M4[] allDropZonesToCheck;

    [Header("UI")]
    public TMP_Text hasilText; // ✅ Text untuk menampilkan hasil pengurangan
    public float delayFeedback = 1f; // ✅ Jeda sebelum feedback

    private static bool hasChecked = false;
    public NpcControllerL5M4 controller;

    public void PlacePuzzlePiece(GameObject puzzlePiece)
    {
        placedPuzzlePiece = puzzlePiece;
        // ✅ Tidak langsung cek, hanya menaruh puzzle
    }

    public bool IsOccupied()
    {
        return placedPuzzlePiece != null;
    }

    public int GetPlacedID()
    {
        if (placedPuzzlePiece != null)
        {
            var drag = placedPuzzlePiece.GetComponent<DragAndDropL5M4>();
            if (drag != null)
                return drag.puzzleID;
        }
        return 0;
    }

    /// <summary>
    /// ✅ Dipanggil oleh Button Cek
    /// </summary>
    public void CheckAnswerByButton()
    {
        if (hasChecked) return;

        // ✅ Pastikan semua dropzone sudah terisi
        foreach (DropZoneL5M4 dz in allDropZonesToCheck)
        {
            if (!dz.IsOccupied())
            {
                Debug.LogWarning("❌ Masih ada slot kosong!");
                return;
            }
        }

        hasChecked = true;
        StartCoroutine(CalculateAndShowResult());
    }

    private IEnumerator CalculateAndShowResult()
    {
        // ✅ Hitung pengurangan berdasarkan urutan dropzone di array
        int result = 0;
        if (allDropZonesToCheck.Length > 0)
        {
            result = allDropZonesToCheck[0].GetPlacedID(); // Ambil ID pertama sebagai nilai awal

            for (int i = 1; i < allDropZonesToCheck.Length; i++)
            {
                result -= allDropZonesToCheck[i].GetPlacedID();
            }
        }

        // ✅ Tampilkan hasil ke TextMeshPro (sebelum feedback)
        if (hasilText != null)
        {
            hasilText.text = $"{result}";
        }

        // ✅ Tunggu 1 detik sebelum feedback benar/salah
        yield return new WaitForSeconds(delayFeedback);

        // ✅ Validasi baru: hasil harus > 0 dan < 10
        bool isCorrect = (result > 0 && result < 10);

        Debug.Log(isCorrect
            ? $"✅ BENAR (hasil {result} > 0 dan < 10)"
            : $"❌ SALAH (hasil {result} tidak sesuai syarat)");

        // ✅ Panggil controller feedback
        NpcControllerL5M4 controller = FindObjectOfType<NpcControllerL5M4>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(isCorrect);
        }
    }

    public void ClearSlot()
    {
        placedPuzzlePiece = null;
        hasChecked = false;
        if (hasilText != null) hasilText.text = "";
    }

    private void OnDisable()
    {
        hasChecked = false;
    }
}
