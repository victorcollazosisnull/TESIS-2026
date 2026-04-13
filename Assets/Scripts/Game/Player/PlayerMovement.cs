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
        if (!TutorialFlags.canMove) return;

        move = input;
        isMoving = input.magnitude > 0.1f;

        if (isMoving && !hasMoved)
        {
            hasMoved = true;

            if (TutorialManager.Instance.GetStep() == 1)
            {
                TutorialManager.Instance.NextStep();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 moveDir = transform.forward * move.y + transform.right * move.x;

        Vector3 velocity = moveDir * speed;

        rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
    }
}
