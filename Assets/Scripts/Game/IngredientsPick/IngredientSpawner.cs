using UnityEngine;

public class IngredientSpawner : MonoBehaviour, IInteractable
{
    [Header("Config")]
    [SerializeField] private PickupObject ingredientPrefab;

    [Header("References")]
    [SerializeField] private PlayerHold playerHold;

    public void Interact()
    {
        if (playerHold.IsHolding() || playerHold.HasKnife())
        {
            Debug.Log("Manos ocupadas");
            return;
        }

        PickupObject newIngredient = Instantiate(ingredientPrefab);

        playerHold.PickUp(newIngredient);

        Debug.Log("Agarraste: " + newIngredient.name);
    }
}