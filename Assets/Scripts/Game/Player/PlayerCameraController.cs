using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.5f;
    [SerializeField] private Transform playerBody; 

    private float xRotation = 0f;

    private bool hasLooked = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable() => PlayerInputs.mouseInputAction += RotateCamera;
    private void OnDisable() => PlayerInputs.mouseInputAction -= RotateCamera;

    private void RotateCamera(Vector2 mouseDelta)
    {
        float mouseX = mouseDelta.x * sensitivity;
        float mouseY = mouseDelta.y * sensitivity;

        if (!hasLooked && mouseDelta.magnitude > 0.1f)
        {
            hasLooked = true;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); 
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}