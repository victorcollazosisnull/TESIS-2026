using UnityEngine;

public class Ingredient : MonoBehaviour
{
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
        RiceCooked,

        Pescado,
        PescadoCut,

        Choclo,
        ChocloCooked,
        ChocloCut,

        Limon,
        LimonCut,
        LimonJuice,

        Camote,
        CamoteCooked,
        CamoteCut
    }

    [Header("Type")]
    public IngredientType type;

    [Header("States")]
    public bool isCut = false;
    public bool isCooked = false;

    [Header("Transforms")]
    public PickupObject cutPrefab;
    public PickupObject cookedPrefab;

    [Header("Visual Rotations")]
    public Vector3 cuttingRotation;

    [Header("Plate Visual")]
    public GameObject plateVisualPrefab;

    public bool CanBeCut()
    {
        return !isCut && cutPrefab != null;
    }

    public bool CanBeCooked()
    {
        //return isCut && !isCooked && cookedPrefab != null;
        return !isCooked && cookedPrefab != null;
    }

    public PickupObject GetCutResult(Vector3 pos, Quaternion rot)
    {
        if (cutPrefab == null) return null;

        return Instantiate(cutPrefab, pos, rot);
    }

    public PickupObject GetCookedResult(Vector3 pos, Quaternion rot)
    {
        if (cookedPrefab == null) return null;

        return Instantiate(cookedPrefab, pos, rot);
    }

    public bool IsReadyForPlate()
    {
        return isCut || isCooked;
    }
}