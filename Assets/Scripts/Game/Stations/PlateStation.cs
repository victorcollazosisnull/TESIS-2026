using System.Collections.Generic;
using UnityEngine;

public class PlateStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform[] ingredientPoints;

    private List<string> ingredients = new List<string>();
    private int currentIndex = 0;

    public void Interact()
    {
        if (!playerHold.IsHolding()) return;

        PickupObject held = playerHold.GetHeldObject();
        Ingredient ingredient = held.GetComponent<Ingredient>();

        if (ingredient == null) return;

        if (!ingredient.isCut || ingredient.isCooked)
        {
            Debug.Log("Ingrediente no válido para plato");
            return;
        }

        if (ingredient.ingredientName != "TomateCut")
        {
            Debug.Log("Este ingrediente no va en el plato aún");
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

        held.Lock();
        held.SetCanDrop(false);

        ingredients.Add(ingredient.ingredientName);
        currentIndex++;

        playerHold.Drop();

        Debug.Log("Ingrediente agregado al plato");
    }
}