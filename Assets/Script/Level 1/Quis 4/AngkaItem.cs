using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngkaItem : MonoBehaviour
{
    public int angkaID; // ID unik untuk setiap angka

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AngkaCollector.instance.ShowCollectButton(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AngkaCollector.instance.HideCollectButton();
        }
    }
}
