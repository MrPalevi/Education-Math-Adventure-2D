using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZoneL3M1 : MonoBehaviour
{
    public int expectedID; // ID yang diharapkan pada dropzone ini
    private GameObject placedPuzzlePiece;

    [Header("List semua dropzone yang harus diperiksa")]
    public DropZoneL3M1[] allDropZonesToCheck;

    private static bool hasChecked = false; // agar hanya dicek sekali

    public void PlacePuzzlePiece(GameObject puzzlePiece)
    {
        placedPuzzlePiece = puzzlePiece;
        CheckAllDropZonesIfReady(); // hanya akan lanjut jika semua terisi
    }

    public bool IsOccupied()
    {
        return placedPuzzlePiece != null;
    }

    public int GetPlacedID()
    {
        if (placedPuzzlePiece != null)
        {
            var drag = placedPuzzlePiece.GetComponent<DragAndDropL3M1>();
            if (drag != null)
                return drag.puzzleID;
        }
        return -1;
    }

    private void CheckAllDropZonesIfReady()
    {
        if (hasChecked) return;

        foreach (DropZoneL3M1 dz in allDropZonesToCheck)
        {
            if (!dz.IsOccupied())
                return; // masih ada slot kosong, tunda
        }

        hasChecked = true;
        bool allCorrect = true;

        foreach (DropZoneL3M1 dz in allDropZonesToCheck)
        {
            if (dz.GetPlacedID() != dz.expectedID)
            {
                allCorrect = false;
                break;
            }
        }

        Debug.Log(allCorrect ? "✅ Semua benar!" : "❌ Ada yang salah!");

        // Panggil controller feedback
        NpcControllerL3M1 controller = FindObjectOfType<NpcControllerL3M1>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(allCorrect);
        }
    }

    public void ClearSlot()
    {
        placedPuzzlePiece = null;
        hasChecked = false;
    }

    private void OnDisable()
    {
        hasChecked = false; // reset saat disable atau scene reload
    }
}
