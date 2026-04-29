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

    private float timer = 0f;
    private bool isCooking = false;
    private bool hasRice = false;

    private void Start()
    {
        canvasUI.SetActive(false);
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
            timer = cookTime;

            fillImage.fillAmount = 1f;
            canvasUI.SetActive(true);

            Debug.Log("Cocinando arroz...");
            return;
        }
    }

    private void Update()
    {
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