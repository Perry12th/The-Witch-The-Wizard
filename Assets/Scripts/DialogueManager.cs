using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField]
    private GameObject narratorDialogueBox;
    [SerializeField]
    private TextMeshProUGUI narratorDialogueText;
    [SerializeField]
    private GameObject narratorContinueTextObject;

    [SerializeField]
    private GameObject actorDialogueBox;
    [SerializeField]
    private Image actorImage;
    [SerializeField]
    private TextMeshProUGUI actorDialogueText;
    [SerializeField]
    private TextMeshProUGUI actorNameText;
    [SerializeField]
    private GameObject actorContinueTextObject;
    
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private bool instantText = true;
    [SerializeField]
    private float letterRate = 10.0f;

    private DialogueType dialogueState = DialogueType.Narrator;
    private TextMeshProUGUI activeDialogueText;
    private GameObject activeContinueTextObject;
    private Queue<Dialogue> dialogues = new Queue<Dialogue>();
    private Dialogue currentDialouge;
    private bool isWriting;

    public bool isOpen { get; private set; }

    #region Singleton
    public static DialogueManager instance;

    private void Awake()
    {
        //Make sure there is only one instance
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    private void Start()
    {
        SwitchDialougeState(DialogueType.Narrator);
    }

    public void StartConversation(Conversation conversation)
    {
        animator.SetBool("IsOpen", true);
        isOpen = true;
        dialogues.Clear();

        foreach (Dialogue dialogue in conversation.DialogueV2s)
        {
            dialogues.Enqueue(dialogue);
        }

        AdvanceDialouge();
    }

    public void AdvanceDialouge()
    {
        if (!instantText)
        {

            StopAllCoroutines();
            if (isWriting)
            {
                activeDialogueText.text = currentDialouge.sentence;
                isWriting = false;
                activeContinueTextObject.SetActive(true);
            }

            else
            {
                if (dialogues.Count == 0)
                {
                    EndDialogue();
                    return;
                }

                currentDialouge = dialogues.Dequeue();
                if (currentDialouge != null && currentDialouge.dialogueType != dialogueState)
                {
                    SwitchDialougeState(currentDialouge.dialogueType);
                }

                activeContinueTextObject.SetActive(false);
                StartCoroutine(TypeSentence(currentDialouge.sentence));
            }
        }
        else
        {
            if (dialogues.Count == 0)
            {
                EndDialogue();
                return;
            }
            else
            {
                currentDialouge = dialogues.Dequeue();
                if (currentDialouge != null && currentDialouge.dialogueType != dialogueState)
                {
                    SwitchDialougeState(currentDialouge.dialogueType);
                }

                PrintSentence(currentDialouge.sentence);
            }
        }
        

    }


    IEnumerator TypeSentence(string sentence)
    {
        isWriting = true;
        activeDialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            activeDialogueText.text += letter;
            yield return new WaitForSeconds(1.0f / letterRate);
        }
        activeContinueTextObject.SetActive(true);
        isWriting = false;
    }

    private void PrintSentence(string sentence)
    {
        activeDialogueText.text = sentence;
        activeContinueTextObject.SetActive(true);
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        isOpen = false;
    }

    private void SwitchDialougeState(DialogueType dialogueType)
    {
        switch (dialogueState)
        {
            case DialogueType.Narrator:
                narratorDialogueBox.SetActive(false);
                break;

            case DialogueType.Actor:
                actorDialogueBox.SetActive(false);
                break;
        }

        dialogueState = dialogueType;

        

        switch (dialogueState)
        {
            case DialogueType.Narrator:
                narratorDialogueBox.SetActive(true);
                activeContinueTextObject = narratorContinueTextObject;
                activeDialogueText = narratorDialogueText;
                break;

            case DialogueType.Actor:
                actorDialogueBox.SetActive(true);
                activeContinueTextObject = actorContinueTextObject;
                activeDialogueText = actorDialogueText;

                actorImage.sprite = currentDialouge.actor.actorSprite;
                actorNameText.text = currentDialouge.actor.actorName;
                break;
        }


    }
}
