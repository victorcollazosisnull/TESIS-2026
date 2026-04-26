using System.Collections.Generic;
using UnityEngine;
using static Ingredient;

public class PlateStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform[] ingredientPoints;
    [SerializeField] private GameObject finalDishObject;

    [Header("Ingredients Finals")]
    [SerializeField] private List<IngredientType> validIngredients;

    private List<GameObject> spawnedVisuals = new List<GameObject>();
    private List<IngredientType> ingredients = new List<IngredientType>();

    private int currentIndex = 0;

    private void Start()
    {
        if (finalDishObject != null)
            finalDishObject.SetActive(false);
    }

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
            GameObject visual = Instantiate(
                ingredient.plateVisualPrefab,
                point.position,
                point.rotation,
                point
            );

            spawnedVisuals.Add(visual);
        }

        held.UnHighlight();
        Destroy(held.gameObject);

        ingredients.Add(ingredient.type);
        currentIndex++;

        playerHold.Drop();

        Debug.Log("Ingrediente agregado al plato (visual)");

        if (validIngredients.Count > 0 && ingredients.Count == validIngredients.Count)
        {
            CompleteDish();
        }
    }

    void CompleteDish()
    {
        for (int i = 0; i < spawnedVisuals.Count; i++)
        {
            if (spawnedVisuals[i] != null)
                Destroy(spawnedVisuals[i]);
        }

        spawnedVisuals.Clear();

        if (finalDishObject != null)
        {
            finalDishObject.SetActive(true);
        }

        Debug.Log("PLATO COMPLETADO");
    }

    public int GetCurrentCount()
    {
        return ingredients.Count;
    }

    public int GetRequiredCount()
    {
        return validIngredients.Count;
    }
}