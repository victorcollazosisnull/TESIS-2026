using UnityEngine;

public class IngredientSpawner : MonoBehaviour, IInteractable
{
    [Header("Config")]
    [SerializeField] private PickupObject ingredientPrefab;

    [Header("References")]
    [SerializeField] private PlayerHold playerHold;

    public void Interact()
    {
        if (playerHold.IsHolding())
        {
            Debug.Log("Ya tienes un objeto en la mano");
            return;
        }

        PickupObject newIngredient = Instantiate(ingredientPrefab);

        playerHold.PickUp(newIngredient);

        Debug.Log("Agarraste: " + newIngredient.name);
    }
}