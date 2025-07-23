using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NpcControllerL7M2 : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 4.5f;

    [Header("UI Komponen")]
    public GameObject chatBoxUI;
    public TextMeshProUGUI chatText;
    public Button terimaButton;
    public Button tolakButton;
    public Button lanjutButton;
    public Button tutupButton;

    public GameObject chatBoxComplete;
    public TextMeshProUGUI chatTextComplete;
    public Button nextCompleteButton;
    public Button closeCompleteButton;

    public GameObject chatBoxNotComplete;
    public GameObject ControllerPanel;
    public GameObject returnToNPCText;
    public GameObject uiTaskPanel;
    public GameObject Teleport;
    public GameObject Stop;
    public GameObject chestBox;

    [Header("Dialog")]
    [TextArea(2, 5)] public string[] dialogKalimatAwal;
    [TextArea(2, 5)] public string[] dialogSetelahBenar;

    private int indexDialog = 0;
    private int indexDialogComplete = 0;

    [Header("Timer")]
    public TimeManager timeManager;
    public float waktuMisi = 120f;

    [Header("Random Soal")]
    public RandomSoalL7M2 randomSoal;

    [Header("PlayerPrefs Score")]
    public string namaPlayerPrefsScore = "L7M2";

    [Header("Feedback Akhir")]
    public GameObject feedbackBenarAkhir;
    public GameObject feedbackSalahAkhir;

    private bool isMissionStarted = false;
    private bool isMissionCompleted = false;
    private bool isPlayerInRange = false;
    private bool isRejected = false;
    private bool misiBenarSemua = false; // ✅ penanda untuk chestBox

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        chatBoxUI?.SetActive(false);
        chatBoxComplete?.SetActive(false);
        chatBoxNotComplete?.SetActive(false);
        returnToNPCText?.SetActive(false);
        uiTaskPanel?.SetActive(false);
        Teleport?.SetActive(false);
        Stop?.SetActive(true);
        chestBox?.SetActive(false);
        ControllerPanel?.SetActive(true);

        if (feedbackBenarAkhir != null) feedbackBenarAkhir.SetActive(false);
        if (feedbackSalahAkhir != null) feedbackSalahAkhir.SetActive(false);

        if (timeManager != null)
            timeManager.OnTimeOut += HandleWaktuHabis;

        terimaButton.onClick.AddListener(TerimaMisi);
        tolakButton.onClick.AddListener(TolakMisi);
        lanjutButton.onClick.AddListener(LanjutDialog);
        tutupButton.onClick.AddListener(TutupChatBox);

        nextCompleteButton.onClick.AddListener(LanjutDialogComplete);
        closeCompleteButton.onClick.AddListener(TutupChatBoxComplete);
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance <= detectionRange)
            {
                spriteRenderer.flipX = player.position.x < transform.position.x;
            }

            if (isPlayerInRange && !isMissionStarted && !isRejected && !isMissionCompleted && !chatBoxUI.activeSelf)
            {
                TampilkanChatAwal();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInRange = true;

        if (isMissionCompleted)
        {
            // ✅ Hanya feedback & chatBoxComplete, jangan pernah munculkan chatBoxUI
            StartCoroutine(TampilkanFeedbackAkhirCoroutine());
        }
        else if (isMissionStarted)
        {
            chatBoxNotComplete.SetActive(true);
            chatBoxUI.SetActive(false);
            chatBoxComplete.SetActive(false);
            ControllerPanel.SetActive(false);
            StartCoroutine(HideChatBoxNotCompleteAfterDelay(2f));
        }
        else
        {
            TampilkanChatAwal();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            isRejected = false;
        }
    }

    void TampilkanChatAwal()
    {
        indexDialog = 0;
        chatBoxUI.SetActive(true);
        chatText.text = dialogKalimatAwal[indexDialog];
        ControllerPanel.SetActive(false);

        terimaButton.gameObject.SetActive(true);
        tolakButton.gameObject.SetActive(true);
        lanjutButton.gameObject.SetActive(false);
        tutupButton.gameObject.SetActive(false);
    }

    void TerimaMisi()
    {
        isMissionStarted = true;
        indexDialog++;
        TampilkanDialog();

        terimaButton.gameObject.SetActive(false);
        tolakButton.gameObject.SetActive(false);
        lanjutButton.gameObject.SetActive(true);
    }

    void TolakMisi()
    {
        isMissionStarted = false;
        isRejected = true;

        chatBoxUI.SetActive(false);
        ControllerPanel.SetActive(true);
        Teleport.SetActive(false);
    }

    void LanjutDialog()
    {
        indexDialog++;
        TampilkanDialog();
    }

    void TampilkanDialog()
    {
        if (indexDialog < dialogKalimatAwal.Length)
        {
            chatText.text = dialogKalimatAwal[indexDialog];
        }

        if (indexDialog == dialogKalimatAwal.Length - 1)
        {
            lanjutButton.gameObject.SetActive(false);
            tutupButton.gameObject.SetActive(true);
        }
    }

    void TutupChatBox()
    {
        chatBoxUI.SetActive(false);
        uiTaskPanel.SetActive(true);
        ControllerPanel.SetActive(true);
        Teleport.SetActive(true);

        if (timeManager != null)
        {
            timeManager.totalTimeInSeconds = waktuMisi;
            timeManager.StartTimer();
        }

        randomSoal?.MulaiMisi();
    }

    public void OnMissionCompleted(bool semuaBenar)
    {
        isMissionCompleted = true;
        isMissionStarted = false;
        misiBenarSemua = semuaBenar;

        returnToNPCText.SetActive(true);
        uiTaskPanel.SetActive(false);
        timeManager?.StopTimer();
    }

    IEnumerator TampilkanFeedbackAkhirCoroutine()
    {
        returnToNPCText.SetActive(false);

        // ✅ Pastikan chatBoxComplete & chatBoxUI MATI dulu
        chatBoxComplete.SetActive(false);
        chatBoxUI.SetActive(false);
        // MASIH BUG BUTUH PERBAIKAN
        // if (misiBenarSemua)
        // {
        //     if (feedbackBenarAkhir != null)
        //     {
        //         feedbackBenarAkhir.SetActive(true);
        //         yield return new WaitForSeconds(2f);
        //         feedbackBenarAkhir.SetActive(false);
        //     }
        // }
        // else
        // {
        //     if (feedbackSalahAkhir != null)
        //     {
        //         feedbackSalahAkhir.SetActive(true);
        //         yield return new WaitForSeconds(2f);
        //         feedbackSalahAkhir.SetActive(false);
        //     }
        // }

        // ✅ Tambah jeda 0.5f sebelum chatBoxComplete muncul
        yield return new WaitForSeconds(0.5f);

        ShowCompleteDialog();
    }

    void ShowCompleteDialog()
    {
        chatBoxComplete.SetActive(true);
        chatBoxUI.SetActive(false);
        chatBoxNotComplete.SetActive(false);
        ControllerPanel.SetActive(false);
        returnToNPCText.SetActive(false);
        uiTaskPanel.SetActive(false);

        // ✅ Simpan skor hanya jika belum pernah disimpan
        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            int skor = misiBenarSemua ? 100 : 0;
            PlayerPrefs.SetInt(namaPlayerPrefsScore, skor);
            PlayerPrefs.Save();
        }

        indexDialogComplete = 0;
        if (dialogSetelahBenar.Length > 0)
        {
            chatTextComplete.text = dialogSetelahBenar[indexDialogComplete];
        }
        else
        {
            chatTextComplete.text = "Terima kasih atas bantuanmu!";
        }

        nextCompleteButton.gameObject.SetActive(dialogSetelahBenar.Length > 1);
        closeCompleteButton.gameObject.SetActive(dialogSetelahBenar.Length <= 1);
    }

    void LanjutDialogComplete()
    {
        indexDialogComplete++;

        if (indexDialogComplete < dialogSetelahBenar.Length)
        {
            chatTextComplete.text = dialogSetelahBenar[indexDialogComplete];
        }

        if (indexDialogComplete == dialogSetelahBenar.Length - 1)
        {
            nextCompleteButton.gameObject.SetActive(false);
            closeCompleteButton.gameObject.SetActive(true);
        }
    }

    void TutupChatBoxComplete()
    {
        chatBoxComplete.SetActive(false);
        ControllerPanel.SetActive(true);
        Stop.SetActive(false);

        if (misiBenarSemua)
        {
            chestBox.SetActive(true);
            Teleport.SetActive(false);
        }
        else
        {
            chestBox.SetActive(false);
        }
    }

    IEnumerator HideChatBoxNotCompleteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (chatBoxNotComplete.activeSelf)
        {
            chatBoxNotComplete.SetActive(false);
            ControllerPanel.SetActive(true);
        }
    }

    void HandleWaktuHabis()
    {
        chatBoxUI?.SetActive(false);
        chatBoxComplete?.SetActive(false);
        chatBoxNotComplete?.SetActive(false);
        ControllerPanel?.SetActive(true);
        uiTaskPanel?.SetActive(false);
        returnToNPCText?.SetActive(false);
        Teleport?.SetActive(false);
        isMissionStarted = false;

        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, 0);
            PlayerPrefs.Save();
        }
    }
}
