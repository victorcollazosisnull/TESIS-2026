using UnityEngine;

public class PlayerHold : MonoBehaviour
{
    [SerializeField] private Transform holdPoint;

    private PickupObject currentObject;

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
}