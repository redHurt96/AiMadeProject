using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TowerDefense
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private PathVisualizerScript _pathVisualizer;
        [SerializeField] private Tower _tower;
        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private float _spawnInterval = 2f;

        private bool _isTowerDestroyed = false;
        private List<Enemy> _spawnedEnemies = new List<Enemy>(); // Список для всех врагов

        private void Start()
        {
            _tower.OnTowerDestroyed += StopSpawningEnemies; // Подписываемся на событие уничтожения башни
            _tower.OnTowerDestroyed += StopAllEnemiesMovement; // Останавливаем всех врагов
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (!_isTowerDestroyed)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(_spawnInterval);
            }
        }

        private void SpawnEnemy()
        {
            Enemy newEnemy = Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);
            newEnemy.Initialize(_pathVisualizer.GetWaypoints(), _tower);
            _spawnedEnemies.Add(newEnemy); // Добавляем врага в список

            newEnemy.OnEnemyDestroyed += RemoveEnemyFromList; // Подписываемся на событие уничтожения врага
        }

        private void StopSpawningEnemies()
        {
            _isTowerDestroyed = true;
        }

        private void StopAllEnemiesMovement()
        {
            foreach (var enemy in _spawnedEnemies)
            {
                if (enemy != null)
                {
                    enemy.StopMovement(); // Останавливаем всех врагов
                }
            }
        }

        private void RemoveEnemyFromList(Enemy enemy)
        {
            if (_spawnedEnemies.Contains(enemy))
            {
                _spawnedEnemies.Remove(enemy); // Удаляем уничтоженного врага из списка
            }
        }
    }
}
