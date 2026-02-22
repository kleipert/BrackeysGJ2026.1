using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject playerUI;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image timerImage;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float fadeDuration = 4f;
    
    private bool _isPaused;
    private bool _isDead;

    public static PauseManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
    void Start()
    {
        SafeSetActive(pauseMenu, false);
        ApplyCursor(false);
        timerImage.fillAmount = 0;
    }

    void OnDisable()
    {
        if (_isPaused)
        {
            Time.timeScale = 1f;
            ApplyCursor(false);
            _isPaused = false;
        }
    }

    public void OnPause()
    {
        if (_isDead) return;
        
        playerInput.SwitchCurrentActionMap("UI");
        Pause();
    }

    public void OnResume()
    {
        if (_isDead) return;
        
        Resume();
    }

    /*private void HandleEscapePress()
    {
        if (dialogueRunner && dialogueRunner.IsDialogueRunning && !_isPaused)
            return;
        else
        {
            if (_isPaused) Resume();
            else Pause();
        }
    }*/

    private void Pause()
    {
        SafeSetActive(pauseMenu, true);
        _isPaused = true;

        Time.timeScale = 0f;
        AudioListener.pause = true;
        StartCoroutine(UnlockCursorRealtime());
    }

    public void Resume()
    {
        SafeSetActive(pauseMenu, false);
        _isPaused = false;

        Time.timeScale = 1f;
        AudioListener.pause = false;
        ApplyCursor(false);
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void Restart()
    {
        Resume();
        ReloadCurrentScene();
    }

    private void ReloadCurrentScene()
    {
        var uiSystem = GameObject.Find("UISystem");
        Destroy(uiSystem);
        var player = GameObject.Find("Player");
        Destroy(player);
            
        Scene currentScene = SceneManager.GetActiveScene();
        int buildIndex = currentScene.buildIndex;

        var statsScene = SceneManager.GetSceneByName("StatsScene");
        SceneManager.SetActiveScene(statsScene);

        StartCoroutine(UnloadSameScene(buildIndex));
        StartCoroutine(LoadSameScene(buildIndex));
    }

    private IEnumerator UnloadSameScene(int idx)
    {
        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(idx);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator LoadSameScene(int idx)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(idx, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
    
    public void SetVolume(float linearVolume)
    {
        audioMixer.SetFloat("Volume", linearVolume);
    }

    private IEnumerator UnlockCursorRealtime()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        ApplyCursor(true);
    }

    private void ApplyCursor(bool unlockedVisible)
    {
        if (unlockedVisible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

        }
    }

    private static void SafeSetActive(GameObject go, bool active)
    {
        if (go && go.activeSelf != active) go.SetActive(active);
    }

    public void IsDead()
    {
        _isDead =  true;
        
        playerInput.SwitchCurrentActionMap("UI");

        StartCoroutine(DeathSequence());
    }

    public void StartTimer(float duration)
    {
        timerImage.fillAmount = 1;

        StartCoroutine(FillToZero(duration));
    }
    
    private IEnumerator DeathSequence()
    {
        yield return FadeAlpha(0f, 1f, fadeDuration);
        
        SafeSetActive(playerUI, false);
        SafeSetActive(deathScreen, true);
        
        Time.timeScale = 0f;
        StartCoroutine(UnlockCursorRealtime());
    }

    private IEnumerator FadeAlpha(float from, float to, float duration)
    {
        if (!fadeImage) yield break;

        Color c = fadeImage.color;
        c.a = from;
        fadeImage.color = c;

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }

    private IEnumerator FillToZero(float duration)
    {
        float start = timerImage.fillAmount;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            timerImage.fillAmount = Mathf.Lerp(start, 0f, t / duration);
            yield return null;
        }

        timerImage.fillAmount = 0f;
    }
}

