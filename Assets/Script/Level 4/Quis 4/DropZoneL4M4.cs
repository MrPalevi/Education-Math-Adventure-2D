using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DropZoneL4M4 : MonoBehaviour
{
    public int expectedID; // ID yang diharapkan pada dropzone ini
    private GameObject placedPuzzlePiece;

    [Header("List semua dropzone yang harus diperiksa")]
    public DropZoneL4M4[] allDropZonesToCheck;

    [Header("UI Output")]
    public TMP_Text hasilText; // Output untuk gabungan ID
    public Button cekButton;   // Tombol cek yang ditekan player

    private static bool hasChecked = false;

    void Start()
    {
        if (cekButton != null)
        {
            cekButton.onClick.AddListener(CheckAllDropZonesManual);
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

    public int GetPlacedID()
    {
        if (placedPuzzlePiece != null)
        {
            var drag = placedPuzzlePiece.GetComponent<DragAndDropL4M4>();
            if (drag != null)
                return drag.puzzleID;
        }
        return -1;
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

    // Fungsi cek manual oleh user
    public void CheckAllDropZonesManual()
    {
        if (hasChecked) return;

        foreach (DropZoneL4M4 dz in allDropZonesToCheck)
        {
            if (!dz.IsOccupied())
            {
                Debug.Log("❗ Masih ada slot kosong!");
                return;
            }
        }

        // Gabungkan ID dan tampilkan di UI
        List<int> idList = new List<int>();
        foreach (DropZoneL4M4 dz in allDropZonesToCheck)
        {
            idList.Add(dz.GetPlacedID());
        }

        string gabungan = string.Join(" + ", idList);
        if (hasilText != null)
        {
            hasilText.text = gabungan;
        }

        StartCoroutine(DelayCheckResult(idList));
    }

    // Coroutine untuk menunggu 0.5 detik lalu cek benar/salah
    IEnumerator DelayCheckResult(List<int> idList)
    {
        hasChecked = true;

        yield return new WaitForSeconds(0.5f);

        bool allCorrect = true;
        for (int i = 0; i < allDropZonesToCheck.Length; i++)
        {
            int expected = allDropZonesToCheck[i].expectedID;
            int actual = idList[i];

            if (actual != expected)
            {
                allCorrect = false;
                break;
            }
        }

        Debug.Log(allCorrect ? "✅ Semua benar!" : "❌ Ada yang salah!");

        NpcControllerL4M4 controller = FindObjectOfType<NpcControllerL4M4>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(allCorrect);
        }
    }
}
