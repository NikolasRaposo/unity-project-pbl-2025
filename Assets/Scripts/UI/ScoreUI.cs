using Player;
using TMPro; // Add this if you use TextMeshPro
// using UnityEngine.UI; // Add this if you use regular Text
using UnityEngine;

/// <summary>
/// Updates the score display UI by listening to the ScoreManager.
/// </summary>
public class ScoreUI : MonoBehaviour {
    [Header("UI References")]
    [Tooltip("Text component to display Player 1's score.")]
    [SerializeField] private TextMeshProUGUI player1ScoreText; // Use 'Text' if not using TMPro

    [Tooltip("Text component to display Player 2's score.")]
    [SerializeField] private TextMeshProUGUI player2ScoreText; // Use 'Text' if not using TMPro

    private void Start() {
        // Subscribe to score updates
        ScoreManager.Instance.OnScoreUpdated += UpdateScoreText;

        // Set initial scores
        if (player1ScoreText != null) {
            player1ScoreText.text = $"P1 Score: {ScoreManager.Instance.GetScore(PlayerID.Player1)}";
        }
        if (player2ScoreText != null) {
            player2ScoreText.text = $"P2 Score: {ScoreManager.Instance.GetScore(PlayerID.Player2)}";
        }
    }

    private void OnDestroy() {
        // Unsubscribe
        if (ScoreManager.Instance != null) {
            ScoreManager.Instance.OnScoreUpdated -= UpdateScoreText;
        }
    }

    /// <summary>
    /// Callback function to update the UI when a score changes.
    /// </summary>
    private void UpdateScoreText(PlayerID playerID, int newScore) {
        if (playerID == PlayerID.Player1 && player1ScoreText != null) {
            player1ScoreText.text = $"P1 Score: {newScore}";
        }
        else if (playerID == PlayerID.Player2 && player2ScoreText != null) {
            player2ScoreText.text = $"P2 Score: {newScore}";
        }
    }
}
