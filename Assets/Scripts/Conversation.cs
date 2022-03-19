using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Conversation", menuName = "Dialogue/Conversation", order = 1)]
public class Conversation : ScriptableObject
{
    public Dialogue[] DialogueV2s;
}

