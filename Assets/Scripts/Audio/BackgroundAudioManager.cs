using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using TS;

public class BackgroundAudioManager : MMSingleton<BackgroundAudioManager>, MMEventListener<MMStateChangeEvent<GameState>>
{
    [Tooltip("±≥æ∞“Ù¿÷")]
    [SerializeField]
    private AudioClip backgroud;

    [Tooltip("±≥æ∞“Ù¿÷")]
    [SerializeField]
    private AudioClip backgroud2;

    private AudioSource audioBackground;

    private bool changedBackground = false;

    protected override void Awake()
    {
        base.Awake();
        audioBackground = GetComponent<AudioSource>();
        audioBackground.clip = backgroud;
        PlayBackground();
    }

    private void Update()
    {
        if (LightManager.Instance.day == 5 && !changedBackground)
        {
            ChangedBackground();
        }
    }

    public void ChangedBackground()
    {
        audioBackground.clip = backgroud2;
        PlayBackground();
        changedBackground = true;
    }

    public void PauseBackground()
    {
        audioBackground.Pause();
    }

    public void PlayBackground()
    {
        audioBackground.Play();
    }
    private void OnEnable()
    {
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
                PlayBackground();
                break;
            case GameState.Continue:
                PlayBackground();
                break;
            case GameState.Pause:
                PauseBackground();
                break;
            case GameState.GameOver:
                PauseBackground();
                break;
            case GameState.Win:
                // It Never Ends
                break;
        }
    }
}
