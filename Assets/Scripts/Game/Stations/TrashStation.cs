using DG.Tweening;
using UnityEngine;

public class TrashStation : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerHold playerHold;

    [Header("Sounds")]
    [SerializeField] private SoundData trashSound;

    [Header("UI")]
    [SerializeField] private GameObject helpUI;

    [Header("Arrow Animation")]
    [SerializeField] private float moveAmount = 15f;
    [SerializeField] private float moveSpeed = 0.6f;
    private void Start()
    {
        if (helpUI != null)
        {
            helpUI.SetActive(false);

            helpUI.transform.DOLocalMoveY(
                helpUI.transform.localPosition.y + moveAmount,
                moveSpeed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
    }
    private void Update()
    {
        if (helpUI == null) return;

        bool shouldShow = false;

        if (playerHold.IsHolding())
        {
            PickupObject held = playerHold.GetHeldObject();

            if (held != null)
                shouldShow = true;
        }

        helpUI.SetActive(shouldShow);
    }
    public void Interact()
    {
        if (!playerHold.IsHolding()) return;

        PickupObject held = playerHold.GetHeldObject();

        if (held == null) return;

        AudioManager.Instance.Play(trashSound);

        Destroy(held.gameObject);

        playerHold.Drop();

        Debug.Log("Objeto eliminado");
    }
}