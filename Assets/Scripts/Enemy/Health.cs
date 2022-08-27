using System;

namespace Enemy
{
    public class Health
    {
        private readonly Action _death;
        private float _health;
        private bool _isDead;

        public Health(float maxHealth)
        {
            _health = maxHealth;
            _isDead = false;
        }

        public Health(float maxHealth, Action death)
        {
            _health = maxHealth;
            _death = death;
        }

        public bool IsDead()
        {
            return _isDead;
        }

        public void Damage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _death();
                _isDead = true;
            }
        }
    }
}