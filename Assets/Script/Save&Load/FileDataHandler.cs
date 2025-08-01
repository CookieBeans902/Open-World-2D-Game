using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    [SerializeField] private bool useEncryption = false;
    private string encryptionCodeWord = "skibidi";
    public FileDataHandler(string dir, string file, bool encrypt)
    {
        this.dataDirPath = dir;
        this.dataFileName = file;
        this.useEncryption = encrypt;
    }
    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string loadedjson = "";
                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new(stream))
                    {
                        loadedjson = reader.ReadToEnd();
                    }
                }
                //Decrypting Data if it was encrypted
                if (useEncryption)
                {
                    loadedjson = EncryptDecrypt(loadedjson);
                }
                //Deserializing the data
                loadedData = JsonUtility.FromJson<GameData>(loadedjson);
            }
            catch (Exception e)
            {
                Debug.Log("Error while reading file" + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }
    public void SaveGame(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            //Serialize data into json
            string dataJson = JsonUtility.ToJson(gameData, true);
            //encrypt the data if necessary
            if (useEncryption)
            {
                dataJson = EncryptDecrypt(dataJson);
            }
            //writing the data
            using (FileStream fileStream = new(fullPath, FileMode.Create))
            {
                using (StreamWriter streamWriter = new(fileStream))
                {
                    streamWriter.Write(dataJson);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error while reading file" + fullPath + "\n" + e);
        }
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    } 
}
