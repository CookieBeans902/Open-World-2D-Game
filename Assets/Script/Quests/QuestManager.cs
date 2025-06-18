using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using Pathfinding;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public Dictionary<int, QuestSO> availableSideQuests;
    public Dictionary<int, QuestSO> allMainQuests;
    [SerializeField] QuestUI questUI;
    [Header("List of the Side Quests")]
    public List<QuestSO> sideQuestList;
    [Header("List of the Main Quests")]
    public List<QuestSO> mainQuestList;
    [Header("The Current Active Side Quests")]
    [SerializeField] List<QuestSO> activeSideQuests = new(); //Tracks ON GOING side quests
    [SerializeField] QuestSO activeMainQuest;
    void Start()
    {
        // Singleton Instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // Creating the list of available Quests
        availableSideQuests ??= new();
        allMainQuests ??= new();
        foreach (QuestSO quest in sideQuestList)
        {
            availableSideQuests.Add(quest.QuestID, quest);
        }
        foreach (QuestSO quest in mainQuestList)
        {
            allMainQuests.Add(quest.QuestID, quest);
        }
    }
    public void BeginSideQuest(int questID)
    {
        Debug.Log("It works");
        availableSideQuests.TryGetValue(questID, out QuestSO newQuest);
        bool previousIncomplete = false;
        List<QuestSO> previousIncompleteQuests = new();
        foreach (int val in newQuest.questDependancies)
        {
            availableSideQuests.TryGetValue(val, out QuestSO dependancy);
            if (dependancy.questState != QuestState.Complete)
            {
                previousIncompleteQuests.Add(dependancy);
                previousIncomplete = true;
            }
            else continue;
        }
        if (previousIncomplete)
        {
            QuestError(previousIncompleteQuests);
            return;
        }
        if (!activeSideQuests.Contains(newQuest))
        {
            activeSideQuests.Add(newQuest);
            questUI.SideQuestUpdate(newQuest);
        }
    }
    public void BeginMainQuest(int questID)
    {
        for (int i = 1; i < questID; i++)
        {
            allMainQuests.TryGetValue(i, out QuestSO previousQuest);
            if (previousQuest.questState != QuestState.Complete)
            {
                QuestError(previousQuest);
                return;
            }
        }
        allMainQuests.TryGetValue(questID, out QuestSO newMainQuest);
        if (activeMainQuest == newMainQuest) return;
        activeMainQuest = newMainQuest;
        questUI.MainQuestUpdate(newMainQuest);
    }
    public void QuestError(List<QuestSO> errorQuests)
    {
        // FlashScreen();
    }
    public void QuestError(QuestSO errorMainQuest)
    {
        // FlashScreen();
    }
    public void SideQuestComplete(int questID)
    {
        availableSideQuests.TryGetValue(questID, out QuestSO quest);
        quest.questState = QuestState.Complete;
        questUI.Destroy(quest);
    }
}
