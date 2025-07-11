using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using Pathfinding;
using System;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; } //Global Instance
    public Dictionary<int, QuestInstance> availableSideQuests;  //List of All SideQuests
    [SerializeField] QuestUI questUI;
    [Header("List of the Side Quests")] //To make use of Lists in inspector to create the dictionary
    public List<QuestSO> sideQuestList;
    [Header("List of the Main Quests")]
    public List<QuestSO> mainQuestList;
    [Header("The Current Active Side Quests")]
    [SerializeField] List<QuestInstance> activeSideQuests = new(); //Tracks ON GOING side quests
    [SerializeField] QuestInstance activeMainQuest;
    [SerializeField] QuestInstance currentQuest;
    private float timer;
    void Awake()
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
        foreach (QuestSO quest in sideQuestList)
        {
            var questInstance = new QuestInstance(quest);
            availableSideQuests.Add(quest.questID, questInstance);
        }
        activeMainQuest = new QuestInstance(mainQuestList[0]);
        questUI.MainQuestUpdate(activeMainQuest);
    }
    [ContextMenu("MainQuest0")]
    void StartMainQuest0()
    {
        activeMainQuest = new QuestInstance(mainQuestList[0]);
        questUI.MainQuestUpdate(activeMainQuest);
    }
    /// <summary>
    /// Uses the QuestID of the list of Sidequests to begin the sidequest
    /// <br> It also verifies the dependancies before starting.
    /// </br>
    /// </summary>
    /// <param name="QuestID">The integer ID of the quest</param>
    public void BeginSideQuest(int QuestID)
    {
        availableSideQuests.TryGetValue(QuestID, out QuestInstance newQuest);
        bool previousIncomplete = false;
        foreach (int val in newQuest.questData.questDependancies)
        {
            availableSideQuests.TryGetValue(val, out QuestInstance dependancy);
            if (dependancy.QuestState != QuestState.Complete)
            {
                previousIncomplete = true;
            }
            else continue;
        }
        if (previousIncomplete)
        {
            QuestError();
            return;
        }
        if (!activeSideQuests.Contains(newQuest))
        {
            activeSideQuests.Add(newQuest);
            questUI.SideQuestUpdate(newQuest);
        }
    }
    /// <summary>
    /// This function starts the next MainQuest with its QuestID while verifying previous completed
    /// main quests.
    /// </summary>
    /// <param name="QuestID">Interger id of the Quest</param>
    public void NextMainQuest()
    {
        int currentQuestID = activeMainQuest.QuestID;
        var newMainQuest = new QuestInstance(mainQuestList[currentQuestID + 1]);
        activeMainQuest = newMainQuest;
        questUI.MainQuestUpdate(activeMainQuest);
    }
    public void QuestError()
    {
        // FlashScreen();
    }
    public void QuestError(QuestInstance errorMainQuest)
    {
        // FlashScreen();
    }
    public void SideQuestComplete(int QuestID)
    {
        availableSideQuests.TryGetValue(QuestID, out QuestInstance quest);
        questUI.UIDestroy(quest);
    }
    public void SetAsActiveQuest(QuestInstance quest)
    {
        currentQuest = quest;
    }

}
