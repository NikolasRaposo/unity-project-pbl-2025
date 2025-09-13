using Entity;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Slider))]
    public class PlayerHealthUI : MonoBehaviour {
        private Slider _healthSlider;
        private HealthComponent _playerHealthComponent;
        private void Awake() {
            _healthSlider = GetComponent<Slider>();
        }
        private void Start() {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player == null || !player.TryGetComponent(out _playerHealthComponent)) return;
            _healthSlider.maxValue = _playerHealthComponent.MaxHealth;
            _healthSlider.value = _playerHealthComponent.CurrentHealth;
            _playerHealthComponent.onDamageTaken.AddListener(UpdateHealthBar);
        }
        private void OnDestroy() {
            if(_playerHealthComponent != null) {
                _playerHealthComponent.onDamageTaken.RemoveListener(UpdateHealthBar);
            }
        }
        private void UpdateHealthBar(int damage) {
            _healthSlider.value = _playerHealthComponent.CurrentHealth;
        }
    }
}
