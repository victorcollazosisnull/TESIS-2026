using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;

    public bool isCut = false;
    public bool isCooked = false;

    [Header("Cut Result")]
    public PickupObject cutPrefab;

    [Header("Cook Result")]
    public PickupObject cookedPrefab;

    public enum IngredientType
    {
        Tomate,
        Cebolla,
        Papa,
        Carne,

        TomateCut,
        CebollaCut,
        PapaCut,
        CarneCut,

        CarneCooked,
        PapaCooked,

        Rice,
        RiceCooked
    }

    public IngredientType type;
}