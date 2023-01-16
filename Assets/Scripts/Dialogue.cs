using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DialogueType
{ 
    Narrator,
    Actor
}

[System.Serializable]
public class Dialogue 
{
    
    public DialogueType dialogueType = DialogueType.Narrator;
    public Actor actor;
    public bool actorFacingRight = true;
    public bool actorOnLeftSide = true;

    [TextArea(3, 10)]
    public string sentence;
}