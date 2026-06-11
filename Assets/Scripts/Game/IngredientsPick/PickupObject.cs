using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupObject : MonoBehaviour, IInteractable, IHighlightable
{
    private Rigidbody rb;
    private Collider col;

    [Header("Settings Pickup")]
    private bool isLocked = false;
    [SerializeField] private bool canDrop = true;
    private bool canHighlight = true;

    [Header("Collider en olla (para poder sacarlo)")]
    [SerializeField] private bool usePickupColliderSize = false;
    [SerializeField] private Vector3 inStationSize = Vector3.one;   // tamaño grande (en olla)
    [SerializeField] private Vector3 inHandSize = new Vector3(0.15f, 0.15f, 0.15f); // tamaño pequeño (en mano/mesa)

    [Header("Highlight")]
    [SerializeField] private Material normalMat;
    [SerializeField] private Material highlightMat;
    [SerializeField] private Color highlightColor = Color.white;
    [SerializeField] private float intensity = 2f;
    private Material[] originalMats;

    private Renderer rend;
    private Color[] originalColors;

    private IStation assignedStation;

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
                originalColors[i] = rend.materials[i].color;
        }
    }

    public void SetAssignedStation(IStation station)
    {
        assignedStation = station;
    }

    public void OnPickUp()
    {
        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;

        // Al agarrarlo achicamos el collider para que no tape la mesa de picar
        if (usePickupColliderSize)
            SetColliderSize(inHandSize);
    }

    public void OnDrop()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        col.enabled = true;
    }

    // Llamar desde CookingStation cuando el objeto cocinado aparece en la olla
    public void SetStationColliderSize()
    {
        if (usePickupColliderSize)
            SetColliderSize(inStationSize);
    }

    private void SetColliderSize(Vector3 size)
    {
        if (col is BoxCollider box)
            box.size = size;
        else if (col is SphereCollider sphere)
            sphere.radius = size.x;
        else if (col is CapsuleCollider capsule)
        {
            capsule.radius = size.x;
            capsule.height = size.y;
        }
    }

    public void Lock() { isLocked = true; }
    public void Unlock() { isLocked = false; }
    public void SetCanDrop(bool value) { canDrop = value; }
    public bool CanDrop() { return canDrop; }

    public void Interact()
    {
        if (isLocked) return;

        PlayerHold playerHold = FindAnyObjectByType<PlayerHold>();

        if (!playerHold.IsHolding() && !playerHold.HasAnyTool())
        {
            playerHold.PickUp(this);

            if (assignedStation != null)
            {
                assignedStation.ClearStation();
                assignedStation = null;
            }
        }
    }

    public void Highlight()
    {
        if (!canHighlight) return;
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
                mat.SetColor("_EmissionColor", Color.black);
        }
    }

    public void SetHighlight(bool value)
    {
        canHighlight = value;
        if (!value) UnHighlight();
    }
}