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
        [SerializeField] private GameObject _victoryScreen;
        [SerializeField] private GameObject _defeatScreen;

        private bool _isTowerDestroyed = false;
        private List<Enemy> _spawnedEnemies = new List<Enemy>();
        private int _enemiesSpawned = 0;
        private LevelManager _levelManager;

        private void Start()
        {
            _levelManager = LevelManager.Instance;
            if (_pathVisualizer == null || _tower == null || _enemyPrefab == null || _spawnPoint == null || 
                _levelManager == null || _victoryScreen == null || _defeatScreen == null)
            {
                Debug.LogError("EntryPoint: One or more required components are not assigned!");
                return;
            }

            _tower.OnTowerDestroyed += StopSpawningEnemies;
            _tower.OnTowerDestroyed += StopAllEnemiesMovement;
            _tower.OnTowerDestroyed += ShowDefeatScreen;
            StartCoroutine(SpawnEnemies());

            _victoryScreen.SetActive(false);
            _defeatScreen.SetActive(false);
        }

        private IEnumerator SpawnEnemies()
        {
            LevelConfig currentConfig = _levelManager.GetCurrentLevelConfig();
            if (currentConfig == null)
            {
                Debug.LogError("No valid level config found!");
                yield break;
            }

            while (!_isTowerDestroyed && _enemiesSpawned < currentConfig.EnemyCount)
            {
                SpawnEnemy();
                _enemiesSpawned++;
                yield return new WaitForSeconds(currentConfig.SpawnInterval);
            }
        }

        private void SpawnEnemy()
        {
            Enemy newEnemy = Instantiate(_enemyPrefab, _spawnPoint.position, Quaternion.identity);
            newEnemy.Initialize(_pathVisualizer.GetWaypoints(), _tower);
            _spawnedEnemies.Add(newEnemy);

            newEnemy.OnEnemyDestroyed += RemoveEnemyFromList;
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
                    enemy.StopMovement();
                }
            }
        }

        private void RemoveEnemyFromList(Enemy enemy)
        {
            if (_spawnedEnemies.Contains(enemy))
            {
                enemy.OnEnemyDestroyed -= RemoveEnemyFromList;
                _spawnedEnemies.Remove(enemy);
                CheckVictoryCondition();
            }
        }

        private void CheckVictoryCondition()
        {
            LevelConfig currentConfig = _levelManager.GetCurrentLevelConfig();
            if (!_isTowerDestroyed && _enemiesSpawned >= currentConfig.EnemyCount && _spawnedEnemies.Count == 0)
            {
                ShowVictoryScreen(); // Только показываем экран победы
            }
        }

        public void ProceedToNextLevel() // Делаем метод публичным для вызова из UI
        {
            if (_levelManager.HasNextLevel())
            {
                _levelManager.ProceedToNextLevel();
                ResetLevelState();
                StartCoroutine(SpawnEnemies());
            }
            else
            {
                ShowVictoryScreen(); // Если уровней больше нет, оставляем экран победы
            }
        }

        private void ResetLevelState()
        {
            _enemiesSpawned = 0;
            _isTowerDestroyed = false;
            foreach (var enemy in _spawnedEnemies)
            {
                if (enemy != null)
                {
                    Destroy(enemy.gameObject);
                }
            }
            _spawnedEnemies.Clear();
            _tower.gameObject.SetActive(true);
            _tower.ResetHealth();
            _victoryScreen.SetActive(false); // Скрываем экран победы
            _defeatScreen.SetActive(false); // На всякий случай скрываем экран поражения
        }

        private void ShowVictoryScreen()
        {
            _victoryScreen.SetActive(true);
        }

        private void ShowDefeatScreen()
        {
            _defeatScreen.SetActive(true);
        }
    }
}