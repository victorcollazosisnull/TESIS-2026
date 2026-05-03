using UnityEngine;

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/Dialogue/DialogueSO", order = 1)]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private string[] dialogueLines;

    [Header("Images")]
    public Sprite[] dialogueImages;

    [Header("Image Change Index")]
    public int[] imageChangeIndex; 

    public string[] DialogueLines => dialogueLines;
}