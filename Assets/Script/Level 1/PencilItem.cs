using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PencilItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PencilCollector.instance.ShowCollectButton(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PencilCollector.instance.HideCollectButton();
        }
    }
}
