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

            if (!ingredient.CanBeCooked())
            {
                Debug.Log("No se puede cocinar");
                return;
            }

            currentObject = held;

            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

            if (TutorialManager.Instance?.GetStep() == 7)
            {
                TutorialManager.Instance.NextStep();
            }

            Debug.Log("Ingrediente colocado en sartén");
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

        Vector3 spawnPos = placePoint.position;
        Quaternion spawnRot = placePoint.rotation;

        Destroy(currentObject.gameObject);

        PickupObject cookedObj = ingredient.GetCookedResult(spawnPos, spawnRot);

        if (cookedObj != null)
        {
            cookedObj.SetCanDrop(false);
            cookedObj.GetComponent<Collider>().enabled = true;
            cookedObj.Unlock();
        }

        if (TutorialManager.Instance?.GetStep() == 8)
        {
            TutorialManager.Instance.NextStep();
        }

        currentObject = null;

        fillImage.fillAmount = 0f;
        canvasUI.SetActive(false);

        Debug.Log("Listo");
    }
}