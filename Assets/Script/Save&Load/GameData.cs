using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int sceneIndex;
    public SerializableDictionary<int, Vector2> position;
    public int playerLevel;
    public int playerExp;
    public QuestID currentQuestID;
    public QuestSaveData mainQuestInfo;
    public List<QuestSaveData> sideQuestInfo;
    public List<int> completedSideQuests;
    public List<ItemSaveData> inventoryItems;
    public List<SkillSaveData> skillItems;
    public SerializableDictionary<string, bool> collectibleInformation;
    public GameData()
    {
        playerLevel = 0;
        playerExp = 0;
        sceneIndex = 1;
        position = new();
        position[1] = new Vector2();
        sideQuestInfo = new();
        mainQuestInfo = new();
        completedSideQuests = new();
        inventoryItems = new();
        skillItems = new();
        collectibleInformation = new();
    }
}

