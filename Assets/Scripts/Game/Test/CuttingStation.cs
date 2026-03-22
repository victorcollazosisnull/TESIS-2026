using UnityEngine;

public class CuttingStation : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;

    public void Interact()
    {
        if (!playerHold.IsHolding()) return;

        PickupObject held = playerHold.GetHeldObject();

        Ingredient ingredient = held.GetComponent<Ingredient>();

        if (ingredient == null)
        {
            Debug.Log("No es un ingrediente");
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