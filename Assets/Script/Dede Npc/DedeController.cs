using System.Collections;
using UnityEngine;

public class DedeController : MonoBehaviour
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
    public GameObject DedeChatCoxPanelComplet;
    public GameObject chestBox;
    public GameObject UjangChatBox;

    [Header("Feedback UI")]
    public GameObject feedbackBenar;
    public GameObject feedbackSalah;
    public float feedbackDuration = 2f;

    [Header("Pengaturan Skor")]
    public string namaPlayerPrefsScore = "L1M3";

    private bool lastIsCorrect = false; // ✅ simpan hasil terakhir

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        DedeChatCoxPanelComplet?.SetActive(false);
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
        DedeChatCoxPanelComplet?.SetActive(false);
    }

    public void MulaiMisiPuzzle()
    {
        UjangChatBox?.SetActive(false);
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

    public void ShowChatBoxUjang()
    {
        UjangChatBox?.SetActive(true);
        chatBoxUI?.SetActive(false);
    }

    public void OnPuzzleCheckResult(bool isCorrect)
    {
        timeManager?.StopTimer();
        panelPuzzle?.SetActive(false);
        Stop.SetActive(false);
        ShowFeedback(isCorrect);
    }

    private void ShowFeedback(bool isCorrect)
    {
        lastIsCorrect = isCorrect; // ✅ simpan hasil

        int score = isCorrect ? 100 : 0;

        if (isCorrect)
        {
            feedbackBenar?.SetActive(true);
            feedbackSalah?.SetActive(false);
        }
        else
        {
            feedbackSalah?.SetActive(true);
            feedbackBenar?.SetActive(false);
            chestBox?.SetActive(false); // pastikan mati saat salah
        }

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
    }

    public void CloseFeedback()
    {
        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);

        // ✅ Hanya tampilkan chestBox kalau benar
        if (lastIsCorrect)
        {
            chestBox?.SetActive(true);
        }
        else
        {
            chestBox?.SetActive(false);
        }

        isMissionCompleted = true;
        DedeChatCoxPanelComplet?.SetActive(true);
        controllerPanel?.SetActive(true);
    }

    void ShowChat()
    {
        if (isTimeOut) return;

        isChatShown = true;

        if (isMissionCompleted && DedeChatCoxPanelComplet != null)
        {
            chatBoxUI?.SetActive(false);
            controllerPanel?.SetActive(true);
            DedeChatCoxPanelComplet.SetActive(true);
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
        DedeChatCoxPanelComplet?.SetActive(false);
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
        DedeChatCoxPanelComplet?.SetActive(false);
        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);
        chestBox?.SetActive(false);
        timeManager?.StopTimer();

        foreach (DropZone dz in FindObjectsOfType<DropZone>())
        {
            dz.Clear();
        }

        Debug.Log("Misi di-reset.");
    }
}
