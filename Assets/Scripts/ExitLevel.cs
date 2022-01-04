using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if(currentSceneIndex == 3)
            {
                nextSceneIndex = 5;
            }

            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            FindObjectOfType<ScenePersist>().ResetScenePersist();
            SceneManager.LoadScene(nextSceneIndex);
        }
        
    }
}
