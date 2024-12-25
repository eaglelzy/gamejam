using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS {


public enum GameState { Start, Pause, Continue, GameOver, Win }
    //[CreateAssetMenu(fileName = nameof(GameManager), menuName = "Manager/" + nameof(GameManager))]
public class GameManager : MMSingleton<GameManager>, MMEventListener<MMStateChangeEvent<GameState>>
 {
        //public static GameManager Instance { get; private set; }

        private MMStateMachine<GameState> machine;
        public void LoadScene(string sceneName) {
            SceneManager.LoadScene(sceneName);
            ChangeGameState(GameState.Start);
        }

        public void QuitGame() {
            Application.Quit();
#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
 #endif
        }

        public void ChangeGameState(GameState state)
        {
            machine.ChangeState(state);
        }

        private void Pause(bool isPaused)
        {
            if (isPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        private void OnEnable()
        {
            machine = new MMStateMachine<GameState>(gameObject, true);
            MMEventManager.AddListener(this);
        }

        private void OnDisable()
        {
            MMEventManager.RemoveListener(this);
        }

        public void OnMMEvent(MMStateChangeEvent<GameState> eventType)
        {
            switch (eventType.NewState)
            {
                case GameState.Start:
                    Pause(false);
                    break;
                case GameState.Continue:
                    Pause(false);
                    UIManager.Instance.ShowPause(false);
                    break;
                case GameState.Pause:
                    Pause(true);
                    UIManager.Instance.ShowPause(true);
                    break;
                case GameState.GameOver:
                    UIManager.Instance.ShowPause(true);
                    Pause(true);
                    break;
                case GameState.Win:
                    // It Never Ends
                    break;
            }
        }
    }
}