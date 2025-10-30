using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcControllerL3M2 : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

    public GameObject chatBoxUI;
    public GameObject UjangChatBox;
    public GameObject UjangChatBoxNolak;
    public GameObject controllerPanel;
    public GameObject Stop;
    private bool isChatShown = false;
    private bool isPlayerInRange = false;
    private bool isMissionCompleted = false;
    private bool isTimeOut = false;

    [Header("Misi Puzzle")]
    public GameObject panelSoal;
    public TimeManager timeManager;

    [Header("Referensi Objek Misi")]
    public GameObject AnnisaChatBoxPanelComplet;
    public GameObject chestBox;

    [Header("Feedback UI")]
    public GameObject feedbackBenar;
    public GameObject feedbackSalah;
    public float feedbackDuration = 2f;
    public GameObject bridge;

    [Header("Pengaturan Skor")]
    public string namaPlayerPrefsScore = "L3M2"; // ✅ Bisa diatur dari Inspector

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        AnnisaChatBoxPanelComplet?.SetActive(false);
        Stop?.SetActive(true);
        chestBox?.SetActive(false);
        bridge?.SetActive(false);

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
        AnnisaChatBoxPanelComplet?.SetActive(false);
    }

    public void MulaiMisiPuzzle()
    {
        UjangChatBox.SetActive(true);
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(false);
    }

    public void DalogUjang()
    {
        UjangChatBox.SetActive(false);
        panelSoal?.SetActive(true);
        isTimeOut = false;
        isMissionCompleted = false;
        timeManager?.StartTimer();
    }

    public void TutupDialogUjang()
    {
        UjangChatBoxNolak.SetActive(false);
        controllerPanel?.SetActive(true);
    }

    public void TolakMisiPuzzle()
    {
        chatBoxUI?.SetActive(false);
        UjangChatBoxNolak.SetActive(true);
    }

    
        public void OnPuzzleCheckResult(bool isCorrect)
    {
        timeManager?.StopTimer();
        panelSoal?.SetActive(false);
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

        AnnisaChatBoxPanelComplet?.SetActive(true);
        controllerPanel?.SetActive(true);
        bridge?.SetActive(true);
    }

    void ShowChat()
    {
        if (isTimeOut) return;

        isChatShown = true;

        if (isMissionCompleted && AnnisaChatBoxPanelComplet != null)
        {
            chatBoxUI?.SetActive(false);
            controllerPanel?.SetActive(true);
            AnnisaChatBoxPanelComplet.SetActive(true);
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
        AnnisaChatBoxPanelComplet?.SetActive(false);
    }

    void HandleTimeOut()
    {
        panelSoal?.SetActive(false);
        chatBoxUI?.SetActive(false);
        isChatShown = false;
        isMissionCompleted = false;
        isTimeOut = true;

        Debug.Log("Waktu habis, misi di-reset.");
    }

    public void ResetMisi()
    {
        panelSoal?.SetActive(false);
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        isChatShown = false;
        isMissionCompleted = false;
        isTimeOut = false;
        AnnisaChatBoxPanelComplet?.SetActive(false);
        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);
        timeManager?.StopTimer();
        Debug.Log("Misi di-reset."); 
    }
}

