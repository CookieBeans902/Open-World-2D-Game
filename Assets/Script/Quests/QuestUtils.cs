using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public enum QuestState
{
    Complete,
    onGoing,
    notStarted,
}
[Serializable]
public class ObjectiveState
{
    public ObjectiveSO objective;
    public int currentAmount = 0; //only used for the collectible type quests
    public bool isComplete;
    //Some Helper methods for easier access.
    public string Name => objective.name; 
}
