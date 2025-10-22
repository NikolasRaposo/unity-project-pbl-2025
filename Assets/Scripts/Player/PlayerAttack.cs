using Entity;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class PlayerAttack : MonoBehaviour {
        [Header("Player ID")]
        [Tooltip("The ID for this player (Player1 or Player2).")]
        [SerializeField] private PlayerID playerID;

        [Header("Attack Settings")]
        [Tooltip("The range of the attack sphere.")]
        [SerializeField] private float attackRange = 1.5f;
        [Tooltip("Time between attacks.")]
        [SerializeField] private float attackCooldown = 0.5f;
        [Tooltip("Damage dealt per attack.")]
        [SerializeField] private int attackDamage = 10;
        [Tooltip("Which layers to hit.")]
        [SerializeField] private LayerMask enemyLayer;

        [Header("Attack Point")]
        [Tooltip("The transform from which the attack sphere originates.")]
        [SerializeField] private Transform attackPoint;

        // --- Input Action Field ---
        private InputAction _attackAction;
        // ---

        private float _lastAttackTime;

        private void Awake() {
            // Get action map and find the "Attack" action
            InputActionMap playerActionMap = InputManager.Instance.GetPlayerActionMap(playerID);
            _attackAction = playerActionMap.FindAction("Attack");
        }

        private void OnEnable() {
            // Subscribe to the attack action
            if (_attackAction != null) {
                _attackAction.performed += HandleAttackPerformed;
            }
        }

        private void OnDisable() {
            // Unsubscribe
            if (_attackAction != null) {
                _attackAction.performed -= HandleAttackPerformed;
            }
        }

        private void HandleAttackPerformed(InputAction.CallbackContext context) {
            if (!(Time.time >= _lastAttackTime + attackCooldown)) return;
            
            _lastAttackTime = Time.time;
            PerformAttack();
        }

        private void PerformAttack() {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider enemy in hitEnemies) {
                if (enemy.TryGetComponent(out HealthComponent enemyHealth)) {
                    enemyHealth.TakeDamage(attackDamage, this.gameObject);
                }
            }
        }

        private void OnDrawGizmosSelected() {
            if (attackPoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}