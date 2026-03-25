using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;

    private PickupObject currentObject;

    [Header("Cooking")]
    [SerializeField] private float cookTime = 3f;
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject canvasUI;

    [SerializeField] private List<Ingredient.IngredientType> validIngredients;

    private float timer = 0f;
    private bool isCooking = false;

    private void Start()
    {
        canvasUI.SetActive(false);
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

            if (!validIngredients.Contains(ingredient.type))
            {
                Debug.Log("Ingrediente no válido para cocinar");
                return;
            }

            currentObject = held;

            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

            Debug.Log("Carne colocada en sartén");
            return;
        }

        if (currentObject != null)
        {
            isCooking = true;
            timer = cookTime;

            fillImage.fillAmount = 0f;
            canvasUI.SetActive(true);

            Debug.Log("Empezando a cocinar");
        }
    }

    private void Update()
    {
        if (!isCooking) return;

        timer -= Time.deltaTime;

        float progress = timer / cookTime;
        fillImage.fillAmount = progress;

        fillImage.color = Color.Lerp(Color.red, Color.green, progress);

        if (timer <= 0f)
        {
            FinishCooking();
        }
    }

    private void FinishCooking()
    {
        isCooking = false;

        Ingredient ingredient = currentObject.GetComponent<Ingredient>();

        if (ingredient == null) return;

        ingredient.isCooked = true;

        Vector3 spawnPos = placePoint.position;
        Quaternion spawnRot = placePoint.rotation;

        PickupObject cookedPrefab = ingredient.cookedPrefab;

        Destroy(currentObject.gameObject);

        if (cookedPrefab != null)
        {
            PickupObject cookedObj = Instantiate(cookedPrefab, spawnPos, spawnRot);

            cookedObj.SetCanDrop(false);
            cookedObj.GetComponent<Collider>().enabled = true;

            currentObject = null;
        }

        fillImage.fillAmount = 0f;
        canvasUI.SetActive(false);

        Debug.Log("Listo");
    }
}