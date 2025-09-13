using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;
namespace UI {
    [RequireComponent(typeof(Image))]
    public class DashCooldownUI : MonoBehaviour
    {
        private Image _cooldownImage;
        private PlayerDash _playerDash; 
        private void Awake() {
            _cooldownImage = GetComponent<Image>();
            _cooldownImage.fillAmount = 1;
        }
        private void Start() {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && player.TryGetComponent(out _playerDash)) {
                _playerDash.OnDashUsed += StartCooldownVisual;
            }
        }
        private void OnDestroy() {
            if (_playerDash != null) {
                _playerDash.OnDashUsed -= StartCooldownVisual;
            }
        }
        private void StartCooldownVisual(float cooldownDuration) {
            StartCoroutine(CooldownCoroutine(cooldownDuration));
        }
        private IEnumerator CooldownCoroutine(float cooldownDuration) {
            float timer = 0;
            _cooldownImage.fillAmount = 0; 
            while (timer < cooldownDuration) {
                timer += Time.deltaTime;
                _cooldownImage.fillAmount = 1 - (timer / cooldownDuration);
                yield return null;
            }
            _cooldownImage.fillAmount = 1; 
        }
    }
}
