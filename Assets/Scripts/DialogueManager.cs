﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
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

    public TextMeshProUGUI dialogueText;
    public bool isOpen;
    public bool isWriting;
    public Animator animator;
    public GameObject continueTextObject;
    public bool instantText = true;
    public float letterRate = 10.0f;


    private Queue<string> sentences = new Queue<string>();
    private string currentSentence;
    // Start is called before the first frame update

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        isOpen = true;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        AdvanceSentence();
    }

    public void AdvanceSentence()
    {
        if (!instantText)
        {

            StopAllCoroutines();
            if (isWriting)
            {
                dialogueText.text = currentSentence;
                isWriting = false;
                continueTextObject.SetActive(true);
            }

            else
            {
                if (sentences.Count == 0)
                {
                    EndDialogue();
                    return;
                }
                currentSentence = sentences.Dequeue();
                continueTextObject.SetActive(false);
                StartCoroutine(TypeSentence(currentSentence));
            }
        }
        else
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }
            else
            {
                currentSentence = sentences.Dequeue();
                PrintSentence(currentSentence);
            }
        }
        

    }


    IEnumerator TypeSentence(string sentence)
    {
        isWriting = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1.0f / letterRate);
        }
        continueTextObject.SetActive(true);
        isWriting = false;
    }

    private void PrintSentence(string sentence)
    {
        dialogueText.text = sentence;
        continueTextObject.SetActive(true);
    }

    public void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        isOpen = false;
    }
}