using UnityEngine;

namespace TowerDefense
{
    [RequireComponent(typeof(Tower))]
    public class TowerAttack : MonoBehaviour
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _attackRange = 10f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _damage = 10f;

        private Enemy _target;
        private float _attackTimer = 0f;

        public delegate void TargetDestroyed(Enemy enemy);
        public event TargetDestroyed OnTargetDestroyed;

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
                FindTarget();
            }
        }

        private void FindTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRange);
            System.Array.Sort(colliders, (x, y) => Vector3.Distance(transform.position, x.transform.position).CompareTo(Vector3.Distance(transform.position, y.transform.position)));

            foreach (var collider in colliders)
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    SetTarget(enemy);
                    break;
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
                _target.TakeDamage(_damage);
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
                    projectileScript.Initialize(_target.transform.position);
                }
            }
        }
    }
}
