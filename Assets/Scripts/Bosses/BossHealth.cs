using System;
using UnityEngine;

namespace Bosses
{
    public class BossHealth : MonoBehaviour
    {
        [SerializeField] private int _currentHealth = 1;
        public event EventHandler onDamageTaken;

        
        private void Update()
        {
            if(_currentHealth == 0)
                Destroy(gameObject);
        }

        public void SetCurrentHealth(int value)
        {
            _currentHealth = value;
            if (onDamageTaken != null) 
                onDamageTaken.Invoke(this, EventArgs.Empty);
        }

        public int GetCurrentHealth() => _currentHealth;
        
    }
}
