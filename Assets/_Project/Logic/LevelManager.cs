using UnityEngine;
using System.Collections.Generic;

namespace TowerDefense
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelConfig> _levelConfigs; // Список конфигов уровней
        private int _currentLevelIndex = 0;

        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Сохраняем между сценами
            }
            else
            {
                Destroy(gameObject); // Уничтожаем дубликаты
            }
        }

        public LevelConfig GetCurrentLevelConfig()
        {
            if (_currentLevelIndex >= 0 && _currentLevelIndex < _levelConfigs.Count)
            {
                return _levelConfigs[_currentLevelIndex];
            }
            return null; // Или можно возвращать последний уровень, если индекс превышен
        }

        public bool HasNextLevel()
        {
            return _currentLevelIndex + 1 < _levelConfigs.Count;
        }

        public void ProceedToNextLevel()
        {
            if (HasNextLevel())
            {
                _currentLevelIndex++;
            }
        }

        public void ResetToFirstLevel()
        {
            _currentLevelIndex = 0;
        }
    }
}