using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartLift : MonoBehaviour
{
    public enum MovementDirection
    {
        UpToDown,
        DownToUp,
        LeftToRight,
        RightToLeft
    }

    [Header("Pengaturan Gerakan Lift")]
    public MovementDirection direction = MovementDirection.UpToDown;
    public float speed = 1f;
    public float range = 3f;
    public bool playerDetection = false;

    private Vector2 startPos;
    private Vector2 moveDir;
    private bool isPlayerOnLift = false;
    private int dirMultiplier = 1;

    void Start()
    {
        startPos = transform.position;
        moveDir = GetMoveDirection();
    }

    void FixedUpdate()
    {
        if (!playerDetection || (playerDetection && isPlayerOnLift))
        {
            transform.Translate(moveDir * speed * Time.deltaTime * dirMultiplier);

            float distanceMoved = Vector2.Distance(startPos, transform.position);
            if (distanceMoved >= range)
            {
                dirMultiplier *= -1;
                startPos = transform.position; // reset posisi awal untuk jarak selanjutnya
            }
        }
    }

    Vector2 GetMoveDirection()
    {
        switch (direction)
        {
            case MovementDirection.UpToDown: return Vector2.down;
            case MovementDirection.DownToUp: return Vector2.up;
            case MovementDirection.LeftToRight: return Vector2.right;
            case MovementDirection.RightToLeft: return Vector2.left;
            default: return Vector2.up;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerDetection && collision.CompareTag("Player"))
        {
            isPlayerOnLift = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerDetection && collision.CompareTag("Player"))
        {
            isPlayerOnLift = false;
        }
    }
}
