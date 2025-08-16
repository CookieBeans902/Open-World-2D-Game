using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioning : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] Transform location;
    [SerializeField] float radius = 0.5f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] int sceneIndexToTransition;
    [SerializeField] bool isInteracting;
    [SerializeField] GameObject popUp;
    private bool playerLeft;
    void Update()
    {
        isInteracting = Input.GetKeyDown(KeyCode.F);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(location.position, radius, layerMask);
        playerLeft = true;
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                popUp.SetActive(true);
                if (isInteracting)
                {
                    
                    DataPersistenceManager.Instance.SaveGame();
                    transition.SetTrigger("Exit");
                    StartCoroutine(SceneManaging.Instance.LoadNextScene(sceneIndexToTransition));
                }
                playerLeft = false;
            }
        }
        if (playerLeft) popUp.SetActive(false);
    }
}
