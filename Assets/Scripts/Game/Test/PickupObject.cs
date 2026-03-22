using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupObject : MonoBehaviour, IInteractable, IHighlightable
{
    private Rigidbody rb;
    private Collider col;

    [Header("Highlight")]
    [SerializeField] private Material normalMat;
    [SerializeField] private Material highlightMat;

    private Renderer rend;
    private Color[] originalColors;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rend = GetComponent<Renderer>();

        originalColors = new Color[rend.materials.Length];

        for (int i = 0; i < rend.materials.Length; i++)
        {
            originalColors[i] = rend.materials[i].color;
        }
    }

    public void OnPickUp()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;
    }

    public void OnDrop()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
    }

    public void Interact()
    {
        PlayerHold playerHold = FindAnyObjectByType<PlayerHold>();

        if (!playerHold.IsHolding())
        {
            playerHold.PickUp(this);
        }
    }

    public void Highlight()
    {
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].color = originalColors[i] * 0.6f; 
        }
    }

    public void UnHighlight()
    {
        for (int i = 0; i < rend.materials.Length; i++)
        {
            rend.materials[i].color = originalColors[i];
        }
    }
}