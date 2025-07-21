using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class QuestButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] QuestInstance displayQuest;
    [SerializeField] TMP_Text displayName;
    [SerializeField] TMP_Text displayDesc;
    [SerializeField] TMP_Text buttonName;
    [SerializeField] TMP_Text buttonDesc;
    [SerializeField] Sprite coinSprite;
    [SerializeField] Sprite expSprite;
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] Image image;
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
    //Logic For Switching Sprites Or Any Custom Logic when Selecting a Button in the UI

    public void OnDeselect(BaseEventData eventData)
    {
        image.sprite = defaultSprite;
    }
    public void OnSelect(BaseEventData eventData)
    {
        image.sprite = selectedSprite;
    }
}
