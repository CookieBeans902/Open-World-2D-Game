using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

[Serializable]
public class QuestInstance
{
    public QuestSO questData;
    [SerializeField] private QuestState questState = QuestState.notStarted;
    [SerializeField] private List<ObjectiveState> runtimeObj; //Obj is shorthand for objectives here
    private int currentObjectiveIndex = 0;
    //Some helper methods for ease of access
    public int QuestID => questData.questID;
    public QuestState QuestState => questState;
    public QuestInstance(QuestSO questSO)
    {
        questData = questSO;
        runtimeObj = new();
        questState = QuestState.onGoing;
        foreach (var obj in questData.objectives)
        {
            runtimeObj.Add(new ObjectiveState()
            {
                objective = obj,
                currentAmount = 0,
                isComplete = false,
            });
            currentObjectiveIndex = 0;
        }
    }
    public void ObjectiveTracker() //This is for linear and ordered quests.
    {
        if (runtimeObj[currentObjectiveIndex].isComplete)
        {
            currentObjectiveIndex++;
        }
        if (currentObjectiveIndex >= runtimeObj.Count)
        {
            QuestComplete();
        }
    }
    public void ObjectiveComplete() //For linear and ordered quests
    {
        var currObj = runtimeObj[currentObjectiveIndex];
        if (currObj.objective is CollectibleObjSO collectible)
        {
            if (currObj.currentAmount < collectible.desiredAmount) return;
        }
        currObj.isComplete = true;
    }
    void MarkObjectiveComplete(string objective) //Marking certain objectives complete, for unordered quests
    {
        foreach (var obj in runtimeObj)
        {
            if (obj.Name == objective)
            {
                obj.isComplete = true;
                break;
            }
        }
    }
    void GetCount(string id)
    {

    }
    void QuestComplete()
    {
        if (runtimeObj.All(obj => obj.isComplete))
        {
            questState = QuestState.Complete;
        }
    }
}
