using UnityEngine;

public class SoySauceSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private GameObject soySauceVisual;

    public void Interact()
    {
        if (playerHold.HasSoySauce())
        {
            playerHold.ReturnSoySauce();

            if (soySauceVisual != null)
                soySauceVisual.SetActive(true);

            return;
        }

        if (playerHold.IsHolding() || playerHold.HasAnyTool())
        {
            Debug.Log("Tienes las manos ocupadas");
            return;
        }

        playerHold.TakeSoySauce();

        if (soySauceVisual != null)
            soySauceVisual.SetActive(false);
    }
}