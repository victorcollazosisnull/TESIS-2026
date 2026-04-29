using UnityEngine;

public class TrashStation : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;

    public void Interact()
    {
        if (!playerHold.IsHolding()) return;

        PickupObject held = playerHold.GetHeldObject();

        if (held == null) return;

        Destroy(held.gameObject);

        playerHold.Drop();

        Debug.Log("Objeto eliminado");
    }
}