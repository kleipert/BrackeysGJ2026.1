using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private DialogueRunner dialogueRunner;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private PlayerInput playerInput;
    
    private bool _isPaused;

    void Start()
    {
        SafeSetActive(pauseMenu, false);
        ApplyCursor(false);
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
        playerInput.SwitchCurrentActionMap("UI");
        Pause();
    }

    public void OnResume()
    {
        Resume();
    }

    private void HandleEscapePress()
    {
        if (dialogueRunner && dialogueRunner.IsDialogueRunning && !_isPaused)
            return;
        else
        {
            if (_isPaused) Resume();
            else Pause();
        }
    }

    private void Pause()
    {
        SafeSetActive(pauseMenu, true);
        _isPaused = true;

        Time.timeScale = 0f;
        StartCoroutine(UnlockCursorRealtime());
    }

    public void Resume()
    {
        SafeSetActive(pauseMenu, false);
        _isPaused = false;

        Time.timeScale = 1f;
        ApplyCursor(false);
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void Restart()
    {
        Resume();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
    
    public void SetVolume(float linearVolume)
    {
        audioMixer.SetFloat("volume", linearVolume);
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
    
}

