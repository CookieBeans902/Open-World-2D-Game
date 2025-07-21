using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/QuestSO")]
public class QuestSO : ScriptableObject
{
    public QuestID questID;
    public string questName;
    public string questDesc;
    public int coinReward;
    public int experienceReward;
    public ItemSO extraReward;
    public int extraRewardQuantity;
    public List<ObjectiveSO> objectives;
    public QuestID[] questDependancies;
    // Each Quest has a unique QuestID, using which we can determine and execute only if 
    // all of the quests are completed
}
