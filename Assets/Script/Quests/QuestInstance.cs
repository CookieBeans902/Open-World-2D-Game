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
    public QuestID QuestID => questData.questID;
    public QuestState QuestState => questState;
    public ObjectiveState CurrObjective => runtimeObj[currentObjectiveIndex];
    public int currObjIndex => currentObjectiveIndex;
    public void SetObjIndex(int i) => currentObjectiveIndex = i;
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
    public void ObjectiveTracker()
    {
        if (runtimeObj[currentObjectiveIndex].isComplete)
        {
            currentObjectiveIndex++;
            QuestManager.Instance.CurrentQuestUpdateUI();
        }
        if (currentObjectiveIndex >= runtimeObj.Count)
        {
            QuestComplete();
        }
    }
    public void MarkObjectiveComplete()
    {
        if (CurrObjective.objective is CountableObjSO countable)
        {
            if (CurrObjective.currentAmount < countable.desiredAmount) return;
        }
        CurrObjective.isComplete = true;
        ObjectiveTracker();
    }
    void QuestComplete()
    {
        if (runtimeObj.All(obj => obj.isComplete))
        {
            questState = QuestState.Complete;
            if ((int)QuestID < 100) //It is a mainQuest
            {
                QuestManager.Instance.NextMainQuest();
            }
            else
            {
                QuestManager.Instance.SideQuestComplete(QuestID);
            }
        }
    }
}
