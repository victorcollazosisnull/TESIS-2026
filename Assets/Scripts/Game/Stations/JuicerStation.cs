using UnityEngine;

public class JuicerStation : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;

    private PickupObject currentObject;

    public void Interact()
    {
        if (playerHold.IsHolding())
        {
            if (currentObject != null)
                return;

            PickupObject held = playerHold.GetHeldObject();

            Ingredient ingredient = held.GetComponent<Ingredient>();

            if (ingredient == null)
                return;

            if (ingredient.type != Ingredient.IngredientType.LimonCut)
            {
                Debug.Log("Necesitas limon cortado");
                return;
            }

            currentObject = held;

            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

            Debug.Log("Limon colocado");

            return;
        }

        if (currentObject != null)
        {
            if (!playerHold.HasJuicer())
            {
                Debug.Log("Necesitas exprimidor");
                return;
            }

            Ingredient ingredient =
                currentObject.GetComponent<Ingredient>();

            Vector3 spawnPos = placePoint.position;
            Quaternion spawnRot = placePoint.rotation;

            Destroy(currentObject.gameObject);

            PickupObject juice =
                ingredient.GetCookedResult(spawnPos, spawnRot);

            if (juice != null)
            {
                juice.SetCanDrop(false);
                juice.GetComponent<Collider>().enabled = true;
            }

            currentObject = null;

            Debug.Log("Jugo preparado");
        }
    }
}