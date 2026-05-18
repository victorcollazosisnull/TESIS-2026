using UnityEngine;

public class TrashStation : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;

    [Header("Sounds")]
    [SerializeField] private SoundData trashSound;

    public void Interact()
    {
        if (!playerHold.IsHolding()) return;

        PickupObject held = playerHold.GetHeldObject();

        if (held == null) return;

        AudioManager.Instance.Play(trashSound);

        Destroy(held.gameObject);

        playerHold.Drop();

        Debug.Log("Objeto eliminado");
    }
}