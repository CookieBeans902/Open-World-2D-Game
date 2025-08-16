using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestButton : MonoBehaviour
{
    [SerializeField] QuestUI questUI;
    [SerializeField] QuestInstance displayQuest;
    [SerializeField] TMP_Text displayName;
    [SerializeField] TMP_Text displayDesc;
    [SerializeField] TMP_Text displayObjective;
    [SerializeField] TMP_Text coinAmount;
    [SerializeField] TMP_Text expAmount;
    [SerializeField] Image itemSprite;
    [SerializeField] TMP_Text ExtraRewardQuantity;
    [SerializeField] TMP_Text buttonName;
    [SerializeField] TMP_Text buttonDesc;

    public void Setup(QuestInstance quest, QuestUI reference)
    {
        this.questUI = reference;
        displayQuest = quest;
        displayName = questUI.displayName;
        displayDesc = questUI.displayDesc;
        displayObjective = questUI.displayObjective;
        buttonName.text = displayQuest.questData.questName;
        buttonDesc.text = displayQuest.questData.questDesc;
        this.coinAmount = questUI.coinDisplayAmount;
        this.expAmount = questUI.expDisplayAmount;
        this.itemSprite = questUI.extraRewardIcon;
        this.ExtraRewardQuantity = questUI.extraRewardQuantity;
    }
    public void DisplayUpdate()
    {
        displayName.text = displayQuest.questData.questName;
        displayDesc.text = displayQuest.questData.questDesc;
        displayObjective.text = displayQuest.CurrObjective.objective.FullLengthObjectiveDesc;
        coinAmount.text = displayQuest.questData.coinReward.ToString();
        expAmount.text = displayQuest.questData.experienceReward.ToString();
        ExtraItemDisplayUpdate();
    }
    public void SelectQuest()
    {
        QuestManager.Instance.selectedQuest = displayQuest;
    }
    void ExtraItemDisplayUpdate()
    {
        if (displayQuest.questData.extraReward == null)
        {
            questUI.ExtraItemNull(true);
        }
        else
        {
            questUI.ExtraItemNull(false);
            itemSprite.sprite = displayQuest.questData.extraReward.icon;
            ExtraRewardQuantity.text = displayQuest.questData.extraRewardQuantity.ToString();
        }
    }
}
