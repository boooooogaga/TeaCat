using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Asset")]
public class Dialogue : ScriptableObject
{
    public int currentRoot = 0;
    public DialogueRoot[] roots;
}

[System.Serializable]
public class DialogueRoot
{
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    [Header("Text")]
    public string speakerName;
    public string text;
    [Header("Icons")]
    public Sprite[] idle;
    public Sprite[] talking;
    public bool leftSide;
    [Header("Functions")]
    public DialogueEvent[] events;
    public bool readChoice;
    public DialogueChoice choice;
}

[System.Serializable]
public class DialogueEvent
{
    public string methodName;
    public int index;
    public bool callOnDialogue;
}

[System.Serializable]
public class DialogueChoice
{
    public ChoiceOption[] options;
}

[System.Serializable]
public class ChoiceOption
{
    public string text;
    public int nextRoot;
    public DialogueEvent[] events;
}
