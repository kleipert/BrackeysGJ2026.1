using System.Collections;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;
    
    void Awake()
    {
        if (!dialogueRunner) dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    }
    
    void OnEnable() 
    {
        if (!dialogueRunner) { Debug.LogError("[FontChanger] Kein DialogueRunner."); return; }
        dialogueRunner.AddCommandHandler("Switch", () => Switch());
    }

    void Switch()
    {
        StartCoroutine(LoadNextScene());
    }
    
    IEnumerator LoadNextScene()
    {
        var oldIdx = SceneManager.GetActiveScene().buildIndex;
        var newIdx = oldIdx + 1;
            
        var uiSystem = GameObject.Find("UISystem");
        Destroy(uiSystem);
        var player = GameObject.Find("Player");
        Destroy(player);

        uiSystem = null;
        player = null;
        dialogueRunner = null;
            
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newIdx, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(oldIdx);
        var newScene = SceneManager.GetSceneByBuildIndex(newIdx);
        SceneManager.SetActiveScene(newScene);
    }
}