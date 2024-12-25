using System.Collections;
using System.Collections.Generic;
using TS;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.LoadScene("RockScene");
    } 

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
