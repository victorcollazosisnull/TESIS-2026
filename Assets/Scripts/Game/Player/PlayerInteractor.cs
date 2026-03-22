using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private Camera cam;

    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    private IHighlightable currentHighlight;

    private void OnEnable()
    {
        PlayerInputs.interactInput += TryInteract;
    }

    private void OnDisable()
    {
        PlayerInputs.interactInput -= TryInteract;
    }

    private void Update()
    {
        CheckHighlight();
    }

    private void CheckHighlight()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            IHighlightable highlightable = hit.collider.GetComponent<IHighlightable>();

            if (highlightable != null)
            {
                if (currentHighlight != highlightable)
                {
                    currentHighlight?.UnHighlight();
                    currentHighlight = highlightable;
                    currentHighlight.Highlight();
                }
                return;
            }
        }

        if (currentHighlight != null)
        {
            currentHighlight.UnHighlight();
            currentHighlight = null;
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            Debug.Log("Objeto es: " + hit.collider.name);

            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
            else
            {
                if (playerHold.IsHolding())
                {
                    playerHold.Drop();
                    Debug.Log("Soltaste el objeto");
                }
            }
        }
        else
        {
            if (playerHold.IsHolding())
            {
                playerHold.Drop();
                Debug.Log("Soltaste el objeto");
            }
        }
    }
}