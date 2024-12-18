using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS {

    [CreateAssetMenu(fileName = nameof(GameManager), menuName = "Manager/" + nameof(GameManager))]
    public class GameManager : ScriptableObject {
        public static GameManager Instance { get; private set; }

        public void StartGame() {
            SceneManager.LoadScene(1);
        }

        public void ReturnMainMenu() {
            SceneManager.LoadScene(0);
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