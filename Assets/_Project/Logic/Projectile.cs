using UnityEngine;
using System.Collections;

namespace TowerDefense
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _lifetime = 5f; // Время жизни снаряда в секундах
        private Vector3 _targetPosition;

        public void Initialize(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
            StartCoroutine(DestroyAfterLifetime()); // Запускаем корутину при инициализации
        }

        private void Update()
        {
            // Движение к цели
            if (_targetPosition != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

                if (transform.position == _targetPosition)
                {
                    Destroy(gameObject);
                }
            }
        }

        private IEnumerator DestroyAfterLifetime()
        {
            yield return new WaitForSeconds(_lifetime); // Ждем указанное время
            Destroy(gameObject); // Уничтожаем снаряд
        }
    }
}