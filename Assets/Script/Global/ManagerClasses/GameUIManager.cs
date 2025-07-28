using UnityEngine;
using System.Collections.Generic;
public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance;
    Dictionary<int, GameObject> panelDict;
    [SerializeField] GameObject questPanel;
    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject inventoryPanel;
    public bool inPanel = false;
    void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        panelDict = new()
        {
            {1,questPanel},
            {2,settingsPanel},
            {3,inventoryPanel}
        };
    }
    public void OpenUI(int i)
    {
        if (inPanel = panelDict.TryGetValue(i, out GameObject panel))
        {
            panel.SetActive(true);
            inPanel = true;
        }
    }
    public void CloseUI(int i)
    {

        if (panelDict.TryGetValue(i, out GameObject panel))
        {
            panel.SetActive(false);
            inPanel = false;
        }
    }
    public void CloseUI(GameObject panel)
    {
        panel.SetActive(false);
        inPanel = false;
    }
}
