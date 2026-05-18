using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private Rigidbody rb;

    [Header("Player Move Values")]
    [SerializeField] private float speed = 5f;
    public bool isMoving = false;
    private Vector2 move = Vector2.zero;

    private bool hasMoved = false;
    public bool canControl = true;

    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepsSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        PlayerInputs.moveInput += PlayerMove;
    }

    private void OnDisable()
    {
        PlayerInputs.moveInput -= PlayerMove;
    }

    private void PlayerMove(Vector2 input)
    {
        if (!canControl) return;

        move = input;
        isMoving = input.magnitude > 0.1f;

        if (isMoving && !hasMoved)
        {
            hasMoved = true;

        }
    }

    private void FixedUpdate()
    {
        if (!canControl)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

            if (footstepsSource != null && footstepsSource.isPlaying)
            {
                footstepsSource.Pause();
            }

            return;
        }

        Vector3 moveDir = transform.forward * move.y + transform.right * move.x;
        Vector3 velocity = moveDir * speed;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        if (isMoving)
        {
            if (footstepsSource != null && !footstepsSource.isPlaying)
            {
                footstepsSource.Play();
            }
        }
        else
        {
            if (footstepsSource != null && footstepsSource.isPlaying)
            {
                footstepsSource.Pause();
            }
        }
    }
}
