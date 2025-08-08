using UnityEngine;

public class Collectible : MonoBehaviour, IDataPersistence
{
    [SerializeField] CollectibleSO collectibleSO;
    [SerializeField] bool isCollected;
    [Header("Generate a GUID using the Context Menu")]
    [SerializeField] string guid;
    [SerializeField] LayerMask mask;
    [SerializeField] bool isInteractable;

    [ContextMenu("Generate a new Guid")]
    void GenerateGuid()
    {
        guid = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData gameData)
    {
        gameData.collectibleInformation.TryGetValue(guid, out isCollected);
        gameObject.SetActive(!isCollected);
    }

    public void SaveData(GameData gameData)
    {
        if (gameData.collectibleInformation.ContainsKey(guid))
        {
            gameData.collectibleInformation.Remove(guid);
        }
        gameData.collectibleInformation.Add(guid, isCollected);
    }

    void Awake()
    {
        gameObject.SetActive(true);
    }
    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f, mask);
        isInteractable = false;
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                isInteractable = true;
                Debug.Log("In Range of Player");
                break;
            }
        }
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            isCollected = true;
            Debug.Log("Collecting");
            gameObject.SetActive(false);
        }
    }
}
