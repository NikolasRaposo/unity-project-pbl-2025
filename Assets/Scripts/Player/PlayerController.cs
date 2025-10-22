using Input;
using UnityEngine;
using UnityEngine.InputSystem; // Required

namespace Player {
    [RequireComponent(typeof(CharacterController), typeof(PlayerDash))]
    public class PlayerController : MonoBehaviour {
        [Header("Player ID")]
        [Tooltip("The ID for this player (Player1 or Player2).")]
        [SerializeField] private PlayerID playerID;
        
        [Header("Movement Settings")]
        [Tooltip("Movement speed of the player.")]
        public float moveSpeed = 8f;
        [Tooltip("How fast the player rotates to face the movement direction.")]
        public float rotationSpeed = 15f;
        [Tooltip("Layer mask for the ground (used for rotation, though logic is changed).")]
        public LayerMask groundLayer; 

        public Vector2 MoveInput { get; private set; }
        public Vector3 LookDirection { get; private set; }
        public bool IsInvincible => _playerDash != null && _playerDash.IsInvincible;

        // --- Input Action Fields ---
        private InputActionMap _playerActionMap;
        private InputAction _moveAction;
        // ---

        private CharacterController _characterController;
        private PlayerDash _playerDash;

        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            _playerDash = GetComponent<PlayerDash>();
            
            // Get action map from the central InputManager
            _playerActionMap = InputManager.Instance.GetPlayerActionMap(playerID);
            
            // Find and store the specific actions we need
            _moveAction = _playerActionMap.FindAction("Move");
            
            // Configure dash with the correct action map
            _playerDash.Configure(_playerActionMap);
        }
        
        // OnEnable/OnDisable are no longer needed here, InputManager handles it.

        private void Update() {
            if (_playerDash != null && _playerDash.IsDashing) return;
            
            HandleInput();
            HandleMovement();
            HandleRotation();
        }

        private void HandleInput() {
            // Read value directly from the stored move action
            MoveInput = _moveAction.ReadValue<Vector2>();
        }

        private void HandleMovement() {
            Vector3 moveDirection = new Vector3(MoveInput.x, 0f, MoveInput.y);
            _characterController.Move(moveDirection * (moveSpeed * Time.deltaTime));
        }

        private void HandleRotation() {
            Vector3 moveDirection = new Vector3(MoveInput.x, 0f, MoveInput.y);

            if (moveDirection.sqrMagnitude > 0.01f) {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                LookDirection = moveDirection.normalized;
            }
        }
    }
}