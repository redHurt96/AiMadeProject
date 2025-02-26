using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace TowerDefense
{
    public class GameRestarter : MonoBehaviour
    {
        [SerializeField] private Button _restartButton; // Кнопка перезапуска
        [SerializeField] private Button _nextLevelButton; // Кнопка следующего уровня (только для победы)
        [SerializeField] private EntryPoint _entryPoint; // Ссылка на EntryPoint для вызова ProceedToNextLevel

        private void Awake()
        {
            if (_restartButton == null)
            {
                _restartButton = GetComponentInChildren<Button>(true); // Ищем кнопку, даже если неактивна
                if (_restartButton == null)
                {
                    Debug.LogError("GameRestarter: No Restart Button found on this GameObject or its children!");
                }
            }

            // Проверяем, есть ли кнопка следующего уровня (только для VictoryScreen)
            if (_nextLevelButton != null)
            {
                _nextLevelButton.onClick.AddListener(NextLevel);
            }

            if (_restartButton != null)
            {
                _restartButton.onClick.AddListener(RestartGame);
            }

            if (_entryPoint == null)
            {
                _entryPoint = FindObjectOfType<EntryPoint>();
                if (_entryPoint == null)
                {
                    Debug.LogError("GameRestarter: No EntryPoint found in the scene!");
                }
            }
        }

        private void OnDestroy()
        {
            if (_restartButton != null)
            {
                _restartButton.onClick.RemoveListener(RestartGame);
            }
            if (_nextLevelButton != null)
            {
                _nextLevelButton.onClick.RemoveListener(NextLevel);
            }
        }

        private void RestartGame()
        {
            LevelManager.Instance.ResetToFirstLevel();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void NextLevel()
        {
            if (_entryPoint != null)
            {
                _entryPoint.ProceedToNextLevel();
            }
        }
    }
}