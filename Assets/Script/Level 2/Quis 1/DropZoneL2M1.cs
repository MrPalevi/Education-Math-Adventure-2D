using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneL2M1 : MonoBehaviour
{
    private GameObject placedPuzzlePiece;

    [Header("DropZones yang akan dijumlahkan")]
    public DropZoneL2M1[] linkedDropZones; // Diatur dari Inspector

    [Header("Nilai yang diharapkan")]
    public int expectedResult = 8;

    private bool hasChecked = false;

    void Update()
    {
        if (!hasChecked && AllDropZonesOccupied())
        {
            hasChecked = true;
            CheckSum();
        }
    }

    public void PlacePuzzlePiece(GameObject puzzlePiece)
    {
        placedPuzzlePiece = puzzlePiece;
    }

    public bool IsOccupied()
    {
        return placedPuzzlePiece != null;
    }

    public int GetPlacedPuzzleID()
    {
        if (placedPuzzlePiece != null)
        {
            DragAndDropL2M1 script = placedPuzzlePiece.GetComponent<DragAndDropL2M1>();
            if (script != null)
                return script.puzzleID;
        }
        return 0; // default jika kosong
    }

    bool AllDropZonesOccupied()
    {
        if (!IsOccupied()) return false;

        // Jika tidak ada dropzone lain yang digunakan, langsung true
        if (linkedDropZones == null || linkedDropZones.Length == 0)
            return true;

        foreach (DropZoneL2M1 zone in linkedDropZones)
        {
            if (zone == null || !zone.IsOccupied())
                return false;
        }

        return true;
    }

    void CheckSum()
    {
        int total = GetPlacedPuzzleID();
        Debug.Log($"ID dari DropZone ini: {total}");

        foreach (DropZoneL2M1 zone in linkedDropZones)
        {
            int id = zone.GetPlacedPuzzleID();
            total += id;
            Debug.Log($"ID dari linked DropZone: {id}");
        }

        Debug.Log($"Total Puzzle ID = {total}, Expected = {expectedResult}");

        bool isCorrect = (total == expectedResult);

        if (isCorrect)
            Debug.Log("✅ Jawaban Benar!");
        else
            Debug.Log("❌ Jawaban Salah!");

        AnnisaControllerL2M1 controller = FindObjectOfType<AnnisaControllerL2M1>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(isCorrect);
            Debug.Log("Controller ditemukan dan dipanggil!");
        }
        else
        {
            Debug.LogWarning("⚠️ Controller (AnnisaControllerL2M1) tidak ditemukan di scene!");
        }
    }

    public void Clear()
    {
        placedPuzzlePiece = null;
        hasChecked = false;
    }
}