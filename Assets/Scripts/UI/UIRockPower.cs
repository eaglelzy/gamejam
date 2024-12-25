using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRockPower : MonoBehaviour
{
    //石头能量UI
    private Image powerImage;

    private void Awake() {
        powerImage = GetComponent<Image>();
    }

    public void SetPower(float power) {
        powerImage.fillAmount = power / 100;
    }

    private void Update() {
        SetPower(LevelManager.Instance.rock.MovePower);
    }
}
