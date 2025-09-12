using System.Collections.Generic;
using System.Linq;
using Entity;
using UnityEngine;
namespace Managers {
    public class ArenaManager : MonoBehaviour {
        [Header("Arena Setup")]
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private BoxCollider triggerVolume;

        private readonly List<HealthComponent> _activeEnemies = new List<HealthComponent>();
        private bool _combatStarted;
        private void OnEnable() {
            HealthComponent.OnAnyHealthComponentDied += HandleEnemyDied;
        }
        private void OnDisable() {
            HealthComponent.OnAnyHealthComponentDied -= HandleEnemyDied;
        }
        private void OnTriggerEnter(Collider other) {
            if (!_combatStarted && other.CompareTag("Player")) {
                StartCombat();
            }
        }
        private void StartCombat() {
            _combatStarted = true;
            triggerVolume.enabled = false;
            Debug.Log("<color=yellow>COMBATE INICIADO!</color>");

            foreach (GameObject enemyInstance in spawnPoints.Select(spawnPoint => Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation))) {
                if (enemyInstance.TryGetComponent<HealthComponent>(out HealthComponent enemyHealth)) {
                    _activeEnemies.Add(enemyHealth);
                }
            }
        }
        private void HandleEnemyDied(HealthComponent deadEnemy) {
            if (_activeEnemies.Contains(deadEnemy)) {
                _activeEnemies.Remove(deadEnemy);
            }
            if (_combatStarted && _activeEnemies.Count == 0) {
                EndCombat();
            }
        }
        private void EndCombat() {
            _combatStarted = false;
            Debug.Log("<color=green>ARENA CONCLUÍDA! Inimigos derrotados.</color>");
        }
    }
}