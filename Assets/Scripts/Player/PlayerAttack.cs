using UnityEngine;
using UnityEngine.InputSystem;
namespace Player {
    public class PlayerAttack : MonoBehaviour{
        [Header("Attack Settings")]
        [SerializeField] private float attackRange = 1.5f;
        [SerializeField] private float attackCooldown = 0.5f; 
        [SerializeField] private int attackDamage = 10;
        [SerializeField] private LayerMask enemyLayer; 

        [Header("Attack Point")]
        [SerializeField] private Transform attackPoint;

        private PlayerControls _playerControls;
        private float _lastAttackTime;
        private void Awake() {
            _playerControls = new PlayerControls();
        }
        private void OnEnable() {
            _playerControls.Player.Enable();
            _playerControls.Player.Attack.performed += HandleAttackPerformed;
        }
        private void OnDisable() {
            _playerControls.Player.Disable();
            _playerControls.Player.Attack.performed -= HandleAttackPerformed;
        }
        private void HandleAttackPerformed(InputAction.CallbackContext context) {
            if (!(Time.time >= _lastAttackTime + attackCooldown)) return;
            _lastAttackTime = Time.time;
            PerformAttack();
        }
        private void PerformAttack() {
            Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayer);
            foreach (Collider enemy in hitEnemies) {
                Debug.Log($"<color=red>Hit:</color> {enemy.name} by {attackDamage} of damage!");
                // Exemplo de como será no futuro:
                // if (enemy.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
                // {
                //     enemyHealth.TakeDamage(attackDamage);
                // }
            }
        }
        private void OnDrawGizmosSelected() {
            if (attackPoint == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}