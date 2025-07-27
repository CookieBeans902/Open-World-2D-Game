using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {
    public Button prevButton;
    public Button nextButton;
    public Button closeButton;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI desc;

    public Transform headSlot;
    public Transform bodySlot;
    public Transform bootSlot;
    public Transform AccessorySlot;
    public Transform hand1Slot;
    public Transform hand2Slot;

    public Transform fieldsContent;
    public Transform inventoryContent;
    public Transform skillContent;
}
