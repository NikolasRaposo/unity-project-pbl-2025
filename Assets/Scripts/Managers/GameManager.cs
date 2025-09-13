using Entity;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers {
    public class GameManager : MonoBehaviour {
        [Header("Game Over UI")]
        [SerializeField] private GameObject gameOverPanel; 
        private HealthComponent _playerHealth;
        private void Start() {
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(false);
            }
            Time.timeScale = 1f;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.TryGetComponent(out _playerHealth)) {
                _playerHealth.onDied.AddListener(HandlePlayerDeath);
            }
        }
        private void OnDestroy() {
            if (_playerHealth != null) {
                _playerHealth.onDied.RemoveListener(HandlePlayerDeath);
            }
        }
        private void HandlePlayerDeath() {
            Debug.Log("Game Over!");
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(true);
            }
            Time.timeScale = 0f;
        }
        public void RestartScene() {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
