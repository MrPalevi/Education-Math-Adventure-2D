using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnnisaController : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

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
    public GameObject UjangChatBox;
    public GameObject UjangChatBoxNolak;

    [Header("Dialog")]
    [TextArea(2, 5)] public string[] dialogKalimatAwal;
    [TextArea(2, 5)] public string[] dialogSetelahBenar;

    [Header("Audio Dialog")]
    public AudioSource audioSource;
    public AudioClip[] dialogKalimatAwalAudio;
    public AudioClip[] dialogSetelahBenarAudio;


    private int indexDialog = 0;
    private int indexDialogComplete = 0;

    [Header("Timer")]
    public TimeManager timeManager;
    public float waktuMisi = 120f;

    public PencilCollector pencilCollector;

    [Header("PlayerPrefs Score")]
    public string namaPlayerPrefsScore = "L1M1";

    private bool isMissionStarted = false;
    private bool isMissionCompleted = false;
    private bool isPlayerInRange = false;
    private bool isRejected = false;

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
            spriteRenderer.flipX = player.position.x < transform.position.x;

            if (isPlayerInRange && !isMissionStarted && !chatBoxUI.activeSelf && !isRejected)
            {
                TampilkanChatAwal();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        isPlayerInRange = true;

        if (pencilCollector != null)
        {
            if (pencilCollector.HasCompletedMission())
            {
                isMissionCompleted = true;
                ShowCompleteDialog();
            }
            else if (pencilCollector.IsMissionStarted())
            {
                chatBoxNotComplete.SetActive(true);
                chatBoxUI.SetActive(false);
                chatBoxComplete.SetActive(false);
                ControllerPanel.SetActive(false);
                StartCoroutine(HideChatBoxNotCompleteAfterDelay(4f));
            }
            else
            {
                TampilkanChatAwal();
            }
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
        PlayDialogAudio(dialogKalimatAwalAudio, indexDialog);

        terimaButton.gameObject.SetActive(true);
        tolakButton.gameObject.SetActive(true);
        lanjutButton.gameObject.SetActive(false);
        tutupButton.gameObject.SetActive(false);
    }

    void TerimaMisi()
    {
        isMissionStarted = true;
        indexDialog++;
        UjangChatBox.SetActive(true);
        // TampilkanDialog();
        chatBoxUI.SetActive(false);

        terimaButton.gameObject.SetActive(false);
        tolakButton.gameObject.SetActive(false);
        lanjutButton.gameObject.SetActive(true);
    }

    void TolakMisi()
    {
        UjangChatBoxNolak.SetActive(true);
        isMissionStarted = false;
        isRejected = true;
        chatBoxUI.SetActive(false);
        Teleport.SetActive(false);
    }

    public void TutupDialogUjang()
    {
        UjangChatBoxNolak.SetActive(false);
        ControllerPanel.SetActive(true);
    }

    public void LanjutDialogUjang()
    {
        UjangChatBox.SetActive(false);
        TampilkanDialog();
        chatBoxUI.SetActive(true);
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

            // Mainkan audio sesuai indeks
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
        uiTaskPanel.SetActive(true);
        ControllerPanel.SetActive(true);
        Teleport.SetActive(true);

        if (timeManager != null)
        {
            timeManager.totalTimeInSeconds = waktuMisi;
            timeManager.StartTimer();
        }

        pencilCollector.StartCollecting();
    }

    void ShowCompleteDialog()
    {
        chatBoxComplete.SetActive(true);
        chatBoxUI.SetActive(false);
        chatBoxNotComplete.SetActive(false);
        ControllerPanel.SetActive(false);
        returnToNPCText.SetActive(false);
        uiTaskPanel.SetActive(false);
        timeManager?.StopTimer();

        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, 100);
            PlayerPrefs.Save();
            Debug.Log($"Skor berhasil disimpan untuk {namaPlayerPrefsScore} = 100");
        }
        else
        {
            Debug.Log($"Skor {namaPlayerPrefsScore} sudah pernah disimpan, tidak ditimpa ulang.");
        }

        indexDialogComplete = 0;
        if (dialogSetelahBenar.Length > 0)
        {
            chatTextComplete.text = dialogSetelahBenar[indexDialogComplete];
            PlayDialogAudio(dialogSetelahBenarAudio, indexDialogComplete);
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
            PlayDialogAudio(dialogSetelahBenarAudio, indexDialogComplete);
        }

        if (indexDialogComplete == dialogSetelahBenar.Length - 1)
        {
            nextCompleteButton.gameObject.SetActive(false);
            closeCompleteButton.gameObject.SetActive(true);
            chestBox.SetActive(true);
            Teleport.SetActive(false);
        }
    }


    void TutupChatBoxComplete()
    {
        chatBoxComplete.SetActive(false);
        ControllerPanel.SetActive(true);
        Stop.SetActive(false);
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

        // ❌ Simpan nilai 0 hanya jika belum pernah menyelesaikan
        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, 0);
            PlayerPrefs.Save();
            Debug.Log($"Skor gagal disimpan (0) untuk {namaPlayerPrefsScore}");
        }

        // pencilCollector.ResetCollector(); // Opsional jika ingin reset ulang
    }

    void PlayDialogAudio(AudioClip[] clips, int index)
    {
        if (audioSource == null || clips == null || index >= clips.Length || clips[index] == null)
            return;

        audioSource.Stop();
        audioSource.clip = clips[index];
        audioSource.Play();
    }

}
