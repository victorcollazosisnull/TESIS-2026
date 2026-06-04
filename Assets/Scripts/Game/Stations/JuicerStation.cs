using DG.Tweening;
using UnityEngine;

public class JuicerStation : MonoBehaviour, IInteractable, IHighlightable, IStation
{
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;

    private PickupObject currentObject;

    [Header("Highlight")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private float intensity = 2f;
    private bool canHighlight = true;

    [Header("Sounds")]
    [SerializeField] private SoundData placeSound;
    [SerializeField] private SoundData pickupSound;

    [Header("UI")]
    [SerializeField] private GameObject helpUI;
    [Header("Arrow Animation")]
    [SerializeField] private float moveAmount = 15f;
    [SerializeField] private float moveSpeed = 0.6f;

    private void Start()
    {
        if (helpUI != null)
        {
            helpUI.SetActive(false);

            helpUI.transform.DOLocalMoveY(
            helpUI.transform.localPosition.y + moveAmount,
            moveSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
        }
    }

    private void Update()
    {
        if (helpUI == null) return;

        bool shouldShow = false;

        if (playerHold.IsHolding() && currentObject == null)
        {
            PickupObject held = playerHold.GetHeldObject();

            if (held != null)
            {
                Ingredient ingredient = held.GetComponent<Ingredient>();

                if (ingredient != null &&
                    ingredient.type == Ingredient.IngredientType.LimonCut)
                {
                    shouldShow = true;
                }
            }
        }

        helpUI.SetActive(shouldShow);
    }

    public void Interact()
    {
        if (playerHold.IsHolding())
        {
            if (currentObject != null)
                return;

            PickupObject held = playerHold.GetHeldObject();
            Ingredient ingredient = held.GetComponent<Ingredient>();

            if (ingredient == null)
                return;

            if (ingredient.type != Ingredient.IngredientType.LimonCut)
            {
                Debug.Log("Necesitas limon cortado");
                return;
            }

            currentObject = held;

            canHighlight = true;
            held.SetHighlight(false);

            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

            AudioManager.Instance.Play(placeSound);

            Debug.Log("Limon colocado");
            return;
        }

        if (currentObject != null)
        {
            if (!playerHold.HasJuicer())
            {
                Debug.Log("Necesitas exprimidor");
                return;
            }

            Ingredient ingredient = currentObject.GetComponent<Ingredient>();

            Vector3 spawnPos = placePoint.position;
            Quaternion spawnRot = placePoint.rotation;

            Destroy(currentObject.gameObject);

            PickupObject juice = ingredient.GetCookedResult(spawnPos, spawnRot);

            if (juice != null)
            {
                juice.SetCanDrop(false);
                juice.GetComponent<Collider>().enabled = true;
                juice.SetHighlight(true);
                juice.SetAssignedStation(this);
                currentObject = juice;
            }

            canHighlight = false;
            UnHighlight();

            Debug.Log("Jugo preparado");
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

    public void ClearStation()
    {
        currentObject = null;
        canHighlight = true;

        AudioManager.Instance.Play(pickupSound);
    }
}