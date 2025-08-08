using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour, IDataPersistence
{
    [SerializeField] Animator transition;
    [SerializeField] int sceneToLoad;
    [SerializeField] float transitionTime = 2f;
    [SerializeField] bool fromMainMenu;
    [SerializeField] GameObject loadingObject;
    void Start()
    {
        loadingObject.SetActive(false);
    }

    public IEnumerator LoadGameFromMainMenu()
    {
        sceneToLoad = MainMenu.Instance.sceneIndex;
        loadingObject.SetActive(true);
        transition.SetTrigger("Exit");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
    }
    public void StartExistTransition()
    {
        StartCoroutine(LoadGameFromMainMenu());
    }

    public void LoadData(GameData gameData)
    {
        //Nothing to load here. just saving the sceneIndexes respectively
    }

    public void SaveData(GameData gameData)
    {
        gameData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
}
