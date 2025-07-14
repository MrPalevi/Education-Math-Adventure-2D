using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinaController : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

    public GameObject chatBoxUI;
    public GameObject controllerPanel;

    private bool isChatShown = false;
    private bool isPlayerInRange = false;
    private bool isMissionCompleted = false;
    private bool isPanelCompleteShowing = false;

    [Header("Misi Puzzle")]
    public GameObject panelPuzzle;
    public TimeManager timeManager;

    [Header("Referensi Objek Misi")]
    public GameObject rinaChatBoxPanelComplete;
    // private bool missionFinished = false;
    private bool isTimeOut = false; // Tambahan untuk blokir chat saat timeout
    public GameObject bridge;
    public GameObject chestBox;

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

        if (bridge != null)
        {
            bridge.SetActive(false);
        }
    }


    void HandlePuzzleResult(bool isWin)
    {
        // missionFinished = true;
        isMissionCompleted = true;

        int score = isWin ? 100 : 0;
        PlayerPrefs.SetInt("L1M2", score);
        PlayerPrefs.Save();

        Debug.Log("Hasil disimpan ke PlayerPrefs L1M2: " + score);
        if (bridge != null)
        {
            bridge.SetActive(true);
            chestBox.SetActive(true);
        }

    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);

        // Flip arah NPC
        if (distance <= detectionRange)
        {
            spriteRenderer.flipX = player.position.x < transform.position.x;
        }

        // Validasi agar tidak memanggil ShowChat saat misi selesai
        if (isPlayerInRange && !isChatShown && !isMissionCompleted)
        {
            ShowChat();
        }

        // Jika misi selesai dan player masih di area, tampilkan panel complete jika belum tampil
        if (isPlayerInRange && isMissionCompleted && !isPanelCompleteShowing)
        {
            ShowCompletePanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (!isChatShown && !isTimeOut) // Tambahan: blokir saat timeout
            {
                ShowChat();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isChatShown = false;
            chatBoxUI?.SetActive(false);

            if (rinaChatBoxPanelComplete != null)
                rinaChatBoxPanelComplete.SetActive(false);

            isPanelCompleteShowing = false;
        }
    }

    public void FeedbackBenar()
    {
        PlayerPrefs.SetInt("L1M2", 100);
        PlayerPrefs.Save();

        isMissionCompleted = true;
        chatBoxUI.SetActive(false);
        controllerPanel.SetActive(true);

        Debug.Log("Feedback benar. Skor 100 disimpan. Misi selesai.");
    }

    public void FeedbackSalah()
    {
        PlayerPrefs.SetInt("L1M2", 0);
        PlayerPrefs.Save();

        isMissionCompleted = false;
        chatBoxUI.SetActive(false);
        controllerPanel.SetActive(true);

        Debug.Log("Feedback salah. Skor 0 disimpan.");
    }

    void ShowChat()
    {
        if (isTimeOut) return; // Tambahan: blokir jika waktu habis

        isChatShown = true;

        if (isMissionCompleted && rinaChatBoxPanelComplete != null)
        {
            rinaChatBoxPanelComplete.SetActive(true);
            chatBoxUI?.SetActive(false);
            StartCoroutine(HideRinaCompletePanelAfterDelay());
        }
        else
        {
            chatBoxUI?.SetActive(true);
        }

        controllerPanel?.SetActive(false);
        Debug.Log("Player mendekati Rina, tampilkan chatBox");
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
        chatBoxUI?.SetActive(false);
        panelPuzzle?.SetActive(true);
        controllerPanel?.SetActive(false);
        isTimeOut = false; // Reset timeout saat mulai ulang misi

        timeManager?.StartTimer();

        Debug.Log("Misi dimulai: Puzzle aktif dan timer berjalan.");
    }

    public void TolakMisiPuzzle()
    {
        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        Debug.Log("Player menolak misi.");
    }

    IEnumerator HideRinaCompletePanelAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        if (rinaChatBoxPanelComplete != null)
        {
            rinaChatBoxPanelComplete.SetActive(false);
        }
        isPanelCompleteShowing = false;
    }

    void HandleTimeOut()
    {
        panelPuzzle?.SetActive(false);
        chatBoxUI?.SetActive(false);
        isChatShown = false;
        isMissionCompleted = false;
        isTimeOut = true; // Aktifkan blokir agar chat tidak muncul

        Debug.Log("Waktu habis, misi di-reset.");

        // Jangan munculkan ChatBox walaupun player masih di dalam collider
    }

}
