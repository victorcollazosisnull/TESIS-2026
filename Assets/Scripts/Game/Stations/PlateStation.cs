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

        if (!ingredient.isCut && !ingredient.isCooked)
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

        held.transform.SetParent(ingredientPoints[currentIndex]);
        held.transform.localPosition = Vector3.zero;
        held.transform.localRotation = Quaternion.identity;

        Rigidbody rb = held.GetComponent<Rigidbody>();
        Collider col = held.GetComponent<Collider>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        if (col != null)
        {
            col.enabled = false; 
        }

        held.Lock();
        held.SetCanDrop(false);

        ingredients.Add(ingredient.type);
        currentIndex++;

        playerHold.Drop();

        Debug.Log("Ingrediente agregado al plato");
    }
}