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
    public Countables CountableType;
    //Some Helper methods for easier access.
    public string Name => objective.name;
    public void Increment(Countables id)
    {
        if (id == CountableType) currentAmount++;
    }
    public string GetObjectiveDesc()
    {
        if(objective is CountableObjSO countable) {
            return countable.objectiveDesc + $" <{currentAmount}/{countable.desiredAmount}>";
        }
        return objective.objectiveDesc;
    }
}
public enum NpcID
{
    Mayor = 0,
    Villager1 = 1,
}
public enum Countables
{
    None,
    Enemy,
    Flower,
    Crystal,
    Herb,
}
public enum QuestID
{
    //Using 0-99 for Main Quests
    VolatileMemory = 0,
    Exhaustion = 1,
    Illness = 2,
    HerbSearch = 3,
    Prince = 4,
    Evidence = 5,
    
    
    //Using 100+ for Side Quests
    FlowerCollection = 100,
    CrytalMining = 101,
}

