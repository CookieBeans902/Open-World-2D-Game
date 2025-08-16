using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    public static MainMenu Instance { get; set; }
    public int sceneIndex;
    [SerializeField] Button newGame;
    [SerializeField] Button loadGame;
    [SerializeField] Button options;
    [SerializeField] Button quit;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            loadGame.interactable = false;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnNewGame()
    {
        DisableMenuButtons();
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
    public void OnLoadGame()
    {
        DisableMenuButtons();
        DataPersistenceManager.Instance.LoadGame();
        AudioManager.Instance.InGameMusic();
    }
    void DisableMenuButtons()
    {
        newGame.interactable = false;
        loadGame.interactable = false;
        options.interactable = false;
        quit.interactable = false;
    }

    public void LoadData(GameData gameData)
    {
        this.sceneIndex = gameData.sceneIndex;
    }

    public void SaveData(GameData gameData)
    {
        //Nothing to save here
    }
}
