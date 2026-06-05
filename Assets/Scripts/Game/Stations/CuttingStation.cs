using DG.Tweening;
using System;
using UnityEngine;

public class CuttingStation : MonoBehaviour, IInteractable, IHighlightable, IStation
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform placePoint;

    [Header("Sounds")]
    [SerializeField] private SoundData cutSound;
    [SerializeField] private SoundData placeSound;
    [SerializeField] private SoundData pickupSound;

    private PickupObject currentObject;

    [Header("Highlight")]
    [SerializeField] private Renderer rend;
    private bool canHighlight = true;

    [Header("Arrow UI")]
    [SerializeField] private GameObject arrowUI;
    [Header("Arrow Animation")]
    [SerializeField] private float moveAmount = 15f;
    [SerializeField] private float moveSpeed = 0.6f;

    [SerializeField] private Color highlightColor = Color.red;
    [SerializeField] private float intensity = 2f;

    public void Start()
    {
        if (arrowUI != null)
        {
            arrowUI.SetActive(false);
            arrowUI.transform.DOLocalMoveY(
                arrowUI.transform.localPosition.y + moveAmount, moveSpeed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }

    private void Update()
    {
        if (arrowUI == null) return;

        bool shouldShow = false;

        if (playerHold.IsHolding() && currentObject == null)
        {
            PickupObject held = playerHold.GetHeldObject();
            if (held != null)
            {
                Ingredient ingredient = held.GetComponent<Ingredient>();
                if (ingredient != null && ingredient.CanBeCut())
                    shouldShow = true;
            }
        }

        arrowUI.SetActive(shouldShow);
    }

    public void Interact()
    {
        if (playerHold.IsHolding())
        {
            if (currentObject != null)
            {
                GameFeedbackUI.Instance?.Show("La mesa de picar ya tiene algo encima");
                return;
            }

            PickupObject held = playerHold.GetHeldObject();
            Ingredient ingredient = held.GetComponent<Ingredient>();

            if (ingredient == null || !ingredient.CanBeCut())
            {
                GameFeedbackUI.Instance?.Show("Este ingrediente no se puede picar aqui");
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

            AudioManager.Instance.Play(placeSound);
            return;
        }

        if (currentObject != null)
        {
            if (!playerHold.HasKnife())
            {
                GameFeedbackUI.Instance?.Show("Necesitas el cuchillo para picar");
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
            return;
        }

        GameFeedbackUI.Instance?.Show("Pon un ingrediente sobre la mesa primero");
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
                mat.SetColor("_EmissionColor", Color.black);
        }
    }

    public void ClearStation()
    {
        currentObject = null;
        canHighlight = true;
        AudioManager.Instance.Play(pickupSound);
    }
}