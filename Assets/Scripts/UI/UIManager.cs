using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TS;
using UnityEngine.SceneManagement;

public class UIManager : MMSingleton<UIManager>
{

    [SerializeField]
    private Canvas canvas;

    private GameObject pauseMenu;
    private GameObject gameOverMenu;

    protected override void Awake()
    {
        base.Awake();
        pauseMenu = canvas.transform.Find("PauseMenu").gameObject;
        gameOverMenu = canvas.transform.Find("GameOverMenu").gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                ShowPause(false);
                GameManager.Instance.ChangeGameState(GameState.Continue);
            } else
            {
                ShowPause(true);
                GameManager.Instance.ChangeGameState(GameState.Pause);
            }
        }
    }

    public void ContinueClick()
    {
        ShowPause(false);
        GameManager.Instance.ChangeGameState(GameState.Continue);
    }


    public void RestartClick()
    {
        ShowGameOver(false);
        GameManager.Instance.ChangeGameState(GameState.Start);
    }

    public void MainMenuClick()
    {
        ShowPause(false);
        ShowGameOver(false);
        GameManager.Instance.LoadScene("EnterScene");
    }

    public void ExitClick()
    {
        ShowPause(false);
        ShowGameOver(false);
        GameManager.Instance.QuitGame();
    }

    public void ShowPause(bool isShow)
    {
        pauseMenu.SetActive(isShow);
    }
    public void ShowGameOver(bool isShow)
    {
        gameOverMenu.SetActive(isShow);
    }
}
