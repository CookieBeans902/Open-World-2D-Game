using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Scriptable Objects/QuestSO")]
public class QuestSO : ScriptableObject
{
    public int QuestID;
    public string QuestName;
    public string QuestDesc;
    public int coinReward;
    public int experienceReward;
    public ItemSO extraReward;
    public int extraRewardQuantity;
    public QuestState questState = QuestState.notStarted;
    public int[] questDependancies;
    // Each Quest has a unique QuestID, using which we can determine and execute only if 
    // all of the quests are completed
}
