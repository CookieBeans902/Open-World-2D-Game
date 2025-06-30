using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour {
    public Button prevButton;
    public Button nextButton;
    public Button test;
    public Transform header;
    public Transform start;
    public List<Transform> equipSlots;

    void Start() {
        test.onClick.AddListener(() => Debug.Log("Button Pressed"));
    }
}
