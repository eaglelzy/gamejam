using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS {

    [CreateAssetMenu(fileName = nameof(GameManager), menuName = "Manager/" + nameof(GameManager))]
    public class GameManager : ScriptableObject {
        public static GameManager Instance { get; private set; }

        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitGame() {
            Application.Quit();
        }

        private void Awake() {
            Instance = this;
        }

        private void OnDestroy() {
            if (Instance == this) {
                Instance = null;
            }
        }
    }
}