using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TS.Character;

public class UIManager : MMSingleton<UIManager>
{

    [SerializeField]
    private Canvas canvas;

    [Tooltip("Íæ¼Ò")]
    [SerializeField]
    public GameObject player;

    private GameObject pauseMenu;
    private GameObject gameOverMenu;
    private Slider heathSlider;
    private Health health;

    protected override void Awake()
    {
        base.Awake();
        pauseMenu = canvas.transform.Find("PauseMenu").gameObject;
        gameOverMenu = canvas.transform.Find("GameOverMenu").gameObject;
        heathSlider = canvas.transform.Find("Health").GetComponent<Slider>();
        health = player.GetComponent<Health>();
    }

    private void OnEnable()
    {
        health.HealthChangedAction += HealthChanged;
    }

    private void OnDisable()
    {
        health.HealthChangedAction -= HealthChanged;
    }

    private void HealthChanged()
    {
        heathSlider.value = health.CurrentHealth / health.MaximumHealth; 
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
