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
    Dictionary<int, GameObject> sideButtonList = new();
    public void SideQuestUpdate(QuestInstance quest)
    {
        var button = Instantiate(buttonPrefab, sideQuestContainer);
        button.name = quest.questData.questName;
        button.transform.SetAsFirstSibling();
        sideQuestContainer.SetParent(button.transform);
        buttonScript = button.GetComponent<QuestButton>();
        buttonScript.Setup(quest, displayName, displayDesc);
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
        buttonScript.Setup(quest, displayName, displayDesc);
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
}
