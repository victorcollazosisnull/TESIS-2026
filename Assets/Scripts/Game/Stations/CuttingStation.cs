using System;
using UnityEngine;

public class CuttingStation : MonoBehaviour, IInteractable, IHighlightable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;

    [Header("Sounds")]
    [SerializeField] private SoundData cutSound;

    private PickupObject currentObject;

    [Header("Highlight")]
    [SerializeField] private Renderer rend;
    private bool canHighlight = true;

    [SerializeField] private Color highlightColor = Color.red;
    [SerializeField] private float intensity = 2f;

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

            if (ingredient == null || !ingredient.CanBeCut())
            {
                Debug.Log("No se puede colocar o cortar");
                return;
            }

            currentObject = held;

            currentObject.SetAssignedStation(this);

            canHighlight = true;
            held.SetHighlight(false);

            held.Lock();
            held.GetComponent<Collider>().enabled = false;

            playerHold.Drop();

            held.transform.position = placePoint.position;
            held.transform.rotation = placePoint.rotation;

            Transform visual = held.transform.GetChild(0);
            visual.localRotation = Quaternion.Euler(ingredient.cuttingRotation);

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
            if (ingredient == null || !ingredient.CanBeCut()) return;

            playerHold.PlayKnifeAnimation();
            AudioManager.Instance.Play(cutSound);

            Vector3 spawnPos = placePoint.position;
            Quaternion spawnRot = placePoint.rotation;

            Destroy(currentObject.gameObject);

            PickupObject cutObj = ingredient.GetCutResult(spawnPos, spawnRot);

            if (cutObj != null)
            {
                cutObj.GetComponent<Collider>().enabled = true;
                cutObj.SetCanDrop(false);
                cutObj.SetHighlight(true);

                cutObj.SetAssignedStation(this);

                currentObject = cutObj;
            }

            canHighlight = false;
            UnHighlight();

            Debug.Log("Ingrediente cortado");
        }
    }

    public void Highlight()
    {
        if (!canHighlight) return;
        if (rend == null) return;

        foreach (Material mat in rend.materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", highlightColor * intensity);
            }
        }
    }

    public void UnHighlight()
    {
        if (rend == null) return;

        foreach (Material mat in rend.materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.SetColor("_EmissionColor", Color.black);
            }
        }
    }

    public void ClearStation()
    {
        currentObject = null;
        canHighlight = true;
        UnHighlight();
    }
}