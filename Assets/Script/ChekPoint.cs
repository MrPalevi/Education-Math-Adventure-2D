using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Sprite spriteOff; // sebelum disentuh
    public Sprite spriteOn;  // setelah disentuh

    private SpriteRenderer spriteRenderer;
    private bool activated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && spriteOff != null)
        {
            spriteRenderer.sprite = spriteOff;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            Debug.Log("Player menyentuh checkpoint");
            PlayerManager.lastCheckPointPos = transform.position;
            activated = true;

            // Ganti sprite
            if (spriteRenderer != null && spriteOn != null)
            {
                spriteRenderer.sprite = spriteOn;
            }
        }
    }
}
