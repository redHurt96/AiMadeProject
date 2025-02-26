using UnityEngine;

namespace TowerDefense
{
    public class Tower : MonoBehaviour
    {
        [SerializeField] private float _health = 100f;
        [SerializeField] private TowerAttack _towerAttack; // Ссылка на компонент TowerAttack

        public delegate void TowerDestroyed();
        public event TowerDestroyed OnTowerDestroyed;

        private void Start()
        {
            // Убедитесь, что TowerAttack добавлен через инспектор или автоматически
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
            OnTowerDestroyed?.Invoke(); // Вызываем событие уничтожения башни
            Destroy(gameObject);
        }
    }
}