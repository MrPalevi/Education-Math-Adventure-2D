using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcControllerL5M3 : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

    public GameObject chatBoxUI;
    public GameObject controllerPanel;
    public GameObject Stop;
    private bool isChatShown = false;
    private bool isPlayerInRange = false;
    private bool isMissionCompleted = false;
    private bool isTimeOut = false;

    [Header("Misi Puzzle")]
    public GameObject panelPuzzle;
    public TimeManager timeManager;

    [Header("Referensi Objek Misi")]
    public GameObject DadangChatCoxPanelComplet;
    public GameObject chestBox;

    [Header("Feedback UI")]
    public GameObject feedbackBenar;
    public GameObject feedbackSalah;
    public float feedbackDuration = 2f;

    [Header("Pengaturan Skor")]
    public string namaPlayerPrefsScore = "L1M3"; // ✅ Bisa diatur dari Inspector

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        DadangChatCoxPanelComplet?.SetActive(false);
        Stop?.SetActive(true);
        chestBox?.SetActive(false);

        if (timeManager != null)
            timeManager.OnTimeOut += HandleTimeOut;
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);
        if (distance <= detectionRange)
        {
            spriteRenderer.flipX = player.position.x < transform.position.x;
        }

        if (isPlayerInRange && !isChatShown && !isMissionCompleted)
        {
            ShowChat();
        }

        if (Input.GetKeyDown(KeyCode.R)) ResetMisi();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInRange = true;

        if (!isChatShown && !isTimeOut)
        {
            ShowChat();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInRange = false;
        isChatShown = false;
        chatBoxUI?.SetActive(false);
        DadangChatCoxPanelComplet?.SetActive(false);
    }

    public void MulaiMisiPuzzle()
    {
        chatBoxUI?.SetActive(false);
        panelPuzzle?.SetActive(true);
        controllerPanel?.SetActive(false);
        isTimeOut = false;
        isMissionCompleted = false;
        timeManager?.StartTimer();
    }

    public void TolakMisiPuzzle()
    {
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
    }

    public void OnPuzzleCheckResult(bool isCorrect)
    {
        timeManager?.StopTimer();
        panelPuzzle?.SetActive(false);
        Stop?.SetActive(false);

        ShowFeedback(isCorrect); // tampilkan feedback manual
    }

    private void ShowFeedback(bool isCorrect)
    {
        // Matikan semua feedback dulu
        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);

        if (isCorrect)
        {
            feedbackBenar?.SetActive(true);
            Debug.Log("Feedback Benar ditampilkan");
        }
        else
        {
            feedbackSalah?.SetActive(true);
            Debug.Log("Feedback Salah ditampilkan");
            chestBox?.SetActive(false); // pastikan tidak muncul saat salah
        }

        // Simpan skor jika belum ada
        int score = isCorrect ? 100 : 0;
        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, score);
            PlayerPrefs.Save();
            Debug.Log($"Skor {score} disimpan ke PlayerPrefsScore dengan key: {namaPlayerPrefsScore}");
        }
        else
        {
            Debug.Log($"Skor sudah pernah disimpan di {namaPlayerPrefsScore}, tidak ditimpa.");
        }

        isMissionCompleted = true;
    }

    public void CloseFeedback()
    {
        bool isCorrect = feedbackBenar.activeSelf; // cek apakah feedback benar yang aktif

        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);

        if (isCorrect)
        {
            chestBox?.SetActive(true);
            Debug.Log("ChestBox ditampilkan setelah feedbackBenar ditutup");
        }

        DadangChatCoxPanelComplet?.SetActive(true);
        controllerPanel?.SetActive(true);
    }

    void ShowChat()
    {
        if (isTimeOut) return;

        isChatShown = true;

        if (isMissionCompleted && DadangChatCoxPanelComplet != null)
        {
            chatBoxUI?.SetActive(false);
            controllerPanel?.SetActive(true);
            DadangChatCoxPanelComplet.SetActive(true);
            StartCoroutine(HideCompletePanelAfterDelay());
        }
        else
        {
            chatBoxUI?.SetActive(true);
            controllerPanel?.SetActive(false);
        }

        Debug.Log("Player mendekati Dede, menampilkan UI yang sesuai.");
    }

    IEnumerator HideCompletePanelAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        DadangChatCoxPanelComplet?.SetActive(false);
    }

    void HandleTimeOut()
    {
        panelPuzzle?.SetActive(false);
        chatBoxUI?.SetActive(false);
        isChatShown = false;
        isMissionCompleted = false;
        isTimeOut = true;

        Debug.Log("Waktu habis, misi di-reset.");
    }

    public void ResetMisi()
    {
        panelPuzzle?.SetActive(false);
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        isChatShown = false;
        isMissionCompleted = false;
        isTimeOut = false;
        DadangChatCoxPanelComplet?.SetActive(false);
        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);
        timeManager?.StopTimer();

        foreach (DropZone dz in FindObjectsOfType<DropZone>())
        {
            dz.Clear();
        }

        Debug.Log("Misi di-reset.");
    }
}



