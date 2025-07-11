using UnityEngine;

public class DropZone : MonoBehaviour
{
    public int dropZoneID;
    private GameObject placedPuzzlePiece;

    public DropZone linkedDropZone;
    public int expectedResult = 8;

    private bool hasChecked = false;

    void Update()
    {
        if (!hasChecked && IsOccupied() && linkedDropZone != null && linkedDropZone.IsOccupied())
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
            DragAndDrop script = placedPuzzlePiece.GetComponent<DragAndDrop>();
            if (script != null)
                return script.puzzleID;
        }
        return -1;
    }

    void CheckSum()
    {
        int id1 = GetPlacedPuzzleID();
        int id2 = linkedDropZone.GetPlacedPuzzleID();
        int total = id1 + id2;

        Debug.Log($"Puzzle Check: {id1} + {id2} = {total}");

        bool isCorrect = (total == expectedResult);

        if (isCorrect)
            Debug.Log("✅ Jawaban Benar!");
        else
            Debug.Log("❌ Jawaban Salah!");

        // Panggil DedeController
        DedeController controller = FindObjectOfType<DedeController>();
        if (controller != null)
        {
            controller.OnPuzzleCheckResult(isCorrect);
        }
    }

    public void Clear()
    {
        placedPuzzlePiece = null;
        hasChecked = false;
    }
}
