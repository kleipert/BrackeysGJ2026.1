using UnityEngine;

namespace Misc
{
    public class LevelExit : MonoBehaviour
    {
        [SerializeField] private GameObject dialogueRunner;
        [SerializeField] private AudioClip beforeBoss;
        [SerializeField] private AudioClip afterBoss;
        [SerializeField] private bool beforeBossToggle;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            SoundManager.Instance.PlaySound(beforeBossToggle ? beforeBoss : afterBoss, transform, 0.3f);

            dialogueRunner.SetActive(true);
            dialogueRunner = null;
            //dialogueRunner.gameObject.SetActive(true);

            //StartCoroutine(LoadNextLevel());
        }

        /*private IEnumerator LoadNextLevel()
        {
            var oldIdx = SceneManager.GetActiveScene().buildIndex;
            var newIdx = oldIdx + 1;
            
            var uiSystem = GameObject.Find("UISystem");
            Destroy(uiSystem);
            var player = GameObject.Find("Player");
            Destroy(player);
            
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newIdx, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            SceneManager.UnloadSceneAsync(oldIdx);
            var newScene = SceneManager.GetSceneByBuildIndex(newIdx);
            SceneManager.SetActiveScene(newScene);
        }*/
    }
}
