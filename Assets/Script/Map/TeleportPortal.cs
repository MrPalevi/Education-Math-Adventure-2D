using System.Collections;
using UnityEngine;

public class TeleportBidirectional : MonoBehaviour
{
    public enum TeleportMode
    {
        OneWay,
        TwoWay
    }

    [Header("Titik Teleport")]
    public Transform targetPortal;

    [Header("Mode Teleport")]
    public TeleportMode teleportMode = TeleportMode.TwoWay;

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

        canTeleport = false;

        // Hanya nonaktifkan teleport target jika mode dua arah
        TeleportBidirectional otherPortal = targetPortal.GetComponent<TeleportBidirectional>();
        if (teleportMode == TeleportMode.TwoWay && otherPortal != null)
        {
            otherPortal.canTeleport = false;
        }

        // Pindahkan player ke posisi target
        player.transform.position = targetPortal.position;

        yield return new WaitForSeconds(teleportCooldown);

        canTeleport = true;

        if (teleportMode == TeleportMode.TwoWay && otherPortal != null)
        {
            otherPortal.canTeleport = true;
        }
    }
}
