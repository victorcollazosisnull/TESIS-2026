using UnityEngine;

public class JuicerSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private GameObject juicerVisual;

    public void Interact()
    {
        if (playerHold.HasJuicer())
        {
            playerHold.ReturnJuicer();

            if (juicerVisual != null)
                juicerVisual.SetActive(true);

            Debug.Log("Dejaste exprimidor");
            return;
        }

        if (playerHold.IsHolding() || playerHold.HasAnyTool())
        {
            Debug.Log("Tienes las manos ocupadas");
            return;
        }

        playerHold.TakeJuicer();

        if (juicerVisual != null)
            juicerVisual.SetActive(false);

        Debug.Log("Agarraste exprimidor");
    }
}