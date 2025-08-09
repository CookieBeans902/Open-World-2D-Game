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
        position = new()
        {
            {1,new Vector2(-27.95f,1.27f)},
            {2,new Vector2(-48.26f,6.74f)},
            {3,new Vector2(14.11f,-6.45f)},
            {4,new Vector2(-8.06f,-4.05f)},
            {5,new Vector2(-8.29f,-4.34f)},
            {6,new Vector2(0.41f,-3f)}
        };
        sideQuestInfo = new();
        mainQuestInfo = new();
        completedSideQuests = new();
        inventoryItems = new();
        skillItems = new();
        collectibleInformation = new();
    }
}

