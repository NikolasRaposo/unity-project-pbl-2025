using System;
using Entity;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy {
    [RequireComponent(typeof(NavMeshAgent), typeof(HealthComponent))]
    public class EnemyAI : MonoBehaviour {
        private enum State {
            Patrolling,
            Chasing,
            Attacking
        }
        [Header("AI Settings")]
        [SerializeField] private float detectionRadius = 10f; 
        [SerializeField] private float attackRadius = 2f;

        [Header("Attack Settings")]
        [SerializeField] private int attackDamage = 5;
        [SerializeField] private float attackCooldown = 1.5f;

        private State _currentState;
        private NavMeshAgent _agent;
        private Transform _playerTransform;
        private HealthComponent _playerHealth;
        private float _lastAttackTime;

        private void Awake() {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start() {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null) {
                _playerTransform = playerObject.transform;
                _playerHealth = playerObject.GetComponent<HealthComponent>();
            }
            _currentState = State.Patrolling;
        }
        private void Update() {
            if (!_playerTransform) return; 
            switch (_currentState) {
                case State.Patrolling:
                    Patrol();
                    break;
                case State.Chasing:
                    Chase();
                    break;
                case State.Attacking:
                    Attack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        private void Patrol() {
            if (Vector3.Distance(transform.position, _playerTransform.position) < detectionRadius) {
                _currentState = State.Chasing;
            }
        }
        private void Chase() {
            _agent.SetDestination(_playerTransform.position);
            if (Vector3.Distance(transform.position, _playerTransform.position) > detectionRadius) {
                _currentState = State.Patrolling;
            } else if (Vector3.Distance(transform.position, _playerTransform.position) < attackRadius) {
                _currentState = State.Attacking;
            }
        }
        private void Attack() {
            _agent.SetDestination(transform.position);
            if (Vector3.Distance(transform.position, _playerTransform.position) > attackRadius) {
                _currentState = State.Chasing;
            }
            if (!(Time.time >= _lastAttackTime + attackCooldown)) return;
            _lastAttackTime = Time.time;
            if (!_playerHealth) return;
            Debug.Log($"<color=orange>Inimigo atacou o jogador!</color>");
            _playerHealth.TakeDamage(attackDamage);
        }
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
    }
}