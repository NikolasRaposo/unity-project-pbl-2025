using UnityEngine;
using UnityEngine.Events;

namespace Entity {
    public class HealthComponent : MonoBehaviour {
        [Header("Health Settings")]
        [Tooltip("The maximum health of this entity.")]
        [SerializeField] private int maxHealth = 100;
        
        /// <summary>
        /// The current health of the entity.
        /// </summary>
        public int CurrentHealth { get; private set; }
        
        /// <summary>
        /// The maximum health of the entity.
        /// </summary>
        public int MaxHealth => maxHealth;

        /// <summary>
        /// Event fired when damage is taken. Passes (damageAmount, damageDealer).
        /// </summary>
        [Tooltip("Event fired when damage is taken. Passes (damageAmount, damageDealer).")]
        public UnityEvent<int, GameObject> onDamageTaken;
        
        /// <summary>
        /// Event fired when health reaches zero.
        /// </summary>
        [Tooltip("Event fired when health reaches zero.")]
        public UnityEvent onDied;

        /// <summary>
        /// Static event fired when any health component dies.
        /// Passes (deadComponent, killer).
        /// </summary>
        public static event System.Action<HealthComponent, GameObject> OnAnyHealthComponentDied;
        
        // Stores the last entity that dealt damage
        private GameObject _lastDamageDealer;

        private void Awake() {
            CurrentHealth = maxHealth;
        }

        /// <summary>
        /// Reduces the entity's health by a given amount.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to take.</param>
        /// <param name="damageDealer">The GameObject that dealt the damage.</param>
        public void TakeDamage(int damageAmount, GameObject damageDealer) {
            if (damageAmount < 0) return;

            _lastDamageDealer = damageDealer; // Store who hit us
            
            CurrentHealth -= damageAmount;
            Debug.Log($"{gameObject.name} took {damageAmount} damage from {damageDealer?.name}. Health is now {CurrentHealth}/{maxHealth}");
            
            onDamageTaken?.Invoke(damageAmount, damageDealer); // Pass dealer to event

            if (CurrentHealth > 0) return;
            
            CurrentHealth = 0;
            Die();
        }

        private void Die() {
            Debug.Log($"{gameObject.name} has died. Killed by {_lastDamageDealer?.name}.");
            onDied?.Invoke();
            
            // Pass the killer to the static event
            OnAnyHealthComponentDied?.Invoke(this, _lastDamageDealer);
            
            Destroy(gameObject);
        }
    }
}