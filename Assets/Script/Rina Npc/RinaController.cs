using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinaController : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 4f;

    public GameObject chatBoxUI;
    public GameObject controllerPanel;
    public GameObject UjangChatBox;

    private bool isChatShown = false;
    private bool isPlayerInRange = false;
    private bool isMissionCompleted = false;
    private bool isPanelCompleteShowing = false;

    [Header("Misi Puzzle")]
    public GameObject panelPuzzle;
    public TimeManager timeManager;

    [Header("Referensi Objek Misi")]
    public GameObject rinaChatBoxPanelComplete;
    public GameObject bridge;
    public GameObject chestBox;

    [Header("Pengaturan Skor")]
    public string namaPlayerPrefsScore = "L1M2";

    private bool isTimeOut = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);

        if (PuzzleManager2.instance != null)
        {
            PuzzleManager2.instance.OnPuzzleFinished += HandlePuzzleResult;
        }

        if (timeManager != null)
        {
            timeManager.OnTimeOut += HandleTimeOut;
        }

        bridge?.SetActive(false);
        chestBox?.SetActive(false);
    }

    void HandlePuzzleResult(bool isWin)
    {
        isMissionCompleted = true;

        int score = isWin ? 100 : 0;

        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, score);
            PlayerPrefs.Save();
            Debug.Log($"Skor {score} disimpan ke PlayerPrefsScore dengan key: {namaPlayerPrefsScore}");
        }
        else
        {
            Debug.Log($"Skor sudah pernah disimpan sebelumnya di {namaPlayerPrefsScore}, tidak ditimpa.");
        }

        bridge?.SetActive(true);

        // ✅ chestBox hanya muncul jika jawaban benar
        if (isWin)
        {
            chestBox?.SetActive(true);
        }
        else
        {
            chestBox?.SetActive(false);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);
        spriteRenderer.flipX = player.position.x < transform.position.x;

        if (isPlayerInRange && !isChatShown && !isMissionCompleted)
        {
            ShowChat();
        }

        if (isPlayerInRange && isMissionCompleted && !isPanelCompleteShowing)
        {
            ShowCompletePanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInRange = true;

        if (!isChatShown && !isMissionCompleted && !isTimeOut)
        {
            ShowChat();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInRange = false;
        isChatShown = false;
        isPanelCompleteShowing = false;

        chatBoxUI?.SetActive(false);
        rinaChatBoxPanelComplete?.SetActive(false);
    }

    public void FeedbackBenar()
    {
        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, 100);
            PlayerPrefs.Save();
            Debug.Log($"Skor 100 disimpan untuk {namaPlayerPrefsScore}");
        }
        else
        {
            Debug.Log($"Skor sudah pernah disimpan di {namaPlayerPrefsScore}, tidak ditimpa.");
        }

        isMissionCompleted = true;
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
    }

    public void FeedbackSalah()
    {
        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, 0);
            PlayerPrefs.Save();
            Debug.Log($"Skor 0 disimpan untuk {namaPlayerPrefsScore}");
        }

        isMissionCompleted = false;
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
    }

    void ShowChat()
    {
        if (isTimeOut) return;

        isChatShown = true;

        // Tidak perlu menampilkan panelComplete di sini lagi
        chatBoxUI?.SetActive(true);
        controllerPanel?.SetActive(false);
    }

    void ShowCompletePanel()
    {
        if (rinaChatBoxPanelComplete != null && !isPanelCompleteShowing)
        {
            rinaChatBoxPanelComplete.SetActive(true);
            chatBoxUI?.SetActive(false);
            controllerPanel?.SetActive(true);
            isPanelCompleteShowing = true;
            StartCoroutine(HideRinaCompletePanelAfterDelay());
        }
    }

    public void MulaiMisiPuzzle()
    {
        UjangChatBox?.SetActive(false);
        chatBoxUI?.SetActive(false);
        panelPuzzle?.SetActive(true);
        controllerPanel?.SetActive(false);
        isTimeOut = false;

        timeManager?.StartTimer();
    }

    public void TolakMisiPuzzle()
    {
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
    }

    public void ShowChatUjang()
    {
        UjangChatBox?.SetActive(true);
        chatBoxUI?.SetActive(false);
    }

    IEnumerator HideRinaCompletePanelAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        rinaChatBoxPanelComplete?.SetActive(false);
    }

    void HandleTimeOut()
    {
        panelPuzzle?.SetActive(false);
        chatBoxUI?.SetActive(false);
        isChatShown = false;
        isMissionCompleted = false;
        isTimeOut = true;

        Debug.Log("Waktu habis, misi puzzle di-reset.");
    }
}
