using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MMSingleton<LightManager>
{
    [Tooltip("ȫ�ֹ���")]
    [SerializeField]
    private Light2D globalLight;

    [Tooltip("̫��")]
    [SerializeField]
    private Light2D sun;

    [Tooltip("���")]
    [SerializeField]
    private SpriteRenderer sky;

    [Tooltip("���")]
    [SerializeField]
    private Light2D playerLight;

    [Tooltip("����")]
    [SerializeField]
    private Light2D enemyLight;

    [Tooltip("��ҹ����ʱ��")]
    [SerializeField]
    private float changeTime = 5;

    [Tooltip("����ҹ�����ʱ��")]
    [SerializeField]
    private float durationTime = 20;
    // Start is called before the first frame update
    private float gameTime;

    public bool isNight = false;

    [Tooltip("ҹ����ɫ")]
    [SerializeField]
    private Color nightColor = new(10f / 255, 13f / 255, 42f / 255);

    [Tooltip("������ɫ")]
    [SerializeField]
    private Color moonColor = new(195f / 255, 23f / 255, 224f / 255);

    private Color dayColor = Color.white;
    private Vector3 sunScale = new Vector3(0.8f, 0.8f, 0.8f);

    void OnEnable()
    {
        StartCoroutine(nameof(StartTiming));    
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(StartTiming)); 
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
    }

    IEnumerator StartTiming()
    {
        while (true)
        {
            yield return new WaitForSeconds(durationTime);

            float time = 0f;
            while (time < changeTime)
            {
                time += Time.deltaTime;
                ChangedNightAndDay(time / changeTime);
                yield return null;
            }
            isNight = !isNight;
        }
    }

    private void ChangedNightAndDay(float time)
    {
        if (isNight)
        {
            globalLight.color = Color.Lerp(nightColor, dayColor, time);
            sun.color = Color.Lerp(moonColor, dayColor, time);
            sun.transform.localScale = Vector3.Lerp(sunScale, Vector3.one, time);
        } else
        {
            globalLight.color = Color.Lerp(dayColor, nightColor, time);
            sun.color = Color.Lerp(dayColor, moonColor, time);
            sun.transform.localScale = Vector3.Lerp(Vector3.one, sunScale, time);
        }
    }
}
