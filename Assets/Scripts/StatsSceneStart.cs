using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatsSceneStart : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }
    
    
    IEnumerator LoadNextScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        var newScene = SceneManager.GetSceneByBuildIndex(1);
        SceneManager.SetActiveScene(newScene);
    }
}
