using UnityEngine;
namespace Player {
    /// <summary>
    /// Component attached to a player GameObject to identify their PlayerID.
    /// Used by the ScoreManager to attribute kills.
    /// </summary>
    public class PlayerScore : MonoBehaviour {
        /// <summary>
        /// The ID for this player. Set this in the Inspector.
        /// </summary>
        [Tooltip("The ID for this player. Set this in the Inspector.")]
        [SerializeField] private PlayerID playerID;

        /// <summary>
        /// Public getter for the Player's ID.
        /// </summary>
        public PlayerID PlayerID
        {
            get
            {
                return playerID;
            }
        }
    }
}
