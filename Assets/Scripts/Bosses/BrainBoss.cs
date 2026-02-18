using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bosses
{
    public class BrainBoss : MonoBehaviour
    {
        [SerializeField] private int _attacksBeforeWeakMax = 6;
        [SerializeField] private int _attacksLeft;
        [SerializeField] private int _healthTotal = 3;
        [SerializeField] private int _healthCurrent;
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _movementSpeedMax = 5f;
        [SerializeField] private GameObject _lightningPrefab;
        [SerializeField] private Transform[] _lightningPositionsGround;
        [SerializeField] private Transform[] _lightningPositionsPlatforms;
        private Transform[] _pickedPositionsGround;
        private Transform[] _pickedPositionsPlatforms;

        
        private bool _isIdle = true;
        [SerializeField] private float _idleTimeBetweenAttacksMin = 5f;
        [SerializeField] private float _idleTimeBetweenAttacks;
        [SerializeField] private float _idleTimeBetweenAttacksLeft;
        [SerializeField] private Transform _idlePositionLeft;
        [SerializeField] private Transform _idlePositionRight;
        [SerializeField] private Transform _idlePositionMiddle;
        [SerializeField] private bool _isMovingLeft = true;
        
        
        [SerializeField] private float _recoveryTime = 5f;
        [SerializeField] private float _recoveryTimeLeft;
        [SerializeField] private Transform _recoveryPosition;
        
        
        [SerializeField] private bool _isMovingToRecover = false;
        [SerializeField] private bool _isRecovering = false;
        [SerializeField] private bool _isMovingFromRecover = false;
        [SerializeField] private bool _isInIdleMiddle = true;
        [SerializeField] private bool _isInRecoveryZone = false;
        


        [SerializeField] private bool _playerFound = false;
        private Animator _anim;
        private static readonly int IsIdle = Animator.StringToHash("isIdle");
        private static readonly int IsAttacking = Animator.StringToHash("isAttacking");
        private static readonly int IsRecovering = Animator.StringToHash("isRecovering");


        void Start()
        {
            _anim = GetComponent<Animator>();
            _healthCurrent = _healthTotal;
            
            _attacksLeft = _attacksBeforeWeakMax - _healthCurrent;
            _movementSpeed = _movementSpeedMax - _healthCurrent;
            _idleTimeBetweenAttacks = _idleTimeBetweenAttacksMin + _healthCurrent;
            
            
            _idleTimeBetweenAttacksLeft = _idleTimeBetweenAttacks;

            _recoveryTimeLeft = _recoveryTime;
            _pickedPositionsGround = new Transform[8];
            _pickedPositionsPlatforms = new Transform[3];
            
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.name)
            {
                case "BossIdleLeft":
                    _isMovingLeft = false;
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                    break;
                case "BossIdleMiddle":
                    _isInIdleMiddle = true;
                    break;
                case "BossIdleRight":
                    _isMovingLeft = true;
                    transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                    break;
                case "BossRecovery":
                    _isInRecoveryZone = true;
                    break;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            switch (other.gameObject.name)
            {
                case "BossIdleMiddle":
                    _isInIdleMiddle = false;
                    break;
                case "BossRecovery":
                    _isInRecoveryZone = false;
                    break;
            }
        }

        private void Idle()
        {
            if (_isMovingLeft)
            {
                transform.Translate(Vector3.left * (_movementSpeed * Time.deltaTime));
            }
            else
            {
                transform.Translate(Vector3.right * (_movementSpeed * Time.deltaTime));
            }
            
            _idleTimeBetweenAttacksLeft -= Time.deltaTime;
            if (_idleTimeBetweenAttacksLeft <= 0f)
            {
                if (_attacksLeft >= 1)
                {
                    PickLightningPositions();
                    Attack();
                    _idleTimeBetweenAttacksLeft = _idleTimeBetweenAttacks;
                }
            }
        }

        private void PickLightningPositions()
        {
            List<int> indices = new List<int>();
            int index;
            var itemsFromLeft = 3;
            var itemsFromMiddle = 2;
            var itemsFromRight = 3;
            var itemsFromPlatform = 3;
            var random = new System.Random();
            
            for (int i = 0; i < itemsFromLeft; i++)
            {
                do 
                    index = random.Next(0, 3);
                while(indices.Contains(index));
                    indices.Add(index);
            }
            
            for (int i = 0; i < itemsFromMiddle; i++)
            {
                do 
                    index = random.Next(4, 6);
                while(indices.Contains(index));
                indices.Add(index);
            }
            
            for (int i = 0; i < itemsFromRight; i++)
            {
                do 
                    index = random.Next(7, 11);
                while(indices.Contains(index));
                    indices.Add(index);
            }

            for (int i = 0; i < _pickedPositionsGround.Length; i++)
            {
                _pickedPositionsGround[i] = _lightningPositionsGround[indices[i]];
            }
            indices.Clear();
            
            for (int i = 0; i < itemsFromPlatform; i++)
            {
                do 
                    index = random.Next(0, 3);
                while(indices.Contains(index));
                    indices.Add(index);
            }

            for (int i = 0; i < _pickedPositionsPlatforms.Length; i++)
            {
                _pickedPositionsPlatforms[i] = _lightningPositionsPlatforms[indices[i]];
            }
        }

        private void Attack()
        {
            _isIdle = false;
            StartAttackAnimation();
            _attacksLeft--;
        }

        private void AttackDone()
        {
            foreach (var trans in _pickedPositionsGround)
            {
                var lightning = Instantiate(_lightningPrefab, trans.position, Quaternion.identity);
                lightning.GetComponent<LightningImpact>().SetLightningMode(_healthCurrent);
            }
            
            foreach (var trans in _pickedPositionsPlatforms)
            {
                var lightning = Instantiate(_lightningPrefab, trans.position, Quaternion.identity);
                lightning.GetComponent<LightningImpact>().SetLightningMode(_healthCurrent);
            }
            _isIdle = true;
            StartIdleAnimation();
            if (_attacksLeft == 0)
            {
                _isIdle = false;
                _isMovingToRecover = true;
            }
        }

        private bool recoveryXReached = false;
        private bool recoveryYReached = false;
        

        private void MoveToRecover()
        {
            if(recoveryXReached == false)
                recoveryXReached = _isInIdleMiddle;
            
            if(recoveryYReached == false)
                recoveryYReached = _isInRecoveryZone;
            
            if(!recoveryXReached && transform.position.x > _recoveryPosition.position.x)
                transform.Translate(Vector3.left * (_movementSpeed * Time.deltaTime));
            else if(!recoveryXReached && transform.position.x < _recoveryPosition.position.x)
                transform.Translate(Vector3.right * (_movementSpeed * Time.deltaTime));
            
            if(recoveryXReached && !recoveryYReached)
                transform.Translate(Vector3.down * (_movementSpeed * Time.deltaTime));

            if (recoveryXReached && recoveryYReached)
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
                
                _attacksLeft = _attacksBeforeWeakMax - _healthCurrent;
                _movementSpeed = _movementSpeedMax - _healthCurrent;
                _idleTimeBetweenAttacks = _idleTimeBetweenAttacksMin + _healthCurrent;
                
                _idleTimeBetweenAttacksLeft = _idleTimeBetweenAttacks;


                recoveryXReached = false;
                recoveryYReached = false;
                _isMovingFromRecover = true;
            }
        }

        private void MoveFromRecover()
        {
            var correctPosY = _isInIdleMiddle;
            
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
