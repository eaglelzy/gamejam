using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRockPower : MonoBehaviour {
    public Gradient gradient;  //能量条颜色渐变

    private Image powerImage;  //石头能量UI

    public void SetPower(float power) {
        powerImage.fillAmount = power / 100;
        powerImage.color = gradient.Evaluate(powerImage.fillAmount);
    }

    private void Awake() {
        powerImage = GetComponent<Image>();
    }

    private void Update() {
        SetPower(LevelManager.Instance.rock.MovePower);
    }
}