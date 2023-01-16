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
    private RectTransform actorDialogueBox;
    [SerializeField]
    private Image actorImage;
    [SerializeField]
    private TextMeshProUGUI actorDialogueText;
    [SerializeField]
    private TextMeshProUGUI actorNameText;
    [SerializeField]
    private GameObject actorContinueTextObject;
    [SerializeField]
    private Vector3 leftActorImagePosition;
    [SerializeField]
    private Vector3 rightActorImagePosition;
    [SerializeField]
    private Vector3 leftActorDialoguePosition;
    [SerializeField]
    private Vector3 rightActorDialoguePosition;

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
        actorDialogueBox.gameObject.SetActive(false);
        narratorDialogueBox.SetActive(false);
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
                if (currentDialouge != null)
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
        //activeContinueTextObject.SetActive(true);
        isWriting = false;
    }

    private void PrintSentence(string sentence)
    {
        activeDialogueText.text = sentence;
        //activeContinueTextObject.SetActive(true);
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
                actorDialogueBox.gameObject.SetActive(false);
                break;
        }

        dialogueState = dialogueType;

        

        switch (dialogueState)
        {
            case DialogueType.Narrator:
                narratorDialogueBox.SetActive(true);
                //activeContinueTextObject = narratorContinueTextObject;
                activeDialogueText = narratorDialogueText;
                break;

            case DialogueType.Actor:
                actorDialogueBox.gameObject.SetActive(true);
                //activeContinueTextObject = actorContinueTextObject;
                activeDialogueText = actorDialogueText;

                actorImage.rectTransform.sizeDelta = new Vector2(currentDialouge.actor.actorSprite.rect.width, currentDialouge.actor.actorSprite.rect.height) / 1.55f;
                actorNameText.text = currentDialouge.actor.actorName;

                actorImage.sprite = currentDialouge.actor.actorSprite;
                if (!currentDialouge.actorFacingRight)
                {
                    actorImage.rectTransform.eulerAngles = new Vector3(0, 180, 0);
                }
                else
                {
                    actorImage.rectTransform.eulerAngles = new Vector3(0, 0, 0);
                }
                if (currentDialouge.actorOnLeftSide)
                {
                    actorImage.rectTransform.anchoredPosition = leftActorImagePosition;
                    actorDialogueText.rectTransform.anchoredPosition = leftActorDialoguePosition;
                }
                else
                {
                    actorImage.rectTransform.anchoredPosition = rightActorImagePosition;
                    actorDialogueText.rectTransform.anchoredPosition = rightActorDialoguePosition;
                }
                break;
        }


    }
}
