using UnityEngine;

public class PlayerHold : MonoBehaviour 
{
    [SerializeField] private Transform holdPoint;
    [SerializeField] private GameObject knifeVisual;
    [SerializeField] private GameObject juiceVisual; 

    private PickupObject currentObject;
    private bool hasKnife = false;
    private bool hasJuicer = false;
    public bool IsHolding()
    {
        return currentObject != null;
    }

    public PickupObject GetHeldObject()
    {
        return currentObject;
    }

    public void PickUp(PickupObject obj)
    {
        if (HasAnyTool()) return;

        currentObject = obj;

        obj.transform.SetParent(holdPoint);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        obj.OnPickUp();
    }

    public void Drop()
    {
        if (currentObject == null) return;

        currentObject.transform.SetParent(null);
        currentObject.OnDrop();

        currentObject = null;
    }

    public bool HasKnife()
    {
        return hasKnife;
    }

    public void TakeKnife()
    {
        hasKnife = true;

        if (knifeVisual != null)
            knifeVisual.SetActive(true);
    }

    public void ReturnKnife()
    {
        hasKnife = false;

        if (knifeVisual != null)
            knifeVisual.SetActive(false);
    }

    public void TakeJuicer()
    {
        hasJuicer = true;

        if (juiceVisual != null)
            juiceVisual.SetActive(true);
    }

    public void ReturnJuicer()
    {
        hasJuicer = false;

        if (juiceVisual != null)
            juiceVisual.SetActive(false);
    }

    public bool HasJuicer()
    {
        return hasJuicer;
    }

    public bool HasAnyTool()
    {
        return hasKnife || hasJuicer;
    }
}