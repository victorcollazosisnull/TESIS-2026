using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupObject : MonoBehaviour, IInteractable, IHighlightable
{
    private Rigidbody rb;
    private Collider col;

    [Header("Settings Pickup")]
    private bool isLocked = false;
    [SerializeField] private bool canDrop = true;

    [Header("Highlight")]
    [SerializeField] private Material normalMat;
    [SerializeField] private Material highlightMat;
    [SerializeField] private Color highlightColor = Color.white;
    [SerializeField] private float intensity = 2f;
    private Material[] originalMats;

    private Renderer rend;
    private Color[] originalColors;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        rend = GetComponentInChildren<Renderer>();

        originalMats = rend.materials;

        originalColors = new Color[rend.materials.Length];

        for (int i = 0; i < rend.materials.Length; i++)
        {
            if (rend.materials[i].HasProperty("_Color"))
            {
                originalColors[i] = rend.materials[i].color;
            }
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

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    public void SetCanDrop(bool value)
    {
        canDrop = value;
    }

    public bool CanDrop()
    {
        return canDrop;
    }

    public void Interact()
    {
        if (isLocked) return;

        PlayerHold playerHold = FindAnyObjectByType<PlayerHold>();

        if (!playerHold.IsHolding())
        {
            playerHold.PickUp(this);
        }
    }

    public void Highlight()
    {
        if (rend == null) return;

        foreach (Material mat in rend.materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", highlightColor * intensity);
            }
        }
    }

    public void UnHighlight()
    {
        if (rend == null) return;

        foreach (Material mat in rend.materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}