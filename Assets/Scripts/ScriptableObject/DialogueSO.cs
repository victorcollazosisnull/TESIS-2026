using UnityEngine;
[CreateAssetMenu(fileName = "DialogueSO", menuName = "Scriptable Objects/Dialogue/DialogueSO", order = 1)]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private string[] dialogueLines;
    public string[] DialogueLines => dialogueLines;
}
