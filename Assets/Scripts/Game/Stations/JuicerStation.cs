using DG.Tweening;
using UnityEngine;

public class JuicerStation : MonoBehaviour, IInteractable, IStation
{
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;

    private PickupObject currentObject;

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

            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

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

            Ingredient ingredient =
                currentObject.GetComponent<Ingredient>();

            Vector3 spawnPos = placePoint.position;
            Quaternion spawnRot = placePoint.rotation;

            Destroy(currentObject.gameObject);

            PickupObject juice =
                ingredient.GetCookedResult(spawnPos, spawnRot);

            if (juice != null)
            {
                juice.SetCanDrop(false);
                juice.GetComponent<Collider>().enabled = true;

                juice.SetAssignedStation(this);

                currentObject = juice;
            }

            Debug.Log("Jugo preparado");
        }
    }
    public void ClearStation()
    {
        currentObject = null;
    }
}