using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Save File Config")]
    [SerializeField] private string fileName;
    [SerializeField] bool encryptData;
    private FileDataHandler dataHandler;
    public static DataPersistenceManager Instance { get; set; }
    private GameData gameData;

    private List<IDataPersistence> dataPersistences;
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
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void NewGame()
    {
        gameData = new GameData();
    }
    public void NewSaveGame()
    {
        gameData = new GameData();
        SaveGame();
        Debug.Log("Destroyed previous save and creating a new one");
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.dataPersistences = FindAllDataPersistentObjects();
        LoadGame();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.LogWarning("No existing save data has been found, please create one using New Game");
            return;
        }
        foreach (IDataPersistence objects in dataPersistences)
        {
            objects.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        if (this.gameData == null)
        {
            Debug.LogWarning("No data found to save, please initilize a new game");
            return;
        }
        foreach (IDataPersistence objects in dataPersistences)
        {
            objects.SaveData(gameData);
        }
        dataHandler.SaveGame(gameData);
    }
    private List<IDataPersistence> FindAllDataPersistentObjects()
    {
        dataPersistences = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistence>().ToList();
        return dataPersistences;
    }
    void OnApplicationQuit()
    {
        SaveGame();
    }
    public bool HasGameData()
    {
        return gameData != null;
    }
}
