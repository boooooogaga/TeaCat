using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogeManager : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] Text nameText;
    [SerializeField] Text mainText;
    [SerializeField] KeyCode nextKey = KeyCode.E;
    [Header("Icons")]
    [SerializeField] Image leftIcon;
    [SerializeField] Image rightIcon;

    [Header("Choice")]
    [SerializeField] GameObject choiceContainer;
    [SerializeField] GameObject choiceButtonPrefab;

    [Header("Settings")]
    [SerializeField] float charDelay = 0.05f;
    [SerializeField] float spaceDelay = 0.03f;
    [SerializeField] float dotDelay = 0.1f;

    [Header("Icon Animation")]
    [SerializeField] float iconAnimDelay = 0.1f;

    Coroutine leftAnim;
    Coroutine rightAnim;
    bool speaking;

    [Header("Animation")]
    [SerializeField] Animator mainDialogueAnimator;
    [SerializeField] Animator rIconAnimator;
    [SerializeField] Animator lIconAnimator;

    Dialogue currentDialogue;
    DialogueRoot currentRoot;
    MonoBehaviour dialogueObject;
    int lineIndex;

    [SerializeField] Dialogue testDil;

    public bool typing;
    public bool inDialogue;

    Coroutine typingCoroutine;

    DialogueLine currentLine;
    string currentFullText;

    Interacting interacting;

    void Awake()
    {
        interacting = FindObjectOfType<Interacting>();
    }
    void Start()
    {
        // For testing
        if (testDil != null)
            StartDialogue(testDil);
    }

    void Update()
    {
        if (inDialogue)
        {
            if (Input.GetKeyDown(nextKey))
                Next();
        }
    }

    // Dialogue Control

    public void StartDialogue(Dialogue dialogue, MonoBehaviour target = null)
    {
        if (dialogue == null || dialogue.roots == null || dialogue.roots.Length == 0)
                return;
        if (dialogue.currentRoot < 0 || dialogue.currentRoot >= dialogue.roots.Length)
                return;
        if (inDialogue)
                return;


        inDialogue = true;
        currentDialogue = dialogue;
        currentRoot = dialogue.roots[dialogue.currentRoot];
        dialogueObject = target;
        lineIndex = 0;

        if (interacting != null)
            interacting.canInteract = false;

        ShowLine();
    }

    void ShowLine()
    {
        if (currentRoot == null ||
            currentRoot.lines == null ||
            lineIndex >= currentRoot.lines.Length)
        {
            EndDialogue();
            return;
        }

        currentLine = currentRoot.lines[lineIndex];
        currentFullText = currentLine.text;

        PlayIdle(currentLine);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        nameText.text = currentLine.speakerName;
        mainText.text = "";

        ClearChoices();

        typingCoroutine = StartCoroutine(
            HandleLine(currentLine)
        );
    }

    void FinishTyping()
    {
        if (!typing)
            return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        mainText.text = currentFullText;

        typing = false;
        speaking = false;

        PlayIdle(currentLine);

        if (currentLine.readChoice &&
            currentLine.choice != null)
        {
            ShowChoices(currentLine.choice);
        }
    }

    public void Next()
    {
        if (currentRoot == null ||
            currentRoot.lines == null)
            return;

        if (lineIndex >= currentRoot.lines.Length)
            return;

        if (typing)
        {
            FinishTyping();
            return;
        }

        if (currentRoot.lines[lineIndex].readChoice)
            return;

        lineIndex++;
        ShowLine();
    }

    IEnumerator HandleLine(DialogueLine line)
    {
        typing = true;
        speaking = true;

        PlayTalking(line);
        UpdateAnimatorParameters(line.leftSide);

        for (int i = 0; i < line.text.Length; i++)
        {
            if (line.events != null)
            {
                foreach (var ev in line.events)
                {
                    if (ev.index == i)
                    {
                        CallMethod(
                            ev.methodName,
                            ev.callOnDialogue
                                ? dialogueObject
                                : this
                        );
                    }
                }
            }

            mainText.text += line.text[i];

            yield return new WaitForSeconds(
                line.text[i] == ' '
                    ? spaceDelay
                    : line.text[i] == '.'
                        ? dotDelay
                        : charDelay
            );
        }

        typing = false;
        speaking = false;

        PlayIdle(line);

        if (line.readChoice &&
            line.choice != null)
        {
            ShowChoices(line.choice);
        }
    }

    void EndDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        StopAllCoroutines();
        StopIconAnim();

        inDialogue = false;
        typing = false;
        speaking = false;

        mainText.text = "";
        nameText.text = "";

        ClearChoices();

        mainDialogueAnimator.SetBool("visible", false);
        lIconAnimator.SetBool("visible", false);
        rIconAnimator.SetBool("visible", false);

        currentDialogue = null;
        currentRoot = null;
        dialogueObject = null;

        if (interacting != null)
            interacting.canInteract = true;
    }


    // Choices

    void ShowChoices(DialogueChoice choice)
    {
        foreach (var option in choice.options)
        {
            var btnObj = Instantiate(choiceButtonPrefab, choiceContainer.transform);
            var btnText = btnObj.GetComponentInChildren<Text>();
            btnText.text = option.text;

            var opt = option;
            btnObj.GetComponent<Button>().onClick.AddListener(() =>
            {
                ClearChoices();
                SelectChoice(opt);
            });
        }

        choiceContainer.SetActive(true);
    }

    void SelectChoice(ChoiceOption option)
    {
        if (option.events != null)
            foreach (var ev in option.events)
                CallMethod(ev.methodName, ev.callOnDialogue ? dialogueObject : this);

        if (option.nextRoot >= 0 && option.nextRoot < currentDialogue.roots.Length)
        {
            currentDialogue.currentRoot = option.nextRoot;
            currentRoot = currentDialogue.roots[option.nextRoot];
            lineIndex = 0;
            ShowLine();
        }
    }

    void ClearChoices()
    {
        choiceContainer.SetActive(false);
        foreach (Transform t in choiceContainer.transform)
            Destroy(t.gameObject);
    }

    // Icons

    void StopIconAnim()
    {
        if (leftAnim != null) StopCoroutine(leftAnim);
        if (rightAnim != null) StopCoroutine(rightAnim);
    }

    void PlayIdle(DialogueLine line)
    {
        speaking = false;
        StopIconAnim();

        Image icon = line.leftSide ? leftIcon : rightIcon;

        //leftIcon.gameObject.SetActive(line.leftSide);
        //rightIcon.gameObject.SetActive(!line.leftSide);

        Coroutine c = StartCoroutine(
            Anim(line.idle, icon, iconAnimDelay, () => true)
        );

        if (line.leftSide) leftAnim = c;
        else rightAnim = c;
    }

    void PlayTalking(DialogueLine line)
    {
        StopIconAnim();

        Image icon = line.leftSide ? leftIcon : rightIcon;

        //leftIcon.gameObject.SetActive(line.leftSide);
        //rightIcon.gameObject.SetActive(!line.leftSide);

        speaking = true;

        Coroutine c = StartCoroutine(
            Anim(line.talking, icon, iconAnimDelay, () => speaking)
        );

        if (line.leftSide) leftAnim = c;
        else rightAnim = c;
    }

    IEnumerator Anim(
    Sprite[] anim,
    Image icon,
    float delay,
    System.Func<bool> isActive
)
    {
        if (anim == null || anim.Length == 0)
            yield break;

        while (isActive())
        {
            for (int i = 0; i < anim.Length; i++)
            {
                if (!isActive())
                    yield break;

                icon.sprite = anim[i];
                yield return new WaitForSeconds(delay);
            }
        }
    }

    // Animation
    void UpdateAnimatorParameters(bool leftIconSide)
    {
        mainDialogueAnimator.SetBool("visible", inDialogue);
        lIconAnimator.SetBool("visible", leftIconSide);
        rIconAnimator.SetBool("visible", !leftIconSide);
    }

    // Seb Methods

    void CallMethod(string methodName, MonoBehaviour target)
    {
        if (target == null) return;

        var method = target.GetType().GetMethod(methodName,
            System.Reflection.BindingFlags.Public |
            System.Reflection.BindingFlags.Instance);

        if (method != null) method.Invoke(target, null);
    }
}
