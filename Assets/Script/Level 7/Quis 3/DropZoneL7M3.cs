using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropZoneL7M3 : MonoBehaviour
{
    public int expectedID; // ID yang diharapkan pada dropzone ini
    private GameObject placedPuzzlePiece;

    [Header("List semua dropzone yang harus diperiksa")]
    public DropZoneL7M3[] allDropZonesToCheck;

    private static bool hasChecked = false; // agar hanya dicek sekali
    public NpcControllerL7M3 controller;

    public Button checkButton; // Assign via Inspector

    public void PlacePuzzlePiece(GameObject puzzlePiece)
    {
        placedPuzzlePiece = puzzlePiece;

        if (AllDropZonesOccupied())
        {
            if (checkButton != null)
                checkButton.interactable = true;
        }
        else
        {
            if (checkButton != null)
                checkButton.interactable = false;
        }
    }

    public bool IsOccupied()
    {
        return placedPuzzlePiece != null;
    }

    public int GetPlacedID()
    {
        if (placedPuzzlePiece != null)
        {
            var drag = placedPuzzlePiece.GetComponent<DragAndDropL7M3>();
            if (drag != null)
                return drag.puzzleID;
        }
        return -1;
    }

    private bool AllDropZonesOccupied()
    {
        foreach (DropZoneL7M3 dz in allDropZonesToCheck)
        {
            if (!dz.IsOccupied())
                return false;
        }
        return true;
    }

    public void ManualCheckAllDropZones()
    {
        if (hasChecked) return;

        if (!AllDropZonesOccupied())
        {
            Debug.Log("❗ Masih ada slot yang kosong.");
            return;
        }

        hasChecked = true;
        bool allCorrect = true;

        foreach (DropZoneL7M3 dz in allDropZonesToCheck)
        {
            if (dz.GetPlacedID() != dz.expectedID)
            {
                allCorrect = false;
                break;
            }
        }

        Debug.Log(allCorrect ? "✅ Semua benar!" : "❌ Ada yang salah!");

        NpcControllerL7M3 controller = FindObjectOfType<NpcControllerL7M3>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(allCorrect);
        }

        // Nonaktifkan tombol setelah pengecekan
        if (checkButton != null)
            checkButton.interactable = false;
    }

    public void ClearSlot()
    {
        placedPuzzlePiece = null;
        hasChecked = false;
        if (checkButton != null)
            checkButton.interactable = false;
    }

    private void OnDisable()
    {
        hasChecked = false; // reset saat disable atau scene reload
        if (checkButton != null)
            checkButton.interactable = false;
    }
}
