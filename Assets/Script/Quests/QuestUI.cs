using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private Transform mainQuestContainer;
    [SerializeField] private Transform sideQuestContainer;
    [SerializeField] private TMP_Text displayName;
    [SerializeField] private TMP_Text displayDesc;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private QuestButton buttonScript;
    Dictionary<int, GameObject> sideButtonList;
    public void Start()
    {
        sideButtonList ??= new();
    }

    public void SideQuestUpdate(QuestSO quest)
    {
        var button = Instantiate(buttonPrefab, sideQuestContainer);
        button.name = quest.QuestName;
        button.transform.SetAsFirstSibling();
        sideQuestContainer.SetParent(button.transform);
        buttonScript = button.GetComponent<QuestButton>();
        buttonScript.Setup(quest, displayName, displayDesc);
        sideButtonList.Add(quest.QuestID, button);
        quest.questState = QuestState.onGoing;
    }
    public void MainQuestUpdate(QuestSO quest)
    {
        var button = Instantiate(buttonPrefab, mainQuestContainer);
        mainQuestContainer.SetParent(button.transform);
        button.transform.SetAsLastSibling();
        if (mainQuestContainer.childCount != 1)
        {
            Destroy(mainQuestContainer.GetChild(0).gameObject);
        }
        button.name = quest.QuestName;
        buttonScript = button.GetComponent<QuestButton>();
        buttonScript.Setup(quest, displayName, displayDesc);
        quest.questState = QuestState.onGoing;
    }
    public void Destroy(QuestSO quest)
    {
        if (sideButtonList.TryGetValue(quest.QuestID, out GameObject btn))
        {
            sideButtonList.Remove(quest.QuestID);
            Destroy(btn);
        }
    }
}
