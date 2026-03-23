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

            if (ingredient != null && ingredient.isCut)
            {
                Debug.Log("Esto ya está cortado");
                return;
            }

            currentObject = held;
            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

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

            if (ingredient == null)
            {
                Debug.Log("No es ingrediente");
                return;
            }

            if (ingredient.isCut)
            {
                Debug.Log("Ya está cortado");
                return;
            }

            ingredient.isCut = true;

            Vector3 spawnPos = placePoint.position;
            Quaternion spawnRot = placePoint.rotation;

            Destroy(currentObject.gameObject);

            if (ingredient.cutPrefab != null)
            {
                PickupObject cutObj = Instantiate(ingredient.cutPrefab, spawnPos, spawnRot);

                cutObj.SetCanDrop(false);

                cutObj.transform.SetParent(null);

                cutObj.GetComponent<Collider>().enabled = true;

                currentObject = null;
            }

            Debug.Log("Ingrediente cortado");
        }
    }
}