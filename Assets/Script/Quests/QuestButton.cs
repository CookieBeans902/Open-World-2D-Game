using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    [SerializeField] QuestSO displayQuest;
    [SerializeField] TMP_Text questName;
    [SerializeField] TMP_Text questDesc;
    [SerializeField] TMP_Text buttonName;
    [SerializeField] TMP_Text buttonDesc;
    [SerializeField] Sprite coinSprite;
    [SerializeField] Sprite expSprite;


    public void Setup(QuestSO quest,TMP_Text Name,TMP_Text Desc)
    {
        displayQuest = quest;
        questName = Name;
        questDesc = Desc;
        buttonName.text = displayQuest.QuestName;
        buttonDesc.text = displayQuest.QuestDesc;
    }
    public void DisplayUpdate()
    {
        questName.text = displayQuest.QuestName;
        questDesc.text = displayQuest.QuestDesc;
    }
    
}
