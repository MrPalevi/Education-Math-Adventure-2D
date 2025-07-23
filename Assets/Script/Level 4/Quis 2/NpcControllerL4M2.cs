using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcControllerL4M2 : MonoBehaviour
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
    public GameObject panelSoal;
    public TimeManager timeManager;

    [Header("Referensi Objek Misi")]
    public GameObject DadangChatBoxPanelComplet;
    public GameObject chestBox;

    [Header("Feedback UI")]
    public GameObject feedbackBenar;
    public GameObject feedbackSalah;
    public float feedbackDuration = 2f;
    // public GameObject bridge;

    [Header("Pengaturan Skor")]
    public string namaPlayerPrefsScore = "L1M3"; // ✅ Bisa diatur dari Inspector

    [Header("Pengaturan Posisi ChestBox")]
    public Vector2 offsetChestBox = new Vector2(2f, 0f); // Bisa diubah di Inspector


    void Start()
    {
        PlayerPrefs.DeleteAll(); // Menghapus SEMUA data yang tersimpan
        PlayerPrefs.Save();
        Debug.Log("Smua playerPrefs di reset");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        controllerPanel?.SetActive(true);
        DadangChatBoxPanelComplet?.SetActive(false);
        Stop?.SetActive(true);
        chestBox?.SetActive(false);
        // bridge?.SetActive(false);

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
        DadangChatBoxPanelComplet?.SetActive(false);
    }

    public void MulaiMisiPuzzle()
    {
        chatBoxUI?.SetActive(false);
        panelSoal?.SetActive(true);
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
        panelSoal?.SetActive(false);
        Stop.SetActive(false);
        StartCoroutine(ShowFeedbackThenComplete(isCorrect));
    }

    IEnumerator ShowFeedbackThenComplete(bool isCorrect)
    {
        int score = isCorrect ? 100 : 0;

        if (isCorrect)
        {
            feedbackBenar?.SetActive(true);
        }
        else
        {
            feedbackSalah?.SetActive(true);
            chestBox?.SetActive(false); // Pastikan tidak muncul saat salah
        }

        // Simpan skor jika belum pernah disimpan
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

        yield return new WaitForSeconds(feedbackDuration);

        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);

        if (isCorrect)
        {
            yield return new WaitForSeconds(0.5f);

            // ⬇️ Tentukan posisi baru berdasarkan arah NPC
            Vector3 chestPos = transform.position;

            if (spriteRenderer.flipX)
            {
                chestPos += new Vector3(-offsetChestBox.x, offsetChestBox.y, 0f); // Player dari kanan → chest di kiri
            }
            else
            {
                chestPos += new Vector3(offsetChestBox.x, offsetChestBox.y, 0f); // Player dari kiri → chest di kanan
            }

            chestBox.transform.position = chestPos;
            chestBox?.SetActive(true);
        }
        isMissionCompleted = true;
        DadangChatBoxPanelComplet?.SetActive(true);
        controllerPanel?.SetActive(true);
        // bridge?.SetActive(true);
    }

    void ShowChat()
    {
        if (isTimeOut) return;

        isChatShown = true;

        if (isMissionCompleted && DadangChatBoxPanelComplet != null)
        {
            chatBoxUI?.SetActive(false);
            controllerPanel?.SetActive(true);
            DadangChatBoxPanelComplet.SetActive(true);
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
        DadangChatBoxPanelComplet?.SetActive(false);
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
        DadangChatBoxPanelComplet?.SetActive(false);
        feedbackBenar?.SetActive(false);
        feedbackSalah?.SetActive(false);
        timeManager?.StopTimer();
        Debug.Log("Misi di-reset."); 
    }
}


