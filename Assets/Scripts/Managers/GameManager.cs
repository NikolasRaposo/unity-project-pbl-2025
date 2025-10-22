using Entity;
using Input;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// Adicionar a diretiva do Editor se quisermos parar o playmode no editor
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Managers {
    public class GameManager : MonoBehaviour {
        [Header("Game Over UI")]
        [SerializeField] private GameObject gameOverPanel;
        
        [Header("Pause Menu UI")]
        [Tooltip("The UI panel that appears when the game is paused.")]
        [SerializeField] private GameObject pauseMenuPanel; 

        private HealthComponent _playerHealth;
        private InputAction _cancelAction; 
        private bool _isPaused = false;

        private void Awake() {
            if (InputManager.Instance != null) {
                PlayerControls.UIActions uiActions = InputManager.Instance.GetUIActions();
                _cancelAction = uiActions.Cancel;
            }
            else {
                Debug.LogError("InputManager.Instance is null. Ensure an InputManager exists in the scene and its Script Execution Order is set correctly.");
            }
        }

        private void Start() {
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(false);
            }
            if (pauseMenuPanel != null) {
                pauseMenuPanel.SetActive(false);
            }

            Time.timeScale = 1f;
            _isPaused = false;

            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.TryGetComponent(out _playerHealth)) {
                _playerHealth.onDied.AddListener(HandlePlayerDeath);
            }

            if (_cancelAction != null) {
                _cancelAction.performed += HandlePauseInput;
            }
        }

        private void OnDestroy() {
            if (_playerHealth != null) {
                _playerHealth.onDied.RemoveListener(HandlePlayerDeath);
            }
            
            if (_cancelAction != null) {
                _cancelAction.performed -= HandlePauseInput;
            }
        }

        /// <summary>
        /// Handles the pause input event (e.g., Escape key press).
        /// </summary>
        private void HandlePauseInput(InputAction.CallbackContext context) {
            if (gameOverPanel != null && gameOverPanel.activeInHierarchy) {
                return;
            }
            TogglePause();
        }

        /// <summary>
        /// Toggles the game's pause state.
        /// </summary>
        public void TogglePause() {
            _isPaused = !_isPaused;
            if (_isPaused) {
                PauseGame();
            } else {
                ResumeGame();
            }
        }

        private void PauseGame() {
            _isPaused = true;
            Time.timeScale = 0f;
            if (pauseMenuPanel != null) {
                pauseMenuPanel.SetActive(true);
            }
            
            if (InputManager.Instance != null) {
                InputManager.Instance.GetPlayerActionMap(PlayerID.Player1).Disable();
                InputManager.Instance.GetPlayerActionMap(PlayerID.Player2).Disable();
            }
        }

        /// <summary>
        /// Resumes the game from a paused state.
        /// Public to be callable from a UI Button.
        /// </summary>
        public void ResumeGame() {
            _isPaused = false;
            Time.timeScale = 1f;
            if (pauseMenuPanel != null) {
                pauseMenuPanel.SetActive(false);
            }

            if (InputManager.Instance != null) {
                InputManager.Instance.GetPlayerActionMap(PlayerID.Player1).Enable();
                InputManager.Instance.GetPlayerActionMap(PlayerID.Player2).Enable();
            }
        }

        private void HandlePlayerDeath() {
            Debug.Log("Game Over!");
            if (gameOverPanel != null) {
                gameOverPanel.SetActive(true);
            }
            Time.timeScale = 0f;

            if (InputManager.Instance != null) {
                InputManager.Instance.GetPlayerActionMap(PlayerID.Player1).Disable();
                InputManager.Instance.GetPlayerActionMap(PlayerID.Player2).Disable();
            }
        }

        /// <summary>
        /// Restarts the currently active scene.
        /// Public to be callable from a UI Button.
        /// </summary>
        public void RestartScene() {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        /// <summary>
        /// Loads the main menu scene.
        /// Public to be callable from a UI Button.
        /// </summary>
        /// <param name="sceneName">The name of the main menu scene to load.</param>
        public void LoadMainMenu(string sceneName) {
            Time.timeScale = 1f;
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Quits the application.
        /// NOTE: This only works in a built application (PC, Mac, Linux).
        /// In the Unity Editor, it will stop the Play mode.
        /// </summary>
        public void QuitGame() {
            Debug.Log("Quitting game...");
            
            // If we are running in the Unity Editor
            #if UNITY_EDITOR
                // Stop playing the scene
                EditorApplication.isPlaying = false;
            #else
                // Quit the application
                Application.Quit();
            #endif
        }
    }
}