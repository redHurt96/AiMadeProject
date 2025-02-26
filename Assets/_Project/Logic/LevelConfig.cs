using UnityEngine;

namespace TowerDefense
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "TowerDefense/Level Config", order = 1)]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _enemyCount = 10; // Количество врагов
        [SerializeField] private float _spawnInterval = 2f; // Интервал спавна

        public int EnemyCount => _enemyCount;
        public float SpawnInterval => _spawnInterval;
    }
}