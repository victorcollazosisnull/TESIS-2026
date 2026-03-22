using UnityEngine;

public class PlayerHeadBob : MonoBehaviour
{
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private float bobSpeed = 10f; 
    [SerializeField] private float bobAmount = 0.05f;

    private float timer = 0f;
    private Vector3 originalPos;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    private void Update()
    {
        if (movement.isMoving)
        {
            timer += Time.deltaTime * bobSpeed;
            float bobY = Mathf.Sin(timer) * bobAmount;

            float bobX = Mathf.Cos(timer / 2) * (bobAmount * 0.5f);

            transform.localPosition = originalPos + new Vector3(bobX, bobY, 0);
        }
        else
        {
            timer = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPos, Time.deltaTime * 8f);
        }
    }
}