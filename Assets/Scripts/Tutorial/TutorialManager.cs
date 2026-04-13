using UnityEngine;
using System.Collections;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    [SerializeField] private TextMeshProUGUI tutorialText;

    private int currentStep = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowStep();

        StartCoroutine(StartTutorialSequence());
    }

    public void NextStep()
    {
        currentStep++;
        ShowStep();
    }
    private IEnumerator StartTutorialSequence()
    {
        yield return new WaitForSeconds(1f); 

        NextStep(); 
    }
    private void ShowStep()
    {
        switch (currentStep)
        {
            case 0:
                TutorialFlags.canMove = false;
                TutorialFlags.canLook = false;
                TutorialFlags.canInteract = false;
                tutorialText.text = "Bienvenido a la cocina...";
                break;

            case 1:
                TutorialFlags.canMove = true;
                tutorialText.text = "Muévete con WASD";
                break;

            case 2:
                TutorialFlags.canLook = true;
                tutorialText.text = "Mueve la cámara";
                break;

            case 3:
                TutorialFlags.canInteract = true;
                tutorialText.text = "Presiona E para interactuar";
                break;

            case 4:
                tutorialText.text = "Agarra una papa";
                break;

            case 5:
                tutorialText.text = "Llévala a la mesa de corte";
                break;

            case 6:
                tutorialText.text = "Corta la papa";
                break;

            case 7:
                tutorialText.text = "Llévala a la sartén";
                break;

            case 8:
                tutorialText.text = "Cocina la papa";
                break;

            case 9:
                tutorialText.text = "Ponla en el plato";
                break;

            case 10:
                tutorialText.text = "¡Tutorial completado!";
                break;
        }
    }

    public int GetStep()
    {
        return currentStep;
    }
}