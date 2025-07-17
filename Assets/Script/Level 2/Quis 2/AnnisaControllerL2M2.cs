using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnisaControllerL2M2 : MonoBehaviour
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
    public GameObject AnnisaChatCoxPanelComplet;
    public GameObject chestBox;

    [Header("Feedback UI")]
    public GameObject feedbackBenar;
    public GameObject feedbackSalah;
    public float feedbackDuration = 2f;

    [Header("Pengaturan Skor")]
    public string namaPlayerPrefsScore = "L2M1"; // ✅ Bisa diatur dari Inspector

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        AnnisaChatCoxPanelComplet?.SetActive(false);
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
        AnnisaChatCoxPanelComplet?.SetActive(false);
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
        Stop.SetActive(false);
        StartCoroutine(ShowFeedbackThenComplete(isCorrect));
    }

IEnumerator ShowFeedbackThenComplete(bool isCorrect)
{
    int score = isCorrect ? 100 : 0;

    // Pastikan semua feedback dimatikan dulu
    feedbackBenar?.SetActive(false);
    feedbackSalah?.SetActive(false);

    yield return null; // Tunggu 1 frame agar reset efektif

    // Tampilkan feedback sesuai hasil
    if (isCorrect)
    {
        feedbackBenar?.SetActive(true);
        Debug.Log("Feedback Benar ditampilkan");
    }
    else
    {
        feedbackSalah?.SetActive(true);
        Debug.Log("Feedback Salah ditampilkan");
        chestBox?.SetActive(false); // Pastikan chest tidak tampil saat salah
    }

    // Simpan skor jika belum disimpan
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

    // Tunggu durasi feedback tampil
    yield return new WaitForSeconds(feedbackDuration);

    feedbackBenar?.SetActive(false);
    feedbackSalah?.SetActive(false);

    // ✅ Delay 0.5 detik lalu munculkan chestBox jika benar
    if (isCorrect)
    {
        yield return new WaitForSeconds(0.5f);
        chestBox?.SetActive(true);
        Debug.Log("ChestBox ditampilkan setelah delay");
    }

    isMissionCompleted = true;
    AnnisaChatCoxPanelComplet?.SetActive(true);
    controllerPanel?.SetActive(true);
}


    void ShowChat()
    {
        if (isTimeOut) return;

        isChatShown = true;

        if (isMissionCompleted && AnnisaChatCoxPanelComplet != null)
        {
            chatBoxUI?.SetActive(false);
            controllerPanel?.SetActive(true);
            AnnisaChatCoxPanelComplet.SetActive(true);
            StartCoroutine(HideCompletePanelAfterDelay());
        }
        else
        {
            chatBoxUI?.SetActive(true);
            controllerPanel?.SetActive(false);
        }

        Debug.Log("Player mendekati Annisa, menampilkan UI yang sesuai.");
    }

    IEnumerator HideCompletePanelAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        AnnisaChatCoxPanelComplet?.SetActive(false);
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
        AnnisaChatCoxPanelComplet?.SetActive(false);
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
