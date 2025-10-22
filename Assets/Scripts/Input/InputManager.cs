using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input {
    /// <summary>
    /// Manages the PlayerControls input asset and provides access to specific action maps
    /// for different players. Ensures only one instance of PlayerControls exists.
    /// </summary>
    public class InputManager : MonoBehaviour {
        /// <summary>
        /// Singleton instance of the InputManager.
        /// </summary>
        public static InputManager Instance { get; private set; }

        private PlayerControls _playerControls;

        private void Awake() {
            // Singleton pattern
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        
            // Initialize the controls
            _playerControls = new PlayerControls();
        }

        private void OnEnable() {
            // Enable all action maps
            _playerControls.Player.Enable();
            _playerControls.Player2.Enable();
            _playerControls.UI.Enable();
        }

        private void OnDisable() {
            // Disable all action maps (if controls exist)
            if (_playerControls != null) {
                _playerControls.Player.Disable();
                _playerControls.Player2.Disable();
                _playerControls.UI.Disable();
            }
        }

        /// <summary>
        /// Gets the appropriate Player action map based on the provided PlayerID.
        /// </summary>
        /// <param name="playerID">The ID of the player (Player1 or Player2).</param>
        /// <returns>The corresponding InputActionMap.</returns>
        public InputActionMap GetPlayerActionMap(PlayerID playerID) {
            switch (playerID) {
                case PlayerID.Player1:
                    return _playerControls.Player.Get(); // .Get() returns the InputActionMap
                case PlayerID.Player2:
                    // This assumes you named your new map "Player2" in the Input Action Asset
                    return _playerControls.Player2.Get(); // .Get() returns the InputActionMap
                default:
                    Debug.LogError($"No Action Map found for PlayerID: {playerID}");
                    return _playerControls.Player.Get(); // Fallback
            }
        }

        /// <summary>
        /// Gets the UI action map.
        /// </summary>
        /// <returns>The UIActions struct.</returns>
        public PlayerControls.UIActions GetUIActions() {
            return _playerControls.UI;
        }
    }
}