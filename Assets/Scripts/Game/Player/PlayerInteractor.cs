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
        if (currentHighlight != null && (currentHighlight as MonoBehaviour) == null)
        {
            currentHighlight = null;
        }

        Ray ray = new Ray(cam.transform.position, cam.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactLayer))
        {
            IHighlightable highlightable = hit.collider.GetComponent<IHighlightable>();

            if (highlightable != null)
            {
                if (currentHighlight != highlightable)
                {
                    if (currentHighlight != null && (currentHighlight as MonoBehaviour) != null)
                    {
                        currentHighlight.UnHighlight();
                    }

                    currentHighlight = highlightable;
                    currentHighlight.Highlight();
                }
                return;
            }
        }

        if (currentHighlight != null)
        {
            if ((currentHighlight as MonoBehaviour) != null)
            {
                currentHighlight.UnHighlight();
            }

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
                    TryDrop();
                    Debug.Log("Soltaste el objeto");
                }
            }
        }
        else
        {
            if (playerHold.IsHolding())
            {
                TryDrop();
                Debug.Log("Soltaste el objeto");
            }
        }
    }

    private void TryDrop()
    {
        if (!playerHold.IsHolding()) return;

        PickupObject held = playerHold.GetHeldObject();

        if (held != null)
        {
            if (!held.CanDrop())
            {
                Debug.Log("Este objeto no se puede soltar");
                return;
            }
        }

        playerHold.Drop();
        Debug.Log("Soltaste el objeto");
    }

}