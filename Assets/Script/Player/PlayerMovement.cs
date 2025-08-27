using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    // Input system
    private PlayerControls controls;
    private Action<InputAction.CallbackContext> jumpCallback;
    private float direction;

    // Movement
    [Header("Movement Settings")]
    public float speed = 200f;
    public float jumpForce = 5f;
    private bool isFacingRight = true;
    private int numberOfJump = 0;
    private bool isGrounded;

    // References
    [Header("References")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public GameObject controlPanel; // panel kontrol UI
    private Rigidbody2D playerRB;
    private Animator animator;
    private AudioManager audioManager;

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio")?.GetComponent<AudioManager>();

        controls = new PlayerControls();

        // Saat tombol arah ditekan
        controls.Land.Move.performed += ctx =>
        {
            direction = ctx.ReadValue<float>();
        };

        // Saat tombol arah dilepas atau input terputus
        controls.Land.Move.canceled += ctx =>
        {
            direction = 0f;
        };

        jumpCallback = ctx => Jump();
        controls.Land.Jump.performed += jumpCallback;
    }

    private void OnEnable()
    {
        controls?.Enable();
    }

    private void OnDisable()
    {
        controls?.Disable();
    }

    private void OnDestroy()
    {
        if (controls != null && jumpCallback != null)
        {
            controls.Land.Jump.performed -= jumpCallback;
        }
    }

    private void FixedUpdate()
    {
        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);
        animator.SetBool("isGrounded", isGrounded);

        // Kalau panel kontrol mati, hentikan gerakan
        if (controlPanel != null && !controlPanel.activeInHierarchy)
        {
            StopMovement();
            return; // hentikan update gerakan
        }

        // Movement normal
        if (playerRB != null)
        {
            playerRB.velocity = new Vector2(direction * speed * Time.fixedDeltaTime, playerRB.velocity.y);
        }

        animator.SetFloat("speed", Mathf.Abs(direction));

        // Flip karakter
        if ((isFacingRight && direction < 0) || (!isFacingRight && direction > 0))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private void Jump()
    {
        if (playerRB == null) return;

        if (isGrounded)
        {
            numberOfJump = 0;
            audioManager?.PlaySFX(audioManager.Jump);
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            numberOfJump++;
        }
        else if (numberOfJump == 1)
        {
            audioManager?.PlaySFX(audioManager.Jump);
            playerRB.velocity = new Vector2(playerRB.velocity.x, jumpForce);
            numberOfJump++;
        }
    }

    // Fungsi berhenti gerakan
    private void StopMovement()
    {
        direction = 0f;
        if (playerRB != null)
        {
            playerRB.velocity = new Vector2(0, playerRB.velocity.y);
        }
        animator.SetFloat("speed", 0f);
    }
}
