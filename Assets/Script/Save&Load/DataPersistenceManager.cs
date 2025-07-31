using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

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
    }
    void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName,encryptData);
        dataPersistences = FindAllDataPersistentObjects();
        LoadGame();
    }
    public void NewGame()
    {
        gameData = new GameData();
        SaveGame();
        Debug.Log("Destroyed previous save and creating a new one");
        LoadGame();
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.Log("No existing save data has been found, creating a new game");
            NewGame();
        }
        foreach (IDataPersistence objects in dataPersistences)
        {
            objects.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
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
}
