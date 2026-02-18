using GameManager;
using UnityEngine;

namespace Bosses
{
    public class BrainBoss : MonoBehaviour
    {
        [SerializeField] private int _attacksBeforeWeak = 3;
        [SerializeField] private int _attacksLeft;
        [SerializeField] private int _healthTotal = 3;
        [SerializeField] private int _healthCurrent;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private GameObject _lightningPrefab;

        
        private bool _isIdle = true;
        [SerializeField] private float _idleTimeBetweenAttacks = 5f;
        [SerializeField] private float _idleTimeBetweenAttacksLeft;
        [SerializeField] private Transform _idlePositionLeft;
        [SerializeField] private Transform _idlePositionRight;
        [SerializeField] private Transform _idlePositionMiddle;
        [SerializeField] private bool _isMovingLeft = true;
        
        
        private bool _isMovingToRecover = false;
        
        
        private bool _isRecovering = false;
        [SerializeField] private float _recoveryTime = 5f;
        [SerializeField] private float _recoveryTimeLeft;
        [SerializeField] private Transform _recoveryPosition;
        
        
        private bool _isMovingFromRecover = false;


        [SerializeField] private bool _playerFound = false;
        private Animator _anim;
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsRecovering = Animator.StringToHash("isRecovering");


        void Start()
        {
            _anim = GetComponent<Animator>();
            _healthCurrent = _healthTotal;
            _attacksLeft = _attacksBeforeWeak;
            _idleTimeBetweenAttacksLeft = _idleTimeBetweenAttacks;
            _recoveryTimeLeft = _recoveryTime;
            
            StartIdleAnimation();
        }

        void Update()
        {
            if (!_playerFound) return;

            if (_isIdle)
                Idle();
            else if (_isMovingToRecover)
                MoveToRecover();
            else if(_isRecovering)
                Recover();
            else if(_isMovingFromRecover)
                MoveFromRecover();
        }

        private void Idle()
        {
            if (_isMovingLeft)
            {
                transform.Translate(Vector3.left * (_movementSpeed * Time.deltaTime));
                if (Vector3.Distance(_idlePositionLeft.position, transform.position) <= 0.1f)
                {
                    _isMovingLeft = false;
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                }
                    
            }
            else
            {
                transform.Translate(Vector3.right * (_movementSpeed * Time.deltaTime));
                if (Vector3.Distance(_idlePositionRight.position, transform.position) <= 0.1f)
                {
                    _isMovingLeft = true;
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                }
            }
            
            _idleTimeBetweenAttacksLeft -= Time.deltaTime;
            if (_idleTimeBetweenAttacksLeft <= 0f)
            {
                if (_attacksLeft >= 1)
                {
                    Attack();
                    _idleTimeBetweenAttacksLeft = _idleTimeBetweenAttacks;
                }
            }
        }

        private void Attack()
        {
            _isIdle = false;
            StartAttackAnimation();
            _attacksLeft--;
            Debug.Log($"Performed Attack! Attacks left: {_attacksLeft}");
        }

        private void AttackDone()
        {

            var targetPos = GameManagerSingleton.Instance.GetPlayerGroundCheckTransform().position;
            targetPos.y -= 0.75f; 
            var lightning = Instantiate(_lightningPrefab, targetPos, Quaternion.identity);
            lightning.GetComponent<LightningImpact>().SetLightningMode(_healthCurrent);
            _isIdle = true;
            StartIdleAnimation();
            if (_attacksLeft == 0)
            {
                _isIdle = false;
                _isMovingToRecover = true;
            }
        }

        private void MoveToRecover()
        {
            var correctPosX = Mathf.Abs(_recoveryPosition.position.x - transform.position.x) <= 0.1f;
            var correctPosY = Mathf.Abs(_recoveryPosition.position.y - transform.position.y) <= 0.1f;
            
            // Move to correct X
            if(!correctPosX && transform.position.x > _recoveryPosition.position.x)
                transform.Translate(Vector3.left * (_movementSpeed * Time.deltaTime));
            else if(!correctPosX && transform.position.x < _recoveryPosition.position.x)
                transform.Translate(Vector3.right * (_movementSpeed * Time.deltaTime));
            
            // Move to correct Y
            if(correctPosX && !correctPosY)
                transform.Translate(Vector3.down * (_movementSpeed * Time.deltaTime));

            if (correctPosX && correctPosY)
            {
                _isMovingToRecover = false;
                _isRecovering = true;
                StartRecoverAnimation();
            }
        }

        private void Recover()
        {
            _recoveryTimeLeft -= Time.deltaTime;
            if (_recoveryTimeLeft <= 0f)
            {
                _recoveryTimeLeft = _recoveryTime;
                _isRecovering = false;
                StartIdleAnimation();
                _attacksLeft = _attacksBeforeWeak;
                _isMovingFromRecover = true;
            }
        }

        private void MoveFromRecover()
        {
            var correctPosY = Mathf.Abs(_idlePositionMiddle.position.y - transform.position.y) <= 0.1f;
            
            if(!correctPosY)
                transform.Translate(Vector3.up * (_movementSpeed * Time.deltaTime));

            if (correctPosY)
            {
                _isMovingFromRecover = false;
                _isIdle = true;
                StartIdleAnimation();
            }
        }

        private void StartIdleAnimation()
        {
            _anim.SetBool(IsIdle, true);
            _anim.SetBool(IsAttacking, false);
            _anim.SetBool(IsRecovering, false);
        }
        
        private void StartAttackAnimation()
        {
            _anim.SetBool(IsIdle, false);
            _anim.SetBool(IsAttacking, true);
            _anim.SetBool(IsRecovering, false);
        }
        
        private void StartRecoverAnimation()
        {
            _anim.SetBool(IsIdle, false);
            _anim.SetBool(IsAttacking, false);
            _anim.SetBool(IsRecovering, true);
        }
    }
}
