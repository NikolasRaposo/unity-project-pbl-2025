using UnityEngine;
using UnityEngine.Events;

namespace Entity {
    public class HealthComponent : MonoBehaviour {
        [Header("Health Settings")]
        [SerializeField] private int maxHealth = 100;
        public int CurrentHealth { get; private set; }
        public int MaxHealth => maxHealth;
        public UnityEvent<int> onDamageTaken;
        public UnityEvent onDied;
        public static event System.Action<HealthComponent> OnAnyHealthComponentDied;
        private void Awake() {
            CurrentHealth = maxHealth;
        }
        public void TakeDamage(int damageAmount) {
            if (damageAmount < 0) return;
            CurrentHealth -= damageAmount;
            Debug.Log($"{gameObject.name} took {damageAmount} damage. Health is now {CurrentHealth}/{maxHealth}");
            onDamageTaken?.Invoke(damageAmount);
            if (CurrentHealth > 0) return;
            CurrentHealth = 0;
            Die();
        }
        private void Die() {
            Debug.Log($"{gameObject.name} has died.");
            onDied?.Invoke();
            OnAnyHealthComponentDied?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
