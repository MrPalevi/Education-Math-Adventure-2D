using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnnisaController : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

    public GameObject chatBoxUI;
    public GameObject ControllerPanel;
    public GameObject returnToNPCText;
    public GameObject chatBoxNotComplete;
    public GameObject chatBoxComplete;
    public GameObject uiTaskPanel;
    public GameObject Teleport;
    public GameObject Stop;
    public TimeManager timeManager;

    public PencilCollector pencilCollector;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (chatBoxUI != null)
        {
            chatBoxUI.SetActive(false); // Sembunyikan di awal
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(player.position, transform.position);

            if (distance <= detectionRange)
            {
                if (player.position.x > transform.position.x)
                    spriteRenderer.flipX = false;
                else
                    spriteRenderer.flipX = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PencilCollector.instance != null)
            {
                if (PencilCollector.instance.HasCompletedMission())
                {
                    chatBoxComplete.SetActive(true);
                    chatBoxUI.SetActive(false);
                    chatBoxNotComplete.SetActive(false);
                    ControllerPanel.SetActive(false);
                    returnToNPCText.SetActive(false);
                    uiTaskPanel.SetActive(false);
                    timeManager.StopTimer();
                    Debug.Log("Misi selesai, tampilkan hasil");
                }
                else if (PencilCollector.instance.IsMissionStarted())
                {
                    // Misi sudah dimulai, tapi belum selesai
                    chatBoxNotComplete.SetActive(true);
                    chatBoxUI.SetActive(false);
                    chatBoxComplete.SetActive(false);
                    ControllerPanel.SetActive(false);

                    StartCoroutine(HideChatBoxNotCompleteAfterDelay(2f));
                }
                else
                {
                    // Misi belum dimulai
                    chatBoxUI.SetActive(true);
                    chatBoxComplete.SetActive(false);
                    chatBoxNotComplete.SetActive(false);
                    ControllerPanel.SetActive(false);
                    Debug.Log("Tampilkan chat awal");
                }
            }
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

    public void StartMission()
    {
        chatBoxUI.SetActive(false);
        uiTaskPanel.SetActive(true);
        ControllerPanel.SetActive(true);
        timeManager.StartTimer();
        PencilCollector.instance.StartCollecting();
        Teleport.SetActive(true);
    }

    public void Tidak()
    {
        chatBoxUI.SetActive(false);
        chatBoxComplete.SetActive(false);
        ControllerPanel.SetActive(true);
        Teleport.SetActive(false);
    }

    public void Terimakasih()
    {
        Stop.SetActive(false);
        chatBoxUI.SetActive(false);
        chatBoxComplete.SetActive(false);
        ControllerPanel.SetActive(true);
        Teleport.SetActive(false);
    }

}
