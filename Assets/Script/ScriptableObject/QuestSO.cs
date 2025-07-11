using UnityEngine;
using System.Collections.Generic;
using Mono.Cecil;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/QuestSO")]
public class QuestSO : ScriptableObject
{
    public int questID;
    public string questName;
    public string questDesc;
    public int coinReward;
    public int experienceReward;
    public ItemSO extraReward;
    public int extraRewardQuantity;
    public List<ObjectiveSO> objectives;
    public readonly bool isOrdered;
    public int[] questDependancies;
    // Each Quest has a unique QuestID, using which we can determine and execute only if 
    // all of the quests are completed
}
