using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Ingredient;

public class PlateStation : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private PlayerHold playerHold;
    [SerializeField] private Transform[] ingredientPoints;
    [SerializeField] private GameObject finalDishObject;
    [SerializeField] private ParticleSystem completionParticles;

    [Header("Sounds")]
    [SerializeField] private SoundData placeSound;
    [SerializeField] private SoundData victorySound;

    [Header("Ingredients Finals")]
    [SerializeField] private List<IngredientType> validIngredients;

    private List<GameObject> spawnedVisuals = new List<GameObject>();
    private List<IngredientType> ingredients = new List<IngredientType>();
    private int currentIndex = 0;

    [Header("UI")]
    [SerializeField] private GameObject helpUI;
    [Header("Arrow Animation")]
    [SerializeField] private float moveAmount = 15f;
    [SerializeField] private float moveSpeed = 0.6f;

    private bool uiVisible = false;

    private void Start()
    {
        if (finalDishObject != null)
            finalDishObject.SetActive(false);

        if (completionParticles != null)
            completionParticles.Stop();

        helpUI.SetActive(false);

        helpUI.transform.DOLocalMoveY(
            helpUI.transform.localPosition.y + moveAmount, moveSpeed)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void Update()
    {
        bool shouldShow = false;

        if (playerHold.IsHolding())
        {
            PickupObject held = playerHold.GetHeldObject();
            if (held != null)
            {
                Ingredient ingredient = held.GetComponent<Ingredient>();
                if (ingredient != null &&
                    ingredient.IsReadyForPlate() &&
                    validIngredients.Contains(ingredient.type) &&
                    !ingredients.Contains(ingredient.type))
                {
                    shouldShow = true;
                }
            }
        }

        if (shouldShow != uiVisible)
        {
            uiVisible = shouldShow;
            helpUI.SetActive(shouldShow);
        }
    }

    public void Interact()
    {
        if (!playerHold.IsHolding())
        {
            GameFeedbackUI.Instance?.Show("No tienes nada en la mano");
            return;
        }

        PickupObject held = playerHold.GetHeldObject();
        Ingredient ingredient = held.GetComponent<Ingredient>();

        if (ingredient == null) return;

        if (!ingredient.IsReadyForPlate())
        {
            GameFeedbackUI.Instance?.Show("Este ingrediente aun no esta listo para el plato");
            return;
        }

        if (!validIngredients.Contains(ingredient.type))
        {
            GameFeedbackUI.Instance?.Show("Este ingrediente no va en este plato");
            return;
        }

        if (ingredients.Contains(ingredient.type))
        {
            GameFeedbackUI.Instance?.Show("Este ingrediente ya esta en el plato");
            return;
        }

        if (currentIndex >= ingredientPoints.Length)
        {
            GameFeedbackUI.Instance?.Show("El plato ya esta completo");
            return;
        }

        Transform point = ingredientPoints[currentIndex];

        if (ingredient.plateVisualPrefab != null)
        {
            GameObject visual = Instantiate(
                ingredient.plateVisualPrefab,
                point.position, point.rotation, point);
            spawnedVisuals.Add(visual);
        }

        held.UnHighlight();
        Destroy(held.gameObject);

        ingredients.Add(ingredient.type);
        currentIndex++;

        playerHold.Drop();

        bool isLastIngredient = ingredients.Count == validIngredients.Count;

        if (isLastIngredient)
        {
            CompleteDish();
        }
        else
        {
            AudioManager.Instance.Play(placeSound);
        }
    }

    void CompleteDish()
    {
        for (int i = 0; i < spawnedVisuals.Count; i++)
            if (spawnedVisuals[i] != null)
                Destroy(spawnedVisuals[i]);

        spawnedVisuals.Clear();

        if (finalDishObject != null)
            finalDishObject.SetActive(true);

        if (completionParticles != null)
            completionParticles.Play();

        AudioManager.Instance.Play(victorySound);
    }

    public int GetCurrentCount() => ingredients.Count;
    public int GetRequiredCount() => validIngredients.Count;
}