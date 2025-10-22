using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Managers {
    /// <summary>
    /// A serializable class to link a UI Button to a scene name.
    /// This makes it possible to edit the list of buttons and scenes in the Inspector.
    /// </summary>
    [System.Serializable]
    public class SceneButton {
        [Tooltip("The UI Button element that will trigger the scene load.")]
        public Button button;

        [Tooltip("The exact name of the scene this button should load. Must be in Build Settings.")]
        public string sceneName;
    }

    /// <summary>
    /// Manages all main menu navigation by linking UI buttons to scene names via a list in the Inspector.
    /// Attach this to a central manager object in your menu scene (e.g., the Canvas).
    /// </summary>
    public class MainMenuManager : MonoBehaviour {
        [Header("Scene Navigation")]
        [Tooltip("Assign your UI buttons and their target scene names here.")]
        [SerializeField]
        private List<SceneButton> sceneButtons = new List<SceneButton>();

        // The Awake method is called when the script instance is being loaded.
        // It's a great place to set up references and listeners.
        private void Awake() {
            // Configure each button in the list to load its assigned scene when clicked.
            foreach (var sceneButton in sceneButtons) {
                // Safety check to ensure the button is assigned in the Inspector.
                if (sceneButton.button != null) {
                    // This is a crucial step. We add a "listener" to the button's onClick event.
                    // When the button is clicked, it will execute the code we provide here.
                    // We use a lambda expression "() => ..." to call our loading function.
                    sceneButton.button.onClick.AddListener(() => LoadSceneByName(sceneButton.sceneName));
                } else {
                    Debug.LogWarning("A button in the SceneButtons list is not assigned.", this);
                }
            }
        }

        /// <summary>
        /// Loads a scene based on the string name provided.
        /// This method is called automatically by the listeners configured in Awake().
        /// </summary>
        /// <param name="sceneName">The exact name of the scene to load (must be in Build Settings).</param>
        public void LoadSceneByName(string sceneName) {
            if (string.IsNullOrEmpty(sceneName)) {
                Debug.LogError("SCENE LOADER: Scene name is null or empty! Cannot load scene.", this);
                return;
            }
            Debug.Log("Loading scene: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Quits the application.
        /// Note: This only works in a standalone build (e.g., .exe, .apk), not in the Unity Editor.
        /// You can link this to a 'Quit' button's OnClick event directly in the Inspector.
        /// </summary>
        public void QuitGame() {
            Debug.Log("Quitting application...");
            Application.Quit();
        }
    }
}