using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Camera cam;

    [Header("References")]
    [SerializeField] private PlayerHold playerHold;

    private void OnEnable()
    {
        PlayerInputs.interactInput += TryInteract;
    }

    private void OnDisable()
    {
        PlayerInputs.interactInput -= TryInteract;
    }

    private void TryInteract()
    {
        if (playerHold.IsHolding())
        {
            playerHold.Drop();
            return;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            Debug.Log("Objeto es: " + hit.collider.name);

            PickupObject pickup = hit.collider.GetComponent<PickupObject>();

            if (pickup != null)
            {
                playerHold.PickUp(pickup);
            }
            else
            {
                Debug.Log("no tiene el script PickupObject");
            }
        }
        else
        {
            Debug.Log("no hay nada");
        }
    }
}