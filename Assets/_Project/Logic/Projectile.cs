using UnityEngine;

namespace TowerDefense
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 10f;
        private Vector3 _targetPosition; // Точка назначения

        public void Initialize(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        private void Update()
        {
            // Двигаем снаряд к целевой точке
            if (_targetPosition != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

                // Если снаряд достиг цели, уничтожаем его
                if (transform.position == _targetPosition)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}