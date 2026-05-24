using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RiceCookingStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;

    [Header("Cooking")]
    [SerializeField] private float cookTime = 3f;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject canvasUI;

    [Header("Rice Result")]
    [SerializeField] private PickupObject riceCookedPrefab;
    private bool isCooked = false;

    [Header("UI")]
    [SerializeField] private GameObject cookText;
    [SerializeField] private GameObject helpUI;
    [Header("Arrow Animation")]
    [SerializeField] private float moveAmount = 15f;
    [SerializeField] private float moveSpeed = 0.6f;
    private bool uiVisible = false;

    private float timer = 0f;
    private bool isCooking = false;
    private bool hasRice = false;

    private void Start()
    {
        cookText.SetActive(false);
        canvasUI.SetActive(false);

        helpUI.SetActive(false);

        helpUI.transform.DOLocalMoveY(
        helpUI.transform.localPosition.y + moveAmount,
        moveSpeed)
        .SetLoops(-1, LoopType.Yoyo)
        .SetEase(Ease.InOutSine);
    }

    public void Interact()
    {
        if (isCooking)
        {
            Debug.Log("Está cocinando arroz...");
            return;
        }

        if (playerHold.IsHolding())
        {
            if (hasRice)
            {
                Debug.Log("Ya hay arroz en la olla");
                return;
            }

            PickupObject held = playerHold.GetHeldObject();
            Ingredient ingredient = held.GetComponent<Ingredient>();

            if (ingredient == null)
            {
                Debug.Log("No es ingrediente");
                return;
            }

            if (ingredient.type != Ingredient.IngredientType.Rice)
            {
                Debug.Log("Solo puedes poner arroz crudo");
                return;
            }

            hasRice = true;
            isCooked = false; 

            Destroy(held.gameObject);
            playerHold.Drop();
            cookText.SetActive(true);

            Debug.Log("Arroz colocado en la olla");
            return;
        }

        if (hasRice && isCooked)
        {
            PickupObject rice = Instantiate(riceCookedPrefab);

            playerHold.PickUp(rice);

            hasRice = false;
            isCooked = false;

            Debug.Log("Recogiste arroz cocido");
            return;
        }

        if (hasRice && !isCooking && !isCooked)
        {
            isCooking = true;
            cookText.SetActive(false);
            timer = cookTime;

            fillImage.fillAmount = 1f;
            canvasUI.SetActive(true);

            Debug.Log("Cocinando arroz...");
            return;
        }
    }

    private void Update()
    {
        if (!hasRice)
        {
            bool shouldShow = false;

            if (playerHold.IsHolding())
            {
                PickupObject held = playerHold.GetHeldObject();

                if (held != null)
                {
                    Ingredient ingredient = held.GetComponent<Ingredient>();

                    if (ingredient != null &&
                        ingredient.type == Ingredient.IngredientType.Rice)
                    {
                        shouldShow = true;
                    }
                }
            }

            if (shouldShow != uiVisible)
            {
                uiVisible = shouldShow;
                helpUI.SetActive(shouldShow);
            }
        }
        else
        {
            if (uiVisible)
            {
                uiVisible = false;
                helpUI.SetActive(false);
            }
        }

        if (!isCooking) return;

        timer -= Time.deltaTime;

        float progress = timer / cookTime;
        fillImage.fillAmount = progress;

        fillImage.color = Color.Lerp(Color.green, Color.red, 1f - progress);

        if (timer <= 0f)
        {
            FinishCooking();
        }
    }

    private void FinishCooking()
    {
        isCooking = false;
        isCooked = true;

        fillImage.fillAmount = 0f;
        canvasUI.SetActive(false);

        Debug.Log("Arroz listo");
    }
}