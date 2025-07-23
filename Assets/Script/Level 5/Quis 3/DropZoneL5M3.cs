using System.Collections;
using UnityEngine;

public class DropZoneL5M3 : MonoBehaviour
{
    [Header("Expected ID (urutan harus sama antar DropZone)")]
    public int[] expectedIDs; // ✅ Array sesuai urutan pasangan

    private GameObject placedPuzzlePiece;

    [Header("List semua dropzone yang harus diperiksa")]
    public DropZoneL5M3[] allDropZonesToCheck;

    private static bool hasChecked = false;

    public void PlacePuzzlePiece(GameObject puzzlePiece)
    {
        placedPuzzlePiece = puzzlePiece;
        CheckAllDropZonesIfReady();
    }

    public bool IsOccupied()
    {
        return placedPuzzlePiece != null;
    }

    public int GetPlacedID()
    {
        if (placedPuzzlePiece != null)
        {
            var drag = placedPuzzlePiece.GetComponent<DragAndDropL5M3>();
            if (drag != null)
                return drag.puzzleID;
        }
        return -1;
    }

    private void CheckAllDropZonesIfReady()
    {
        if (hasChecked) return;

        // ✅ Pastikan semua DropZone terisi
        foreach (DropZoneL5M3 dz in allDropZonesToCheck)
        {
            if (!dz.IsOccupied())
                return;
        }

        hasChecked = true;
        bool foundCorrectPair = false;

        int totalPairs = allDropZonesToCheck[0].expectedIDs.Length;

        // ✅ Periksa setiap pasangan index
        for (int i = 0; i < totalPairs; i++)
        {
            bool currentPairCorrect = true;

            foreach (DropZoneL5M3 dz in allDropZonesToCheck)
            {
                if (dz.GetPlacedID() != dz.expectedIDs[i])
                {
                    currentPairCorrect = false;
                    break;
                }
            }

            if (currentPairCorrect)
            {
                foundCorrectPair = true;
                break; // ✅ Langsung berhenti jika ada pasangan yang benar
            }
        }

        Debug.Log(foundCorrectPair ? "✅ BENAR (ada pasangan yang cocok)" : "❌ SALAH (tidak ada pasangan yang cocok)");

        // ✅ Panggil controller feedback
        NpcControllerL5M3 controller = FindObjectOfType<NpcControllerL5M3>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(foundCorrectPair);
        }
    }

    public void ClearSlot()
    {
        placedPuzzlePiece = null;
        hasChecked = false;
    }

    private void OnDisable()
    {
        hasChecked = false;
    }
}
