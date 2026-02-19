using System.Collections;
using UnityEngine;

public class PlayerSplit : MonoBehaviour
{
    [SerializeField] GameObject playerImage;
    [SerializeField] float splitTimeScale = 0.2f;
    [SerializeField] float splitDuration = 5f;

    private const float baseFixedDeltaTime = 0.02f;
    private bool _isActivated;
    private bool _corountineStarted;
    private GameObject _clone;

    private void Update()
    {
        if (_isActivated && !_corountineStarted) StartCoroutine(CountdownEvent());
    }

    public void OnSplit()
    {
        if (_isActivated) return;

        _isActivated = true;
        _clone = Instantiate(playerImage, transform.position, Quaternion.identity);
        
        PauseManager.Instance.StartTimer(splitDuration / 5);

        Time.timeScale = splitTimeScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;
    }

    IEnumerator CountdownEvent()
    {
        _corountineStarted = true;
        yield return new WaitForSeconds(splitDuration / 5);
        
        if (_clone) Destroy(_clone);
        
        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;
        _isActivated = false;
        _corountineStarted = false;
    }
}