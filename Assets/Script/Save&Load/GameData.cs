using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public static GameData Instance { get; set; }
    public int sceneIndex;
    public Vector2 pos;
    public int direction;
    public QuestSaveData mainQuestInfo;
    public List<QuestSaveData> sideQuestInfo;
    //key is quest ID, int[0] is currentObjectiveIndex and int[1] is currentAmount of that Objective
    public List<int> completedSideQuests;

    public GameData()
    {
        sceneIndex = 0;
        pos = Vector2.zero;
        sideQuestInfo = new();
        completedSideQuests = new();
    }
}

