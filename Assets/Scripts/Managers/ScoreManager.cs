using System.Collections.Generic;
using Entity;
using Player;
using UnityEngine;

/// <summary>
/// Manages player scores, tracking kills and notifying the UI.
/// </summary>
public class ScoreManager : MonoBehaviour {
    /// <summary>
    /// Singleton instance of the ScoreManager.
    /// </summary>
    public static ScoreManager Instance { get; private set; }

    /// <summary>
    /// Event fired when a player's score is updated.
    /// Passes (PlayerID, newTotalScore).
    /// </summary>
    public event System.Action<PlayerID, int> OnScoreUpdated;

    private Dictionary<PlayerID, int> _scores = new Dictionary<PlayerID, int>();

    private void Awake() {
        // Singleton pattern
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Initialize scores
        _scores[PlayerID.Player1] = 0;
        _scores[PlayerID.Player2] = 0;
    }

    private void OnEnable() {
        // Subscribe to the global enemy death event
        HealthComponent.OnAnyHealthComponentDied += HandleEnemyDied;
    }

    private void OnDisable() {
        // Unsubscribe
        HealthComponent.OnAnyHealthComponentDied -= HandleEnemyDied;
    }

    /// <summary>
    /// Called when any HealthComponent in the scene dies.
    /// </summary>
    /// <param name="deadComponent">The component that died.</param>
    /// <param name="killer">The GameObject that dealt the killing blow.</param>
    private void HandleEnemyDied(HealthComponent deadComponent, GameObject killer) {
        // Check if the killer was a player
        if (killer != null && killer.TryGetComponent<PlayerScore>(out PlayerScore playerScore)) {
            // Add score to the player who got the kill
            PlayerID playerID = playerScore.PlayerID;
            _scores[playerID]++;
            
            Debug.Log($"<color=cyan>{playerID} killed an enemy! New score: {_scores[playerID]}</color>");
            
            // Notify UI
            OnScoreUpdated?.Invoke(playerID, _scores[playerID]);
        }
    }

    /// <summary>
    /// Gets the current score for a specific player.
    /// </summary>
    public int GetScore(PlayerID playerID) {
        return _scores.ContainsKey(playerID) ? _scores[playerID] : 0;
    }
}