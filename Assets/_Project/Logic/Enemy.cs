using UnityEngine;

namespace TowerDefense
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _attackRange = 3f;
        [SerializeField] private float _attackDamage = 10f;
        [SerializeField] private float _health = 100f;

        private Transform[] _waypoints;
        private Tower _tower; // Ссылка на одну башню
        private int _currentWaypointIndex = 0;
        private bool _isAttacking = false;
        private bool _isMoving = true;
        private float _attackCooldown = 1f;
        private float _attackTimer = 0f;

        public delegate void EnemyDestroyed(Enemy enemy);
        public event EnemyDestroyed OnEnemyDestroyed;

        public void Initialize(Transform[] waypoints, Tower tower)
        {
            _waypoints = waypoints;
            _tower = tower; // Сохраняем ссылку на башню
        }

        private void Update()
        {
            if (_isMoving)
            {
                if (_waypoints.Length > 0)
                {
                    MoveAlongPath();
                }
                AttackTower(); // Проверка на атаку башни
            }
        }

        private void MoveAlongPath()
        {
            if (_waypoints.Length == 0 || _currentWaypointIndex >= _waypoints.Length) return;

            float distance = Vector3.Distance(transform.position, _waypoints[_currentWaypointIndex].position);

            if (distance < 0.1f)
            {
                _currentWaypointIndex++;
            }

            if (_currentWaypointIndex < _waypoints.Length)
            {
                transform.position = Vector3.MoveTowards(transform.position, _waypoints[_currentWaypointIndex].position, _moveSpeed * Time.deltaTime);
            }
        }

        private void AttackTower()
        {
            if (_tower == null) return;

            // Проверка расстояния до башни
            float distanceToTower = Vector3.Distance(transform.position, _tower.transform.position);

            if (distanceToTower <= _attackRange)
            {
                _isAttacking = true;
            }

            if (_isAttacking)
            {
                _attackTimer -= Time.deltaTime;

                if (_attackTimer <= 0f)
                {
                    _attackTimer = _attackCooldown;
                    _tower.TakeDamage(_attackDamage); // Наносим урон башне
                }
            }
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void StopMovement()
        {
            _isMoving = false;
        }

        private void OnDestroy()
        {
            OnEnemyDestroyed?.Invoke(this); // Вызываем событие уничтожения врага
        }
    }
}
