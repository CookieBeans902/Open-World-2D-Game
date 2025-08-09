using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private Transform mainQuestContainer;
    [SerializeField] private Transform sideQuestContainer;
    [Header("Display In Quest Window")]
    public TMP_Text displayName;
    public TMP_Text displayDesc;
    public TMP_Text displayObjective;
    public TMP_Text coinDisplayAmount;
    public TMP_Text expDisplayAmount;
    public Image extraRewardIcon;
    public TMP_Text extraRewardQuantity;
    public GameObject buttonPrefab;
    [SerializeField] private QuestButton buttonScript;
    [Header("Displaying as Current Quest")]
    [SerializeField] private TMP_Text CurrentQuestNameText;
    [SerializeField] private TMP_Text CurrrentObjectiveDesc;
    readonly Dictionary<QuestID, GameObject> sideButtonList = new();

    public void SideQuestUpdate(QuestInstance quest)
    {
        var button = Instantiate(buttonPrefab, sideQuestContainer);
        button.name = quest.questData.questName;
        button.transform.SetAsFirstSibling();
        sideQuestContainer.SetParent(button.transform);
        buttonScript = button.GetComponent<QuestButton>();
        buttonScript.Setup(quest, this);
        sideButtonList.Add(quest.QuestID, button);
    }
    public void MainQuestUpdate(QuestInstance quest)
    {
        var button = Instantiate(buttonPrefab, mainQuestContainer);
        mainQuestContainer.SetParent(button.transform);
        button.transform.SetAsLastSibling();
        if (mainQuestContainer.childCount != 1)
        {
            Destroy(mainQuestContainer.GetChild(0).gameObject);
        }
        button.name = quest.questData.questName;
        buttonScript = button.GetComponent<QuestButton>();
        buttonScript.Setup(quest, this);
    }
    /// <summary>
    /// Takes input of a QuestInstance object referencing the side quest to be deleted
    /// <br>Note that this is only for SideQuests, Main Quest deletions are handled automatically</br>
    /// </summary>
    /// <param name="quest">The QuestInstance object</param>
    public void UIDestroy(QuestInstance quest)
    {
        if (sideButtonList.TryGetValue(quest.QuestID, out GameObject btn))
        {
            sideButtonList.Remove(quest.QuestID);
            Destroy(btn);
        }
    }
    public void AllMainQuestsComplete()
    {
        Destroy(mainQuestContainer.GetChild(0).gameObject);
        //Some logic for displaying end of all main quests.
    }
    public void CurrentQuestUpdateUI()
    {
        var currentQuest = QuestManager.Instance.currentQuest;
        CurrentQuestNameText.text = displayName.text = currentQuest.questData.questName;
        CurrrentObjectiveDesc.text = currentQuest.CurrObjective.GetObjectiveDesc();
    }
    public void InitialQuestUpdateUI(QuestInstance quest)
    {
        CurrentQuestNameText.text = displayName.text = quest.questData.questName;
        CurrrentObjectiveDesc.text = quest.CurrObjective.GetObjectiveDesc();
        displayObjective.text = quest.CurrObjective.objective.FullLengthObjectiveDesc;
        displayDesc.text = quest.questData.questDesc;
        coinDisplayAmount.text = quest.questData.coinReward.ToString();
        expDisplayAmount.text = quest.questData.experienceReward.ToString();
        if (quest.questData.extraReward == null) ExtraItemNull(true);
        else
        {
            extraRewardIcon.sprite = quest.questData.extraReward.icon;
            extraRewardQuantity.text = quest.questData.extraRewardQuantity.ToString();
        }
        
    }
    public void ExtraItemNull(bool isNull)
    {
        if (isNull)
        {
            extraRewardIcon.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            extraRewardQuantity.gameObject.SetActive(false);
        }
        else
        {
            extraRewardQuantity.gameObject.SetActive(true);
            extraRewardIcon.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }
    }
}
