using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour, IDataPersistence
{
    public int sceneIndex;
    [SerializeField] Button newGame;
    [SerializeField] Button loadGame;
    [SerializeField] Button options;
    [SerializeField] Button quit;
    public void Start()
    {
        if (!DataPersistenceManager.Instance.HasGameData())
        {
            loadGame.interactable = false;
        }
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnNewGame()
    {
        DisableMenuButtons();
        DataPersistenceManager.Instance.NewGame();
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
    }
    public void LoadData(GameData gameData)
    {
        this.sceneIndex = gameData.sceneIndex;
    }

    public void SaveData(GameData gameData)
    {
        gameData.sceneIndex = this.sceneIndex;
    }
    public void OnLoadGame()
    {
        DisableMenuButtons();
        DataPersistenceManager.Instance.LoadGame();
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    }
    void DisableMenuButtons()
    {
        newGame.interactable = false;
        loadGame.interactable = false;
        options.interactable = false;
        quit.interactable = false;
    }
}
