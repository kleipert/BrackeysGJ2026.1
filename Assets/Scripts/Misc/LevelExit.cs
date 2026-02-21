using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class LevelExit : MonoBehaviour
    {
        private Scene _bridgeScene;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            StartCoroutine(LoadNextLevel());
        }

        private IEnumerator LoadNextLevel()
        {
            var oldIdx = SceneManager.GetActiveScene().buildIndex;
            var newIdx = oldIdx + 1;
            
            
            
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newIdx, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            
            GameObject.Find("EventSystem").SetActive(false);
            SceneManager.UnloadSceneAsync(oldIdx);
            var newScene = SceneManager.GetSceneByBuildIndex(newIdx);
            SceneManager.SetActiveScene(newScene);
        }
    }
}
