using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DropZoneL5M1 : MonoBehaviour
{
    [Header("Referensi DropZones & PuzzlePieces")]
    public DropZoneL5M1[] allDropZonesToCheck;
    public GameObject[] allPuzzlePieces;

    [Header("UI")]
    public TMP_InputField inputField;
    public GameObject warningUI;

    [Header("NPC Controller")]
    public NpcControllerL5M1 npcController; // ✅ Referensi ke NPC

    private static bool hasChecked = false;
    private GameObject _placedPiece = null;

    void Start()
    {
        if (warningUI != null) warningUI.SetActive(false);
    }

    public void PlacePuzzlePiece(GameObject piece)
    {
        if (!IsOccupied())
        {
            _placedPiece = piece;
        }
    }

    public bool IsOccupied()
    {
        return _placedPiece != null;
    }

    public void ClearSlot()
    {
        _placedPiece = null;
        hasChecked = false;
    }

    private void OnDisable()
    {
        hasChecked = false;
    }

    public void CheckAnswer()
    {
        if (hasChecked) return;

        int dropzoneTerisi = 0;
        foreach (var dz in allDropZonesToCheck)
        {
            if (dz.IsOccupied())
                dropzoneTerisi++;
        }

        if (dropzoneTerisi == 0)
        {
            if (warningUI != null)
                StartCoroutine(TampilkanWarning());
            return;
        }

        int totalAwal = allPuzzlePieces.Length;
        int expectedAnswer = totalAwal - dropzoneTerisi;

        int userInput;
        if (!int.TryParse(inputField.text, out userInput))
        {
            npcController?.OnPuzzleCheckResult(false); // ✅ Panggil NPC jika salah input
            return;
        }

        bool isCorrect = (userInput == expectedAnswer);
        npcController?.OnPuzzleCheckResult(isCorrect); // ✅ Panggil NPC untuk handle feedback
        hasChecked = true;
    }

    IEnumerator TampilkanWarning()
    {
        warningUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningUI.SetActive(false);
    }
}
