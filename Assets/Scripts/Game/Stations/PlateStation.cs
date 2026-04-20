using System.Collections.Generic;
using UnityEngine;
using static Ingredient;

public class PlateStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform[] ingredientPoints;

    [Header("Ingredients Finals")]
    [SerializeField] private List<IngredientType> validIngredients;

    private List<IngredientType> ingredients = new List<IngredientType>();
    private int currentIndex = 0;

    public void Interact()
    {
        if (!playerHold.IsHolding()) return;

        PickupObject held = playerHold.GetHeldObject();
        Ingredient ingredient = held.GetComponent<Ingredient>();

        if (ingredient == null) return;

        if (!ingredient.IsReadyForPlate())
        {
            Debug.Log("Ingrediente no válido para plato");
            return;
        }

        if (!validIngredients.Contains(ingredient.type))
        {
            Debug.Log("Ingrediente no válido para plato");
            return;
        }

        if (ingredients.Contains(ingredient.type))
        {
            Debug.Log("Ingrediente repetido");
            return;
        }

        if (currentIndex >= ingredientPoints.Length)
        {
            Debug.Log("Plato lleno");
            return;
        }

        Transform point = ingredientPoints[currentIndex];

        if (ingredient.plateVisualPrefab != null)
        {
            Instantiate(
                ingredient.plateVisualPrefab,
                point.position,
                point.rotation,
                point
            );
        }

        Destroy(held.gameObject);

        ingredients.Add(ingredient.type);
        currentIndex++;

        playerHold.Drop();


        Debug.Log("Ingrediente agregado al plato (visual)");
    }
}