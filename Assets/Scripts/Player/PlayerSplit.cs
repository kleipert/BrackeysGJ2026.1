using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSplit : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform playerGroundCheck;
    [SerializeField] private float groundCheckDistance = 0.05f;

    [Header("Split")]
    [SerializeField] private GameObject playerImage;
    [SerializeField] private float splitTimeScale = 0.2f;
    [SerializeField] private float splitDuration = 5f;
    
    [Header("Sound")]
    [SerializeField] private AudioClip splitStart;
    [SerializeField] private AudioClip splitEnd;

    private const float baseFixedDeltaTime = 0.02f;

    private bool _isActivated;
    private bool _canSplit = true;
    private GameObject _clone;
    private Coroutine _countdownRoutine;

    private bool _needsAirborneBeforeResplit;

    private float SplitWait => splitDuration / 5f;

    public void OnSplit(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        
        if (_isActivated)
        {
            EndSplit();
            return;
        }

        if (!_canSplit) return;

        StartSplit();
    }

    private void StartSplit()
    {
        _isActivated = true;
        _canSplit = false;
        _needsAirborneBeforeResplit = true;

        _clone = Instantiate(playerImage, transform.position, Quaternion.identity);

        if (PauseManager.Instance != null)
            PauseManager.Instance.StartTimer(SplitWait);

        SoundManager.Instance.PlaySound(splitStart, transform, 0.3f);
        Time.timeScale = splitTimeScale;
        Time.fixedDeltaTime = baseFixedDeltaTime * Time.timeScale;

        _countdownRoutine = StartCoroutine(CountdownEvent());
    }

    private IEnumerator CountdownEvent()
    {
        yield return new WaitForSeconds(SplitWait);
        EndSplit();
    }

    private void EndSplit()
    {
        if (!_isActivated) return;

        if (_countdownRoutine != null)
        {
            StopCoroutine(_countdownRoutine);
            _countdownRoutine = null;
        }

        if (_clone != null)
        {
            Destroy(_clone);
            _clone = null;
        }

        SoundManager.Instance.PlaySound(splitEnd, transform, 0.3f);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = baseFixedDeltaTime;

        _isActivated = false;
    }

    private void Update()
    {
        bool grounded = IsPlayerOnGround();

        if (!_canSplit)
        {
            if (_needsAirborneBeforeResplit)
            {
                if (!grounded) _needsAirborneBeforeResplit = false;
            }
            else
            {
                if (grounded) _canSplit = true;
            }
        }
    }

    private bool IsPlayerOnGround()
    {
        if (!playerGroundCheck) return false;
        return Physics2D.Raycast(playerGroundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    }
}