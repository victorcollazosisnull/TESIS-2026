//using UnityEngine;

//public class AceiteSpawner : MonoBehaviour
//{
//    [SerializeField] private PlayerHold playerHold;
//    [SerializeField] private GameObject aceiteVisual;

//    public void Interact()
//    {
//        if (playerHold.HasAceite())
//        {
//            playerHold.ReturnAceite();

//            if (aceiteVisual != null)
//                aceiteVisual.SetActive(true);

//            return;
//        }

//        if (playerHold.IsHolding() || playerHold.HasAnyTool())
//        {
//            Debug.Log("Tienes las manos ocupadas");
//            return;
//        }

//        playerHold.TakeAceite();

//        if (aceiteVisual != null)
//            aceiteVisual.SetActive(false);
//    }
//}