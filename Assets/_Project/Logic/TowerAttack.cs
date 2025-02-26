using UnityEngine;

namespace TowerDefense
{
    public class TowerAttack : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _attackRange = 10f; // Радиус атаки
        [SerializeField] private float _attackCooldown = 1f; // Задержка между атаками
        private float _attackTimer = 0f;

        private Enemy _target; // Цель для атаки

        private void Update()
        {
            if (_target != null)
            {
                _attackTimer -= Time.deltaTime;

                if (_attackTimer <= 0f)
                {
                    Attack();
                    _attackTimer = _attackCooldown;
                }
            }
            else
            {
                FindTarget(); // Ищем цель, если она не назначена
            }
        }

        private void FindTarget()
        {
            // Находим ближайшего врага в радиусе атаки, используя transform.position текущего объекта
            Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRange);
            foreach (var collider in colliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    SetTarget(enemy); // Устанавливаем цель, если враг в радиусе
                    break; // Останавливаемся на первом враге
                }
            }
        }

        public void SetTarget(Enemy target)
        {
            _target = target;
        }

        private void Attack()
        {
            if (_target != null)
            {
                FireProjectile();
                _target.TakeDamage(10f); // Наносим урон цели
            }
        }

        private void FireProjectile()
        {
            if (_projectilePrefab != null && _firePoint != null)
            {
                GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                if (projectileScript != null)
                {
                    // Передаем цель снаряду
                    projectileScript.Initialize(_target.transform.position);
                }
            }
        }
    }
}
