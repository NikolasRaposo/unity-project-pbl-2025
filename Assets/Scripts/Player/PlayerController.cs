using UnityEngine;

namespace Player {
    [RequireComponent(typeof(CharacterController), typeof(PlayerDash))]
    public class PlayerController : MonoBehaviour {
        [Header("Movement Settings")]
        public float moveSpeed = 8f;
        public float rotationSpeed = 15f;
        public LayerMask groundLayer;

        public Vector2 MoveInput { get; private set; }
        public Vector3 LookDirection { get; private set; }
        public bool IsInvincible {
            get {
                return _playerDash.IsInvincible;
            }
        }
        private PlayerControls _playerControls;
        private CharacterController _characterController;
        private PlayerDash _playerDash;
        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            _playerDash = GetComponent<PlayerDash>(); 
            _playerControls = new PlayerControls();
            _playerDash.Configure(_playerControls);
        }
        private void OnEnable() { _playerControls.Player.Enable(); }

        private void OnDisable() { _playerControls.Player.Disable(); }
        private void Update() {
            if (_playerDash.IsDashing) return;
            HandleInput();
            HandleMovement();
            HandleRotation();
        }

        private void HandleInput() {
            MoveInput = _playerControls.Player.Move.ReadValue<Vector2>();
            LookDirection = _playerControls.Player.Look.ReadValue<Vector2>();
        }

        private void HandleMovement() {
            Vector3 moveDirection = new Vector3(MoveInput.x, 0f, MoveInput.y);
            _characterController.Move(moveDirection*(moveSpeed*Time.deltaTime));
        }
        private void HandleRotation() {
            Ray ray = Camera.main.ScreenPointToRay(LookDirection);
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance: Mathf.Infinity, layerMask: groundLayer)) return;
            Vector3 lookDirection = hitInfo.point - transform.position;
            lookDirection.y = 0;
            if (!(lookDirection.sqrMagnitude > 0.01f)) return;
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            LookDirection = lookDirection.normalized;
        }
    }
}