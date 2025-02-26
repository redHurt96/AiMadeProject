using UnityEngine;

namespace TowerDefense
{
    [RequireComponent(typeof(TowerAttack))]
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private TowerAttack _towerAttack;

        public delegate void TowerDestroyed();
        public event TowerDestroyed OnTowerDestroyed;

        private void Start()
        {
            if (_towerAttack == null)
                _towerAttack = GetComponent<TowerAttack>();
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                DestroyTower();
            }
        }

        private void DestroyTower()
        {
            OnTowerDestroyed?.Invoke();
            gameObject.SetActive(false); // Деактивируем вместо уничтожения
            // Destroy(gameObject); // Убираем полное уничтожение
        }

        public void ResetHealth()
        {
            _health = 100f; // Сбрасываем здоровье для нового уровня
        }
    }
}