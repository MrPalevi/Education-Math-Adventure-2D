using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaAsepController : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer spriteRenderer;
    public float detectionRange = 5f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
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
}
