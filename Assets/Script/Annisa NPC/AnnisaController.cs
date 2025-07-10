using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnnisaController : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

    public GameObject chatBoxUI;
    public GameObject ControllerPanel;

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
            if (chatBoxUI != null)
            {
                chatBoxUI.SetActive(true); // Tampilkan chat box
                ControllerPanel.SetActive(false);
                Debug.Log("Player mendekat, tampilkan chat");
            }
        }
    }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player"))
    //     {
    //         if (chatBoxUI != null)
    //         {
    //             chatBoxUI.SetActive(false); // Sembunyikan chat box
    //             ControllerPanel.SetActive(true);
    //             Debug.Log("Player menjauh, sembunyikan chat");
    //         }
    //     }
    // }

    public void Tidak()
    {
        chatBoxUI.SetActive(false);
        ControllerPanel.SetActive(true);
    }
}
