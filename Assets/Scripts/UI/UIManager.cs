using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TS;

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
                GameManager.Instance.ChangeGameState(GameState.Continue);
            } else
            {
                GameManager.Instance.ChangeGameState(GameState.Pause);
            }
        }
    }

    public void ContinueClick()
    {
        GameManager.Instance.ChangeGameState(GameState.Continue);
    }

    public void MainMenuClick()
    {
        GameManager.Instance.LoadScene("EnterScene");
    }

    public void ExitClick()
    {
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
