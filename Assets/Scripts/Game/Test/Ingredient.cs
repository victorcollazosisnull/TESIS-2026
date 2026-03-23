using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public string ingredientName;

    public bool isCut = false;
    public bool isCooked = false;

    [Header("Cut Result")]
    public PickupObject cutPrefab;
}