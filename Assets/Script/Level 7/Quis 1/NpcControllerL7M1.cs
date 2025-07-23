using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NpcControllerL7M1 : MonoBehaviour
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

    [Header("Dialog")]
    [TextArea(2, 5)] public string[] dialogKalimatAwal;
    [TextArea(2, 5)] public string[] dialogSetelahBenar;

    private int indexDialog = 0;
    private int indexDialogComplete = 0;

    [Header("Timer")]
    public TimeManager timeManager;
    public float waktuMisi = 120f;

    [Header("Item Collector (Multi Item)")]
    public ItemCollector itemCollector;

    [Header("PlayerPrefs Score")]
    public string namaPlayerPrefsScore = "L1M1";

    [Header("Feedback Benar")]
    public GameObject benarPanel; // ✅ UI feedback benar

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
        benarPanel?.SetActive(false);

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

        if (itemCollector != null)
        {
            if (itemCollector.HasCompletedMission())
            {
                isMissionCompleted = true;
                StartCoroutine(ShowFeedbackThenCompleteDialog());
            }
            else if (itemCollector.IsMissionStarted())
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

        itemCollector.StartCollecting();
    }

    IEnumerator ShowFeedbackThenCompleteDialog()
    {
        chatBoxUI.SetActive(false);
        chatBoxNotComplete.SetActive(false);
        ControllerPanel.SetActive(false);
        returnToNPCText.SetActive(false);
        uiTaskPanel.SetActive(false);
        timeManager?.StopTimer();

        // ✅ Tampilkan feedback benar selama 3 detik
        if (benarPanel != null)
        {
            benarPanel.SetActive(true);
            yield return new WaitForSeconds(2f);
            benarPanel.SetActive(false);
        }

        // ✅ Simpan skor
        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, 100);
            PlayerPrefs.Save();
        }

        // ✅ Lanjutkan ke dialog selesai
        chatBoxComplete.SetActive(true);
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

        if (!PlayerPrefs.HasKey(namaPlayerPrefsScore))
        {
            PlayerPrefs.SetInt(namaPlayerPrefsScore, 0);
            PlayerPrefs.Save();
        }
    }
}
