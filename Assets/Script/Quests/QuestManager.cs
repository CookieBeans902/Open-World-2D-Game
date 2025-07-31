using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem;
using Unity.Collections.LowLevel.Unsafe;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    public static QuestManager Instance { get; private set; } //Global Instance
    private Dictionary<QuestID, QuestInstance> availableSideQuests;  //List of All SideQuests
    [SerializeField] QuestUI questUI;
    [Header("List of the Side Quests")] //To make use of Lists in inspector to create the dictionary
    [SerializeField] private List<QuestSO> sideQuestList;
    [Header("List of the Main Quests")]
    [SerializeField] private List<QuestSO> mainQuestList;
    [Header("The Current Active Side Quests")]
    [SerializeField] List<QuestInstance> activeSideQuests = new(); //Tracks ON GOING side quests
    [SerializeField] QuestInstance activeMainQuest;
    public QuestInstance currentQuest;
    public QuestInstance selectedQuest;
    [SerializeField] private GameObject QuestUIPanel;
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
        if (mainQuestList.Count > 0)
        {
            activeMainQuest = new QuestInstance(mainQuestList[0]);
            questUI.MainQuestUpdate(activeMainQuest);
            currentQuest = activeMainQuest;
            questUI.CurrentQuestUpdateUI();
        }
    }
    QuestInstance GetQuestByID(QuestID questID)
    {
        availableSideQuests.TryGetValue(questID, out QuestInstance quest);
        return quest;
    }
    /// <summary>
    /// Uses the QuestID of the list of Sidequests to begin the sidequest
    /// <br> It also verifies the dependancies before starting.
    /// </br>
    /// </summary>
    /// <param name="QuestID">The integer ID of the quest</param>
    public void BeginSideQuest(QuestID id)
    {
        QuestInstance sideQuest = GetQuestByID(id);
        bool previousIncomplete = false;
        foreach (QuestID val in sideQuest.questData.questDependancies)
        {
            QuestInstance dependancy = GetQuestByID(val);
            if (dependancy.QuestState != QuestState.Complete)
            {
                previousIncomplete = true;
            }
        }
        if (previousIncomplete)
        {
            // QuestError();
            return;
        }
        if (!activeSideQuests.Contains(sideQuest))
        {
            activeSideQuests.Add(sideQuest);
            questUI.SideQuestUpdate(sideQuest);
        }
    }
    [ContextMenu("Begin All")]
    public void TestingBeginAllSideQuests()
    {
        foreach (KeyValuePair<QuestID, QuestInstance> pair in availableSideQuests)
        {
            BeginSideQuest(pair.Key);
        }
    }
    /// <summary>
    /// This function starts the next MainQuest with its QuestID while verifying previous completed
    /// main quests.
    /// </summary>
    /// <param name="QuestID">Interger id of the Quest</param>
    public void NextMainQuest()
    {
        QuestID currentQuestID = activeMainQuest.QuestID;
        if ((int)currentQuestID + 1 < mainQuestList.Count)
        {
            questUI.UIDestroy(activeMainQuest);
            var newMainQuest = new QuestInstance(mainQuestList[(int)currentQuestID + 1]);
            activeMainQuest = newMainQuest;
            questUI.MainQuestUpdate(activeMainQuest);
        }
        else
        {
            questUI.AllMainQuestsComplete();
        }
    }
    public void SideQuestComplete(QuestID questID)
    {
        availableSideQuests.TryGetValue(questID, out QuestInstance quest);
        questUI.UIDestroy(quest);
    }
    public void SetAsActiveQuest()
    {
        currentQuest = selectedQuest;
        questUI.CurrentQuestUpdateUI();
    }
    [ContextMenu("Increment")]
    public void IncrementCount()
    {
        currentQuest.CurrObjective.Increment();
        currentQuest.MarkObjectiveComplete();
        questUI.CurrentQuestUpdateUI();
    }
    public bool CanInteract(NpcID npcID)
    {
        NpcID target = currentQuest.CurrObjective.objective.targetNpcID;
        if (target == npcID)
        {
            return true;
        }
        return false;
    }
    public void LoadData(GameData gameData)
    {
        activeSideQuests.Clear(); //clearing any existing SideQuests when loading the data
        //Loading SideQuests
        foreach (var savedQuest in gameData.sideQuestInfo)
        {
            QuestInstance quest = GetQuestByID(savedQuest.questID);
            quest.SetObjectiveStates(savedQuest);
            activeSideQuests.Add(quest);
            questUI.SideQuestUpdate(quest);
        }
        //Loading MainQuest
        QuestID id = gameData.mainQuestInfo.questID;
        activeMainQuest = new QuestInstance(mainQuestList[(int)id]);
        activeMainQuest.SetObjectiveStates(gameData.mainQuestInfo);
    }

    public void SaveData(GameData gameData)
    {
        gameData.sideQuestInfo.Clear(); //Clearing existing gameData to start anew
        //Saving SideQuests
        foreach (var quest in activeSideQuests)
        {
            QuestSaveData saveData = new()
            {
                questID = quest.QuestID,
                currObjIndex = quest.currObjIndex,
                savedAmount = quest.CurrObjective.currentAmount
            };
            gameData.sideQuestInfo.Add(saveData);
        }
        //Saving MainQuest
        QuestSaveData mainQuestSave = new()
        {
            questID = activeMainQuest.QuestID,
            currObjIndex = activeMainQuest.currObjIndex,
            savedAmount = activeMainQuest.CurrObjective.currentAmount
        };
        gameData.mainQuestInfo = mainQuestSave;
    }
    public void UpdateCurrentQuestUI() => questUI.CurrentQuestUpdateUI();
}