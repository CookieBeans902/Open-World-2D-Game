using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public static GameData Instance { get; set; }
    public int sceneIndex;
    public Vector2 pos;
    public int direction;
    public int mainQuestID;
    public int mainQuestObjectiveIndex;
    public SerializableDictionary<int, int> sideQuestIDs;

    public GameData()
    {
        sceneIndex = 0;
        pos = Vector2.zero;
    }
}

