using System.Collections;
using Entity;
using UnityEngine;
using UnityEngine.UI;
namespace UI {
    public class EnemyHealthBar : MonoBehaviour {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private float timeToShowAfterDamage = 3f;

        private HealthComponent _enemyHealth;
        private Coroutine _hideCoroutine;
        private void Awake() {
            _enemyHealth = GetComponentInParent<HealthComponent>();
            healthSlider.gameObject.SetActive(false);
        }
        private void Start() {
            healthSlider.maxValue = _enemyHealth.MaxHealth;
            healthSlider.value = _enemyHealth.CurrentHealth;
            _enemyHealth.onDamageTaken.AddListener(HandleDamageTaken); // This is fine
        }
        private void OnDestroy() {
            if (_enemyHealth != null) {
                _enemyHealth.onDamageTaken.RemoveListener(HandleDamageTaken);
            }
        }
        private void LateUpdate() {
            transform.LookAt(transform.position + Camera.main!.transform.forward);
        }
        private void HandleDamageTaken(int damage, GameObject damageDealer) {
            healthSlider.gameObject.SetActive(true);
            healthSlider.value = _enemyHealth.CurrentHealth;
            if(_hideCoroutine != null) {
                StopCoroutine(_hideCoroutine);
            }
            _hideCoroutine = StartCoroutine(HideAfterDelay());
        }
        private IEnumerator HideAfterDelay() {
            yield return new WaitForSeconds(timeToShowAfterDamage);
            healthSlider.gameObject.SetActive(false);
        }
    }
}
