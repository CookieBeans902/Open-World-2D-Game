using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManaging : MonoBehaviour
{
    public static SceneManaging Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LoadNextScene(int index)
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("before loading");
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }
}
