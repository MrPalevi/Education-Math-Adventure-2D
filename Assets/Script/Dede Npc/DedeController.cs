using System.Collections;
using UnityEngine;

public class DedeController : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

    public GameObject chatBoxUI;
    public GameObject controllerPanel;
    private bool isChatShown = false;
    private bool isPlayerInRange = false;
    private bool isMissionCompleted = false;
    private bool isTimeOut = false;

    [Header("Misi Puzzle")]
    public GameObject panelPuzzle;
    public TimeManager timeManager;

    [Header("Referensi Objek Misi")]
    public GameObject DedeChatCoxPanelComplet;

    [Header("Feedback UI")]
    public GameObject feedbackBenar;
    public GameObject feedbackSalah;
    public float feedbackDuration = 2f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        DedeChatCoxPanelComplet?.SetActive(false);

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
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (!isChatShown && !isTimeOut)
            {
                // Hanya jalankan ShowChat jika belum selesai
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
            DedeChatCoxPanelComplet?.SetActive(false);
        }
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
        // Hentikan Timer setelah puzzle selesai
        timeManager?.StopTimer();

        // Sembunyikan panel puzzle terlebih dahulu
        panelPuzzle?.SetActive(false);

        // Tampilkan feedback kemudian lanjut ke panel selesai
        StartCoroutine(ShowFeedbackThenComplete(isCorrect));
    }

    IEnumerator ShowFeedbackThenComplete(bool isCorrect)
    {
        if (isCorrect)
        {
            feedbackBenar?.SetActive(true);
            PlayerPrefs.SetInt("L1M3", 100);
        }
        else
        {
            feedbackSalah?.SetActive(true);
            PlayerPrefs.SetInt("L1M3", 0);
        }

        PlayerPrefs.Save();
        yield return new WaitForSeconds(feedbackDuration);

        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);

        isMissionCompleted = true;

        // Tampilkan UI panel complete dan aktifkan kontrol panel
        DedeChatCoxPanelComplet?.SetActive(true);
        controllerPanel?.SetActive(true);
        Debug.Log(PlayerPrefs.GetInt("L1M3"));
    }

    void ShowChat()
    {
        if (isTimeOut) return; // Blokir saat waktu habis

        isChatShown = true;

        if (isMissionCompleted && DedeChatCoxPanelComplet != null)
        {
            chatBoxUI?.SetActive(false); // Pastikan ini disembunyikan
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
        if (DedeChatCoxPanelComplet != null)
        {
            DedeChatCoxPanelComplet.SetActive(false);
        }
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

        timeManager?.StopTimer();

        // Reset dropzone
        foreach (DropZone dz in FindObjectsOfType<DropZone>())
        {
            dz.Clear();
        }

        Debug.Log("Misi di-reset.");
    }
}
