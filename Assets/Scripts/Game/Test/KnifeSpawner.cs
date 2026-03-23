using UnityEngine;

public class KnifeSpawner : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private GameObject knifeVisual; 

    public void Interact()
    {
        if (playerHold.HasKnife())
        {
            playerHold.ReturnKnife();

            if (knifeVisual != null)
                knifeVisual.SetActive(true);

            Debug.Log("Dejaste el cuchillo");
            return;
        }

        if (playerHold.IsHolding())
        {
            Debug.Log("Tienes las manos ocupadas");
            return;
        }

        playerHold.TakeKnife();

        if (knifeVisual != null)
            knifeVisual.SetActive(false);

        Debug.Log("Agarraste cuchillo");
    }
}