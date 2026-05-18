using UnityEngine;

public class IngredientSpawner : MonoBehaviour, IInteractable
{
    [Header("Config")]
    [SerializeField] private PickupObject ingredientPrefab;

    [Header("References")]
    [SerializeField] private PlayerHold playerHold;

    [Header("Sounds")]
    [SerializeField] private SoundData pickupSound;

    public void Interact()
    {
        if (playerHold.IsHolding() || playerHold.HasKnife())
        {
            Debug.Log("Manos ocupadas");
            return;
        }

        PickupObject newIngredient = Instantiate(ingredientPrefab);

        playerHold.PickUp(newIngredient);

        AudioManager.Instance.Play(pickupSound);

        Debug.Log("Agarraste: " + newIngredient.name);
    }
}