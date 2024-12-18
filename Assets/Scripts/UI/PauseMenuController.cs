using UnityEngine;

namespace TS.UI {

    public class PauseMenuController : MonoBehaviour {
        public GameObject pauseMenu;
        private bool isPaused;

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                isPaused = !isPaused;
                pauseMenu.SetActive(isPaused);
                if (isPaused) {
                    Time.timeScale = 0f;
                } else {
                    Time.timeScale = 1f;
                }
            }
        }
    }
}