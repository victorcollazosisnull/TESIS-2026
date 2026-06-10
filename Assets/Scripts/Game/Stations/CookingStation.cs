using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CookingStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;
    [SerializeField] private ParticleSystem cookingParticles;

    private PickupObject currentObject;

    [Header("Cooking")]
    [SerializeField] private float cookTime = 3f;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject canvasUI;

    [Header("Sounds")]
    [SerializeField] private SoundData cookingSound;
    [SerializeField] private AudioSource cookingAudioSource;

    [Header("UI")]
    [SerializeField] private GameObject cookText;

    [Header("Arrow UI")]
    [SerializeField] private GameObject arrowUI;
    [Header("Arrow Animation")]
    [SerializeField] private float moveAmount = 15f;
    [SerializeField] private float moveSpeed = 0.6f;

    private float timer = 0f;
    private bool isCooking = false;

    private static readonly Ingredient.IngredientType[] acceptedTypes = new[]
    {
        Ingredient.IngredientType.PapaCut,
        Ingredient.IngredientType.CarneCut,
        Ingredient.IngredientType.Choclo,
        Ingredient.IngredientType.Camote,
    };

    private void Start()
    {
        cookText.SetActive(false);
        canvasUI.SetActive(false);

        if (cookingParticles != null)
            cookingParticles.Stop();

        if (arrowUI != null)
        {
            arrowUI.SetActive(false);

            arrowUI.transform.DOLocalMoveY(
                arrowUI.transform.localPosition.y + moveAmount,
                moveSpeed)
                .SetLoops(-1, DG.Tweening.LoopType.Yoyo)
                .SetEase(DG.Tweening.Ease.InOutSine);
        }
    }

    private void Update()
    {
        UpdateArrowUI();

        if (!isCooking) return;

        timer -= Time.deltaTime;

        float progress = timer / cookTime;
        fillImage.fillAmount = progress;
        fillImage.color = Color.Lerp(Color.red, Color.green, progress);

        if (timer <= 0f)
            FinishCooking();
    }

    private void UpdateArrowUI()
    {
        if (arrowUI == null) return;

        bool shouldShow = false;

        if (!isCooking && currentObject == null && playerHold.IsHolding())
        {
            PickupObject held = playerHold.GetHeldObject();
            if (held != null)
            {
                Ingredient ingredient = held.GetComponent<Ingredient>();
                if (ingredient != null && ingredient.CanBeCooked() && IsAccepted(ingredient.type))
                    shouldShow = true;
            }
        }

        arrowUI.SetActive(shouldShow);
    }

    private bool IsAccepted(Ingredient.IngredientType type)
    {
        foreach (var t in acceptedTypes)
            if (t == type) return true;
        return false;
    }

    public void Interact()
    {
        if (isCooking)
        {
            Debug.Log("Está cocinando...");
            return;
        }

        if (playerHold.IsHolding())
        {
            if (currentObject != null)
            {
                Debug.Log("Ya hay algo en la sartén");
                return;
            }

            PickupObject held = playerHold.GetHeldObject();
            Ingredient ingredient = held.GetComponent<Ingredient>();

            if (ingredient == null)
            {
                Debug.Log("No es ingrediente");
                return; 
            }

            if (!IsAccepted(ingredient.type))
            {
                Debug.Log("Este ingrediente no se cocina en la sartén");
                return;
            }

            if (!ingredient.CanBeCooked())
            {
                Debug.Log("No se puede cocinar");
                return;
            }

            currentObject = held;

            Rigidbody rb = held.GetComponent<Rigidbody>();
            Collider col = held.GetComponent<Collider>();

            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            if (col != null)
                col.enabled = false;

            held.Lock();
            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;
            held.transform.SetParent(placePoint);

            cookText.SetActive(true);

            Debug.Log("Ingrediente colocado en sartén");
            return;
        }

        if (currentObject != null)
        {
            isCooking = true;
            cookText.SetActive(false);
            timer = cookTime;
            fillImage.fillAmount = 0f;
            canvasUI.SetActive(true);

            if (cookingParticles != null)
                cookingParticles.Play();

            if (cookingAudioSource != null && cookingSound != null)
            {
                cookingAudioSource.clip = cookingSound.clip;
                cookingAudioSource.volume = cookingSound.volume;
                cookingAudioSource.loop = true;
                cookingAudioSource.Play();
            }

            Debug.Log("Empezando a cocinar");
        }
    }

    private void FinishCooking()
    {
        isCooking = false;

        if (cookingParticles != null)
            cookingParticles.Stop();

        if (cookingAudioSource != null)
            cookingAudioSource.Stop();

        Ingredient ingredient = currentObject.GetComponent<Ingredient>();
        if (ingredient == null) return;

        Vector3 spawnPos = placePoint.position;
        Quaternion spawnRot = placePoint.rotation;

        Destroy(currentObject.gameObject);

        PickupObject cookedObj = ingredient.GetCookedResult(spawnPos, spawnRot);

        if (cookedObj != null)
        {
            Rigidbody rb = cookedObj.GetComponent<Rigidbody>();
            Collider col = cookedObj.GetComponent<Collider>();

            if (rb != null)
            {
                if (!rb.isKinematic)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                rb.isKinematic = true;
                rb.useGravity = false;
            }

            if (col != null)
            {
                col.enabled = true;
                col.isTrigger = true;
            }

            cookedObj.transform.position = placePoint.position;
            cookedObj.transform.rotation = placePoint.rotation;
            cookedObj.transform.SetParent(placePoint);

            cookedObj.SetCanDrop(false);
            cookedObj.Unlock();
        }

        currentObject = null;
        fillImage.fillAmount = 0f;
        canvasUI.SetActive(false);

        Debug.Log("Listo");
    }
}