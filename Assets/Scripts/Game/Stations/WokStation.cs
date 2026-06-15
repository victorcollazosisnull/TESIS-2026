using DG.Tweening;
using UnityEngine;

public class WokStation : MonoBehaviour, IInteractable, IHighlightable, IStation
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform[] placePonts; // 3 puntos visuales donde aparecen los items
    [SerializeField] private Transform resultPoint;  // punto donde aparece el prefab resultado

    [Header("Item Visual Scale")]
    [SerializeField] private Vector3 itemScaleInWok = new Vector3(0.5f, 0.5f, 0.5f); // ajusta a tu gusto en Inspector

    [Header("Cooking")]
    [SerializeField] private float cookTime = 4f;

    [Header("Result")]
    [SerializeField] private PickupObject lomoSalteadoPrefab;

    [Header("Highlight")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float intensity = 2f;
    private bool canHighlight = true;

    [Header("Sounds")]
    [SerializeField] private SoundData placeSound;
    [SerializeField] private SoundData cookingSound;
    [SerializeField] private AudioSource cookingAudioSource;

    [Header("Cooking UI")]
    [SerializeField] private UnityEngine.UI.Image fillImage;
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private ParticleSystem cookingParticles;

    [Header("Cook Text")]
    [SerializeField] private GameObject cookText; // "Presiona E para cocinar"

    [Header("Arrow UI")]
    [SerializeField] private GameObject arrowUI;
    [Header("Arrow Animation")]
    [SerializeField] private float moveAmount = 15f;
    [SerializeField] private float moveSpeed = 0.6f;

    // Los 3 ingredientes requeridos
    private static readonly Ingredient.IngredientType[] requiredTypes = new[]
    {
        Ingredient.IngredientType.CarneCut,
        Ingredient.IngredientType.CebollaCut,
        Ingredient.IngredientType.TomateCut,
    };

    private PickupObject[] slots; // slot por cada ingrediente requerido
    private Vector3[] originalScales; // escala original de cada item antes de entrar al wok
    private bool isCooking = false;
    private bool hasResult = false;
    private PickupObject resultObject;
    private float timer = 0f;

    private bool vinegarAdded = false;
    private bool soySauceAdded = false;

    private void Start()
    {

        slots = new PickupObject[requiredTypes.Length];
        originalScales = new Vector3[requiredTypes.Length];

        if (canvasUI != null) canvasUI.SetActive(false);
        if (cookText != null) cookText.SetActive(false);
        if (cookingParticles != null) cookingParticles.Stop();

        if (arrowUI != null)
        {
            arrowUI.SetActive(false);
            arrowUI.transform.DOLocalMoveY(
                arrowUI.transform.localPosition.y + moveAmount, moveSpeed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }

    private void Update()
    {
        UpdateArrowUI();

        if (!isCooking) return;

        timer -= Time.deltaTime;
        float progress = timer / cookTime;

        if (fillImage != null)
        {
            fillImage.fillAmount = progress;
            fillImage.color = Color.Lerp(Color.red, Color.green, progress);
        }

        if (timer <= 0f)
            FinishCooking();
    }

    private void UpdateArrowUI()
    {
        if (arrowUI == null) return;

        bool shouldShow = false;

        // Ingredientes normales
        if (!isCooking && !hasResult && !IsWokFull() && playerHold.IsHolding())
        {
            PickupObject held = playerHold.GetHeldObject();

            if (held != null)
            {
                Ingredient ing = held.GetComponent<Ingredient>();

                if (ing != null &&
                    IsRequired(ing.type) &&
                    !AlreadyHasType(ing.type))
                {
                    shouldShow = true;
                }
            }
        }

        // Vinagre y sillao
        if (!isCooking && !hasResult && IsWokFull())
        {
            if ((playerHold.HasVinegar() && !vinegarAdded) ||
                (playerHold.HasSoySauce() && !soySauceAdded))
            {
                shouldShow = true;
            }
        }

        arrowUI.SetActive(shouldShow);
    }

    public void Interact()
    {
        if (hasResult && resultObject != null)
        {
            if (!playerHold.IsHolding() && !playerHold.HasAnyTool())
            {
                playerHold.PickUp(resultObject);
                ClearStation();
            }
            else
            {
                GameFeedbackUI.Instance?.Show("Necesitas tener las manos libres para recoger");
            }
            return;
        }

        if (isCooking)
        {
            GameFeedbackUI.Instance?.Show("El wok ya está cocinando...");
            return;
        }

        if (IsWokFull())
        {
            if (playerHold.HasVinegar() && !vinegarAdded)
            {
                vinegarAdded = true;

                GameFeedbackUI.Instance?.Show("Vinagre agregado");

                if (CanCook() && cookText != null)
                    cookText.SetActive(true);

                return;
            }

            if (playerHold.HasSoySauce() && !soySauceAdded)
            {
                soySauceAdded = true;

                GameFeedbackUI.Instance?.Show("Sillao agregado");

                if (CanCook() && cookText != null)
                    cookText.SetActive(true);

                return;
            }
        }

        if (playerHold.IsHolding())
        {

            PickupObject held = playerHold.GetHeldObject();
            Ingredient ing = held.GetComponent<Ingredient>();

            if (ing == null || !IsRequired(ing.type))
            {
                GameFeedbackUI.Instance?.Show("Este ingrediente no va en el wok");
                return;
            }

            if (AlreadyHasType(ing.type))
            {
                GameFeedbackUI.Instance?.Show("Ese ingrediente ya esta en el wok");
                return;
            }

            int slotIndex = GetSlotIndex(ing.type);
            slots[slotIndex] = held;
            held.SetAssignedStation(this);
            held.Lock();
            held.GetComponent<Collider>().enabled = false;
            held.SetHighlight(false);
            playerHold.Drop();

            Rigidbody heldRb = held.GetComponent<Rigidbody>();
            if (heldRb != null)
            {
                heldRb.isKinematic = true;
                heldRb.useGravity = false;
            }

            if (placePonts != null && slotIndex < placePonts.Length)
            {
                held.transform.SetParent(placePonts[slotIndex]);
                held.transform.localPosition = Vector3.zero;
                held.transform.localRotation = Quaternion.identity;

                held.transform.localScale = new Vector3(
                    itemScaleInWok.x / held.transform.parent.lossyScale.x,
                    itemScaleInWok.y / held.transform.parent.lossyScale.y,
                    itemScaleInWok.z / held.transform.parent.lossyScale.z
                );
            }

            AudioManager.Instance?.Play(placeSound);
            return;
        }

        if (CanCook())
        {
            StartCooking();
            return;
        }

        if (IsWokFull())
        {
            GameFeedbackUI.Instance?.Show(
                "Falta agregar vinagre y sillao");
            return;
        }

        if (GetFilledSlots() == 0)
            GameFeedbackUI.Instance?.Show("Pon carne, cebolla y tomate picados en el wok");
        else
            GameFeedbackUI.Instance?.Show($"Faltan mas ingredientes mas");
    }

    private void StartCooking()
    {
        isCooking = true;

        if (cookText != null) cookText.SetActive(false);
        if (canvasUI != null) canvasUI.SetActive(true);
        if (cookingParticles != null) cookingParticles.Play();

        if (cookingAudioSource != null && cookingSound != null)
        {
            cookingAudioSource.clip = cookingSound.clip;
            cookingAudioSource.volume = cookingSound.volume;
            cookingAudioSource.loop = true;
            cookingAudioSource.Play();
        }

        timer = cookTime;

        foreach (var slot in slots)
            if (slot != null) Destroy(slot.gameObject);
    }

    private void FinishCooking()
    {
        isCooking = false;

        if (cookingParticles != null) cookingParticles.Stop();
        if (cookingAudioSource != null) cookingAudioSource.Stop();
        if (canvasUI != null) canvasUI.SetActive(false);

        if (fillImage != null) fillImage.fillAmount = 0f;

        if (lomoSalteadoPrefab != null)
        {
            Vector3 spawnPos = resultPoint != null ? resultPoint.position : transform.position;
            Quaternion spawnRot = resultPoint != null ? resultPoint.rotation : Quaternion.identity;

            resultObject = Instantiate(lomoSalteadoPrefab, spawnPos, spawnRot);

            Rigidbody resultRb = resultObject.GetComponent<Rigidbody>();
            if (resultRb != null)
            {
                resultRb.isKinematic = true;
                resultRb.useGravity = false;
            }

            resultObject.transform.position = spawnPos;
            resultObject.transform.rotation = spawnRot;

            resultObject.SetCanDrop(false);
            resultObject.SetAssignedStation(this);
            resultObject.SetHighlight(true);

            Collider col = resultObject.GetComponent<Collider>();
            if (col != null) { col.enabled = true; col.isTrigger = true; }

            hasResult = true;
            canHighlight = false;
            UnHighlight();
        }
    }

    public void ClearStation()
    {
        foreach (var slot in slots)
            if (slot != null) Destroy(slot.gameObject);

        slots = new PickupObject[requiredTypes.Length];
        originalScales = new Vector3[requiredTypes.Length];

        resultObject = null;
        hasResult = false;
        isCooking = false;

        vinegarAdded = false;
        soySauceAdded = false;


        canHighlight = true;


        if (cookText != null) cookText.SetActive(false);
        if (canvasUI != null) canvasUI.SetActive(false);
        if (cookingParticles != null) cookingParticles.Stop();
    }

    // ── Helpers ──────────────────────────────────────────

    private bool IsRequired(Ingredient.IngredientType type)
    {
        foreach (var t in requiredTypes)
            if (t == type) return true;
        return false;
    }

    private bool AlreadyHasType(Ingredient.IngredientType type)
    {
        int index = GetSlotIndex(type);
        return slots[index] != null;
    }

    private int GetSlotIndex(Ingredient.IngredientType type)
    {
        for (int i = 0; i < requiredTypes.Length; i++)
            if (requiredTypes[i] == type) return i;
        return 0;
    }

    private bool IsWokFull()
    {
        foreach (var slot in slots)
            if (slot == null) return false;
        return true;
    }

    private int GetFilledSlots()
    {
        int count = 0;
        foreach (var slot in slots)
            if (slot != null) count++;
        return count;
    }

    // ── Highlight ────────────────────────────────────────

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

    private bool CanCook()
    {
        return IsWokFull()
            && vinegarAdded
            && soySauceAdded;
    }
}