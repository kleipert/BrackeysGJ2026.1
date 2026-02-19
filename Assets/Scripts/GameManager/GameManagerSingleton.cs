using Player;
using UnityEngine;

namespace GameManager
{
    public class GameManagerSingleton : MonoBehaviour
    {
        public static GameManagerSingleton Instance;
        private PlayerMovement _playerMovement;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }
            Instance = this;
        }
        
        void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            _playerMovement = player.GetComponent<PlayerMovement>();
        }

        public int GetPlayerFacingDirection() => _playerMovement.FacingDirection;
        public bool GetPlayerIsGrounded() => _playerMovement.IsGrounded;
        public float GetPlayerXInWorld() => _playerMovement.transform.position.x;
        public Transform GetPlayerGroundCheckTransform() => _playerMovement._playerGroundCheck;



    }
}
