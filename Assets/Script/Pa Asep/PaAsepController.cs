using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PaAsepController : MonoBehaviour
{
    public static PaAsepController instance;

    [Header("Komponen UI")]
    public GameObject chatBoxUI;
    public TextMeshProUGUI chatText;
    public Button terimaButton;
    public Button lanjutButtonUjang;
    public Button tolakButton;
    public Button lanjutButton;
    public Button tutupButton;
    public GameObject Portal;
    public GameObject NextLevel;

    [Header("ChatBox UI Tambahan")]
    public GameObject ChatBoxUiComplete;
    public TextMeshProUGUI chatTextComplete;
    public Button nextCompleteButton;
    public Button closeCompleteButton;

    public GameObject ChatBoxUiNoComplete;
    public GameObject panelMisiAngka;
    public GameObject controllerPanel;
    public GameObject chestBox;
    public GameObject ReturnToPaAsep;
    public GameObject TimeOverPanel;
    public GameObject UjangChatBox;

    [Header("Timer")]
    public TimeManager timeManager;
    public float waktuMisi = 120f;

    [Header("Dialog")]
    [TextArea(2, 5)] public string[] dialogKalimatAwal;
    [TextArea(2, 5)] public string[] dialogSetelahBenar;
    [TextArea(2, 5)] public string[] dialogSetelahSalah;

    [Header("Pengaturan Skor")]
    public string namaPlayerPrefsScore = "L1M1";

    [Header("Audio Dialog")]
    public AudioSource audioSource;
    public AudioClip[] dialogKalimatAwalAudio;
    public AudioClip[] dialogSetelahBenarAudio;
    public AudioClip[] dialogSetelahSalahAudio;


    private int indexDialog = 0;
    private int indexDialogComplete = 0;

    private Transform player;
    private SpriteRenderer spriteRenderer;

    public float detectionRange = 5f;

    private bool isMissionStarted = false;
    private bool isMissionCompleted = false;
    private bool isJawabanBenar = false;
    private bool isPlayerInRange = false;
    private bool isRejected = false;
    private bool isTimeOut = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (timeManager != null)
            timeManager.OnTimeOut += HandleTimeOut;

        chatBoxUI.SetActive(false);
        ChatBoxUiComplete.SetActive(false);
        ChatBoxUiNoComplete.SetActive(false);
        panelMisiAngka.SetActive(false);
        chestBox.SetActive(false);
        ReturnToPaAsep.SetActive(false);
        TimeOverPanel.SetActive(false);
        controllerPanel.SetActive(true);
        NextLevel.SetActive(false);

        lanjutButton.onClick.AddListener(LanjutDialog);
        lanjutButtonUjang.onClick.AddListener(LanjutDialog);
        terimaButton.onClick.AddListener(TerimaMisi);
        tolakButton.onClick.AddListener(TolakMisi);
        tutupButton.onClick.AddListener(TutupChatBox);

        nextCompleteButton.onClick.AddListener(LanjutDialogComplete);
        closeCompleteButton.onClick.AddListener(TutupChatBoxComplete);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.position, transform.position);
        spriteRenderer.flipX = player.position.x < transform.position.x;

        if (isPlayerInRange && !isMissionStarted && !chatBoxUI.activeSelf && !isRejected)
        {
            // TampilkanChatAwal();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInRange = true;

        if (isMissionCompleted)
        {
            ReturnToPaAsep.SetActive(false);
            panelMisiAngka.SetActive(false);
            StartCoroutine(TampilkanFeedbackSelesaiDanDialog());
        }
        else if (isMissionStarted)
        {
            ChatBoxUiNoComplete.SetActive(true);
            StartCoroutine(SembunyikanNoCompleteSetelahDelay());
        }
        else if (!chatBoxUI.activeSelf && !isRejected)
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
        controllerPanel.SetActive(false);
        terimaButton.gameObject.SetActive(true);
        tolakButton.gameObject.SetActive(true);
        lanjutButton.gameObject.SetActive(false);
        tutupButton.gameObject.SetActive(false);
        PlayDialogAudio(dialogKalimatAwalAudio, indexDialog);
    }

    void TerimaMisi()
    {
        UjangChatBox.SetActive(true);
        chatBoxUI.SetActive(false);
        // isMissionStarted = true;
        // indexDialog++;
        // TampilkanDialog();
        terimaButton.gameObject.SetActive(false);
        tolakButton.gameObject.SetActive(false);
        lanjutButton.gameObject.SetActive(true);
        // Portal.gameObject.SetActive(true);
    }

    void TolakMisi()
    {
        isMissionStarted = false;
        chatBoxUI.SetActive(false);
        controllerPanel.SetActive(true);
        isRejected = true;
    }

    void LanjutDialog()
    {
        UjangChatBox.SetActive(false);
        chatBoxUI.SetActive(true);
        isMissionStarted = true;
        terimaButton.gameObject.SetActive(false);
        tolakButton.gameObject.SetActive(false);
        lanjutButtonUjang.gameObject.SetActive(true);
        Portal.gameObject.SetActive(true);
        indexDialog++;
        TampilkanDialog();
    }

    void TampilkanDialog()
    {
        if (indexDialog < dialogKalimatAwal.Length)
        {
            chatText.text = dialogKalimatAwal[indexDialog];
            PlayDialogAudio(dialogKalimatAwalAudio, indexDialog);
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
        panelMisiAngka?.SetActive(true);
        controllerPanel?.SetActive(true);

        if (timeManager != null)
        {
            timeManager.totalTimeInSeconds = waktuMisi;
            timeManager.StartTimer();
        }
    }

    public void NotifyMisiSelesai(bool benar)
    {
        isMissionCompleted = true;
        isJawabanBenar = benar;
        ReturnToPaAsep.SetActive(true);

        if (timeManager != null)
        {
            timeManager.StopTimer();
        }
    }

    IEnumerator TampilkanFeedbackSelesaiDanDialog()
    {
        yield return new WaitForSeconds(0.5f);

        // Tampilkan feedback sesuai jawaban
        if (isJawabanBenar)
        {
            AngkaCollector.instance.feedbackBenarUI.SetActive(true);
            AngkaCollector.instance.feedbackSalahUI.SetActive(false);
        }
        else
        {
            AngkaCollector.instance.feedbackSalahUI.SetActive(true);
            AngkaCollector.instance.feedbackBenarUI.SetActive(false);
        }

        if (timeManager != null)
        {
            timeManager.StopTimer();
        }

        // Simpan skor hanya jika belum pernah disimpan
        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            int score = isJawabanBenar ? 100 : 0;
            PlayerPrefs.SetInt(namaPlayerPrefsScore, score);
            PlayerPrefs.Save();
            Debug.Log("Skor " + score + " disimpan di PlayerPrefs dengan key: " + namaPlayerPrefsScore);
        }
        else
        {
            Debug.Log("Skor untuk " + namaPlayerPrefsScore + " sudah ada, tidak ditimpa.");
        }

        // ✅ Hentikan otomatis lanjut, tunggu player klik tombol di feedback UI
    }


    void TampilkanDialogComplete()
    {
        ChatBoxUiComplete.SetActive(true);
        controllerPanel.SetActive(false);
        indexDialogComplete = 0;

        var dialog = isJawabanBenar ? dialogSetelahBenar : dialogSetelahSalah;
        chatTextComplete.text = dialog[indexDialogComplete];

        // Play first audio of dialog complete
        if (isJawabanBenar)
            PlayDialogAudio(dialogSetelahBenarAudio, indexDialogComplete);
        else
            PlayDialogAudio(dialogSetelahSalahAudio, indexDialogComplete);

        nextCompleteButton.gameObject.SetActive(true);
        closeCompleteButton.gameObject.SetActive(false);
    }

    void PlayDialogAudio(AudioClip[] clips, int index)
    {
        if (audioSource == null || clips == null || index >= clips.Length || clips[index] == null)
            return;

        audioSource.Stop();
        audioSource.clip = clips[index];
        audioSource.Play();
    }

    void LanjutDialogComplete()
    {
        var dialog = isJawabanBenar ? dialogSetelahBenar : dialogSetelahSalah;
        var clips = isJawabanBenar ? dialogSetelahBenarAudio : dialogSetelahSalahAudio;

        indexDialogComplete++;
        if (indexDialogComplete < dialog.Length)
        {
            chatTextComplete.text = dialog[indexDialogComplete];
            PlayDialogAudio(clips, indexDialogComplete);
        }

        if (indexDialogComplete == dialog.Length - 1)
        {
            nextCompleteButton.gameObject.SetActive(false);
            closeCompleteButton.gameObject.SetActive(true);
        }
    }

    void TutupChatBoxComplete()
    {
        ChatBoxUiComplete.SetActive(false);
        controllerPanel.SetActive(true);

        if (isJawabanBenar)
            chestBox.SetActive(true);
            NextLevel.SetActive(true);
    }

    IEnumerator SembunyikanNoCompleteSetelahDelay()
    {
        yield return new WaitForSeconds(2f);
        ChatBoxUiNoComplete.SetActive(false);
    }

    void HandleTimeOut()
    {
        panelMisiAngka?.SetActive(false);
        chatBoxUI?.SetActive(false);
        isMissionStarted = false;
        isMissionCompleted = false;
        isJawabanBenar = false;
        isTimeOut = true;
        controllerPanel?.SetActive(true);
        Debug.Log("Waktu habis, misi di-reset.");
    }

    public void TutupFeedbackUI()
    {
        // Matikan semua feedback
        AngkaCollector.instance.feedbackBenarUI.SetActive(false);
        AngkaCollector.instance.feedbackSalahUI.SetActive(false);

        // Jika benar, aktifkan chestBox
        if (isJawabanBenar)
        {
            chestBox.SetActive(true);
            NextLevel.SetActive(true);
        }
        else
        {
            chestBox.SetActive(false);
            NextLevel.SetActive(false);
        }

        // Lanjut ke dialog complete
        TampilkanDialogComplete();
    }

}
