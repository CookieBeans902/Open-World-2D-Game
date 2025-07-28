using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestButton : MonoBehaviour
{
    [SerializeField] QuestInstance displayQuest;
    [SerializeField] TMP_Text displayName;
    [SerializeField] TMP_Text displayDesc;
    [SerializeField] TMP_Text buttonName;
    [SerializeField] TMP_Text buttonDesc;
    [SerializeField] Sprite coinSprite;
    [SerializeField] Sprite expSprite;
    public void Setup(QuestInstance quest, TMP_Text Name, TMP_Text Desc)
    {
        displayQuest = quest;
        displayName = Name;
        displayDesc = Desc;
        buttonName.text = displayQuest.questData.questName;
        buttonDesc.text = displayQuest.questData.questDesc;
    }
    public void DisplayUpdate()
    {
        displayName.text = displayQuest.questData.questName;
        displayDesc.text = displayQuest.questData.questDesc;
    }
    public void SelectQuest()
    {
        QuestManager.Instance.selectedQuest = displayQuest;
    }
}
