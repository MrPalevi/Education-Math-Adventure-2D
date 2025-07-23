using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcControllerL5M1 : MonoBehaviour
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
    public string namaPlayerPrefsScore = "L5M1"; // ✅ Bisa diatur dari Inspector

    [Header("Pengaturan Posisi ChestBox")]
    public Vector2 offsetChestBox = new Vector2(2f, 0f); // Bisa diubah di Inspector

    public GameObject keranjangBox;
    public Vector2 offsetkeranjangBox = new Vector2(0.5f, 0f); // Bisa diubah di Inspector

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

        //test
        Vector3 keranjangPos = transform.position;

            if (spriteRenderer.flipX)
            {
                keranjangPos += new Vector3(-offsetkeranjangBox.x, offsetkeranjangBox.y, 0f); // Player dari kanan → chest di kiri
            }
            else
            {
                keranjangPos += new Vector3(offsetkeranjangBox.x, offsetkeranjangBox.y, 0f); // Player dari kiri → chest di kanan
            }

            keranjangBox.transform.position = keranjangPos;
            keranjangBox?.SetActive(true);

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

        // ✅ Tunda 0.5 detik baru munculkan chestBox (hanya jika benar)
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

        Debug.Log("Player mendekati Dede, menampilkan UI yang sesuai.");
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

        // foreach (DropZoneL5M1 dz in FindObjectsOfType<DropZoneL5M1>())
        // {
        //     dz.Clear();
        // }

        Debug.Log("Misi di-reset.");
    }

}


