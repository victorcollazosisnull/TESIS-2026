using UnityEngine;

public class CuttingStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint; 

    private PickupObject currentObject;

    public void Interact()
    {
        if (playerHold.IsHolding())
        {
            if (currentObject != null)
            {
                Debug.Log("Ya hay algo en la mesa");
                return;
            }

            PickupObject held = playerHold.GetHeldObject();

            currentObject = held;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

            Debug.Log("Objeto colocado en mesa");
            return;
        }

        if (currentObject != null)
        {
            if (!playerHold.HasKnife())
            {
                Debug.Log("Necesitas cuchillo");
                return;
            }

            Ingredient ingredient = currentObject.GetComponent<Ingredient>();

            if (ingredient == null)
            {
                Debug.Log("No es ingrediente");
                return;
            }

            if (ingredient.isCut)
            {
                Debug.Log("Ya está cortado");
                return;
            }

            ingredient.isCut = true;

            Debug.Log("Ingrediente cortado");
        }
    }
}