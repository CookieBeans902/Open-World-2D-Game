using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using System;

public class QuestManager : MonoBehaviour
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
    private bool inQuestWindow = false;
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
            var newMainQuest = new QuestInstance(mainQuestList[(int)currentQuestID + 1]);
            activeMainQuest = newMainQuest;
            questUI.MainQuestUpdate(activeMainQuest);
        }
        else
        {
            questUI.AllMainQuestsComplete();
        }
        questUI.MainQuestUpdate(activeMainQuest);
    }
    public void SideQuestComplete(QuestID questID)
    {
        availableSideQuests.TryGetValue(questID, out QuestInstance quest);
        questUI.UIDestroy(quest);
    }
    public void SetAsActiveQuest()
    {
        currentQuest = selectedQuest;
    }
    public void IncrementCount(Countables id)
    {
        currentQuest.CurrObjective.Increment(id);
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
    public void OpenQuestUI()
    {
        inQuestWindow = true;
        QuestUIPanel.SetActive(true);
    }
    public void CurrentQuestUpdateUI()
    {
        questUI.CurrentQuestUpdateUI();
    }
}