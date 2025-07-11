using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class PuzzleManager2 : MonoBehaviour
{
    public GameObject panelPuzzle;
    public RectTransform puzzlePiecesContainer;
    public PuzzleQuestion[] puzzleQuestions;
    public TextMeshProUGUI questionTextUI;
    public GameObject[] puzzlePieceObjects;
    public DropZone2[] dropZones;

    private int currentQuestionIndex = 0;

    [Header("Feedback UI")]
    public GameObject feedbackMenang;
    public GameObject feedbackKalah;
    public float feedbackDuration = 2f; // durasi tampil (detik)
    public TimeManager timeManager;

    [Header("Feedback RinaController")]
    public static PuzzleManager2 instance;
    public event Action<bool> OnPuzzleFinished;

    void Awake()
    {
        instance = this;
    }
    

    void Start()
    {
        ShufflePuzzlePieces();
        ShowCurrentQuestion();
    }

    void Update()
    {
        if (currentQuestionIndex >= puzzleQuestions.Length) return;

        if (IsPuzzleCompleted())
        {
            bool isCorrect = IsPuzzleCorrect();

            Debug.Log(isCorrect ? "Benar" : "Salah");
            if (isCorrect)
            {
                panelPuzzle.SetActive(false);
                timeManager.StopTimer();
                StartCoroutine(ShowFeedback(feedbackMenang));
            }
            else
            {
                panelPuzzle.SetActive(false);
                timeManager.StopTimer();
                StartCoroutine(ShowFeedback(feedbackKalah));
            }

            // Lanjut ke soal berikutnya jika ada
            currentQuestionIndex++;
            if (currentQuestionIndex < puzzleQuestions.Length)
            {
                ShowCurrentQuestion();
                ShufflePuzzlePieces();
            }
        }
    }

    bool IsPuzzleCompleted()
    {
        PuzzleQuestion currentQuestion = puzzleQuestions[currentQuestionIndex];

        foreach (var dropZone in currentQuestion.dropZones)
        {
            if (dropZone.transform.childCount == 0)
                return false;
        }
        return true;
    }

    bool IsPuzzleCorrect()
    {
        PuzzleQuestion currentQuestion = puzzleQuestions[currentQuestionIndex];

        if (currentQuestion.dropZones.Length != currentQuestion.puzzlePieces.Length)
        {
            Debug.LogError("Jumlah DropZones dan PuzzlePieces tidak cocok.");
            return false;
        }

        for (int i = 0; i < currentQuestion.dropZones.Length; i++)
        {
            var dropZone = currentQuestion.dropZones[i];

            if (dropZone.transform.childCount > 0)
            {
                GameObject puzzlePieceObject = dropZone.transform.GetChild(0).gameObject;
                Image puzzlePieceImage = puzzlePieceObject.GetComponent<Image>();

                if (puzzlePieceImage == null || puzzlePieceImage.sprite != currentQuestion.puzzlePieces[i].sprite)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    IEnumerator ShowFeedback(GameObject feedbackObject)
    {
        feedbackObject.SetActive(true);
        yield return new WaitForSeconds(feedbackDuration);
        feedbackObject.SetActive(false);

        // Panggil event setelah feedback ditampilkan
        bool isWin = (feedbackObject == feedbackMenang);
        OnPuzzleFinished?.Invoke(isWin);
    }

    void ShowCurrentQuestion()
    {
        if (currentQuestionIndex < puzzleQuestions.Length)
        {
            PuzzleQuestion currentQuestion = puzzleQuestions[currentQuestionIndex];
            questionTextUI.text = currentQuestion.questionText;

            // Atur potongan puzzle
            for (int i = 0; i < puzzlePieceObjects.Length; i++)
            {
                Image puzzlePieceImage = puzzlePieceObjects[i].GetComponent<Image>();

                if (i < currentQuestion.puzzlePieces.Length)
                {
                    puzzlePieceImage.sprite = currentQuestion.puzzlePieces[i].sprite;
                    puzzlePieceObjects[i].SetActive(true);
                    puzzlePieceObjects[i].transform.SetParent(puzzlePiecesContainer);
                }
                else
                {
                    puzzlePieceObjects[i].SetActive(false);
                }
            }

            // Aktifkan DropZones yang dibutuhkan
            for (int i = 0; i < dropZones.Length; i++)
            {
                if (i < currentQuestion.dropZones.Length)
                {
                    dropZones[i].gameObject.SetActive(true);
                }
                else
                {
                    dropZones[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void ShufflePuzzlePieces()
    {
        for (int i = 0; i < puzzlePieceObjects.Length; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, puzzlePieceObjects.Length);

            Transform temp = puzzlePieceObjects[i].transform;

            puzzlePieceObjects[i].transform.SetSiblingIndex(puzzlePieceObjects[randomIndex].transform.GetSiblingIndex());
            puzzlePieceObjects[randomIndex].transform.SetSiblingIndex(temp.GetSiblingIndex());
        }
    }

    [System.Serializable]
    public class PuzzleQuestion
    {
        public string questionText;
        public PuzzlePieceData[] puzzlePieces;
        public DropZone2[] dropZones;
    }
}

    [System.Serializable]
    public class PuzzlePieceData
    {
        public Sprite sprite;
        public int pieceID;
    }
