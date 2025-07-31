using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int sceneIndex;
    public Vector2 pos;
    public int direction;
    public QuestSaveData mainQuestInfo;
    public List<QuestSaveData> sideQuestInfo;
    public List<int> completedSideQuests;

    public GameData()
    {
        sceneIndex = 0;
        pos = Vector2.zero;
        sideQuestInfo = new();
        completedSideQuests = new();
    }
}

