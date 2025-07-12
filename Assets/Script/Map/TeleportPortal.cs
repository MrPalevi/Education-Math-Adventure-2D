using System.Collections;
using UnityEngine;

public class TeleportBidirectional : MonoBehaviour
{
    [Header("Titik Teleport")]
    public Transform targetPortal; // Portal tujuan (bisa saling di-link)
    public bool isEntrancePortal = true; // Menandai ini sebagai portal masuk atau keluar

    [Header("Pengaturan Teleport")]
    public float teleportCooldown = 1f;

    private bool canTeleport = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canTeleport) return;

        if (collision.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(collision.gameObject));
        }
    }

    IEnumerator TeleportPlayer(GameObject player)
    {
        if (targetPortal == null)
        {
            Debug.LogWarning("Target portal belum di-set!");
            yield break;
        }

        // Nonaktifkan sementara teleport di kedua portal
        TeleportBidirectional otherPortal = targetPortal.GetComponent<TeleportBidirectional>();
        if (otherPortal != null)
        {
            otherPortal.canTeleport = false;
        }

        canTeleport = false;

        // Pindahkan player ke posisi target
        player.transform.position = targetPortal.position;

        yield return new WaitForSeconds(teleportCooldown);

        // Aktifkan kembali teleport di kedua portal
        canTeleport = true;
        if (otherPortal != null)
        {
            otherPortal.canTeleport = true;
        }
    }
}
