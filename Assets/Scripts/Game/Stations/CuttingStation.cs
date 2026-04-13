using UnityEngine;

public class CuttingStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;

    private PickupObject currentObject;

    public void Interact()
    {
        if (playerHold.IsHolding())
        {
            if (currentObject != null)
            {
                Debug.Log("Ya hay algo en la mesa");
                return;
            }

            PickupObject held = playerHold.GetHeldObject();
            Ingredient ingredient = held.GetComponent<Ingredient>();

            if (ingredient == null)
            {
                Debug.Log("No es ingrediente");
                return;
            }

            if (!ingredient.CanBeCut())
            {
                Debug.Log("No se puede cortar");
                return;
            }

            currentObject = held;

            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

            if (TutorialManager.Instance?.GetStep() == 5)
            {
                TutorialManager.Instance.NextStep(); 
            }

            Debug.Log("Objeto colocado en mesa");
            return;
        }

        if (currentObject != null)
        {
            if (!playerHold.HasKnife())
            {
                Debug.Log("Necesitas cuchillo");
                return;
            }

            Ingredient ingredient = currentObject.GetComponent<Ingredient>();

            if (ingredient == null) return;

            if (!ingredient.CanBeCut())
            {
                Debug.Log("No se puede cortar");
                return;
            }

            Vector3 spawnPos = placePoint.position;
            Quaternion spawnRot = placePoint.rotation;

            Destroy(currentObject.gameObject);

            PickupObject cutObj = ingredient.GetCutResult(spawnPos, spawnRot);

            if (cutObj != null)
            {
                cutObj.SetCanDrop(false);
                cutObj.GetComponent<Collider>().enabled = true;
            }

            currentObject = null;

            if (TutorialManager.Instance?.GetStep() == 6)
            {
                TutorialManager.Instance.NextStep();
            }

            Debug.Log("Ingrediente cortado");
        }
    }
}