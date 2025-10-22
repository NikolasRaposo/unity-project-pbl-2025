using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    // DashMode enum...
    public enum DashMode {
        ToMovementDirection,
        ToMouseCursor, 
    }
    
    [RequireComponent(typeof(PlayerController))]
    public class PlayerDash : MonoBehaviour {
        [Header("Dash Mode")]
        [Tooltip("How to determine the dash direction.")]
        [SerializeField] private DashMode dashMode = DashMode.ToMovementDirection;

        [Header("Dash Settings")]
        // ... fields ...
        [SerializeField] private float dashSpeed = 25f;
        [SerializeField] private float dashDuration = 0.15f;
        [SerializeField] private float dashCooldown = 1f;
        
        public bool IsDashing { get; private set; }
        public bool IsInvincible { get; private set; }
        
        private float _lastDashTime;
        private PlayerController _playerController;
        private CharacterController _characterController;
        
        // --- Input Action Field ---
        private InputAction _dashAction;
        // ---
        
        public event System.Action<float> OnDashUsed;

        private void Awake() {
            _playerController = GetComponent<PlayerController>();
            _characterController = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Configures the dash component with the correct player's input action map.
        /// Called by PlayerController.
        /// </summary>
        /// <param name="playerActionMap">The action map for this player.</param>
        public void Configure(InputActionMap playerActionMap) {
            _dashAction = playerActionMap.FindAction("Dash");
            
            if (_dashAction != null) {
                _dashAction.performed += HandleDashPerformed;
            }
        }

        private void OnDestroy() {
            if (_dashAction != null) {
                _dashAction.performed -= HandleDashPerformed;
            }
        }

        private void HandleDashPerformed(InputAction.CallbackContext context) {
            if (!(Time.time >= _lastDashTime + dashCooldown)) return;
            
            _lastDashTime = Time.time;
            OnDashUsed?.Invoke(dashCooldown);
            StartCoroutine(PerformDash());
        }

        // ... PerformDash() coroutine remains the same ...
        private IEnumerator PerformDash() {
            IsDashing = true;
            IsInvincible = true;
            float startTime = Time.time;
            Vector3 dashDirection;

            switch (dashMode) {
                case DashMode.ToMovementDirection:
                    Vector2 moveInput = _playerController.MoveInput;
                    dashDirection = moveInput.sqrMagnitude > 0.1f
                        ? new Vector3(moveInput.x, 0f, moveInput.y).normalized
                        : transform.forward;
                    break;
                case DashMode.ToMouseCursor:
                    dashDirection = _playerController.LookDirection;
                    if (dashDirection.sqrMagnitude < 0.1f) {
                        dashDirection = transform.forward;
                    }
                    break;
                default:
                    dashDirection = transform.forward;
                    break;
            }

            while (Time.time < startTime + dashDuration) {
                _characterController.Move(dashDirection * (dashSpeed * Time.deltaTime));
                yield return null;
            }
            IsDashing = false;
            IsInvincible = false;
        }
    }
}