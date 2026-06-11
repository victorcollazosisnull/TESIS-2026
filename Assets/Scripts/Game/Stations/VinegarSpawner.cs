using UnityEngine;

public class VinegarSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private GameObject vinegarVisual;

    public void Interact()
    {
        if (playerHold.HasVinegar())
        {
            playerHold.ReturnVinegar();

            if (vinegarVisual != null)
                vinegarVisual.SetActive(true);

            return;
        }

        if (playerHold.IsHolding() || playerHold.HasAnyTool())
        {
            Debug.Log("Tienes las manos ocupadas");
            return;
        }

        playerHold.TakeVinegar();

        if (vinegarVisual != null)
            vinegarVisual.SetActive(false);
    }
}