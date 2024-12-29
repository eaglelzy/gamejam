using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class UIScaler : MonoBehaviour 
{
    [SerializeField] private float scaleTime = 1f;
    [SerializeField] private float targetScale = 1f;
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [SerializeField] private List<TextMeshProUGUI> textList;

    int cnt = 0;

    private void Start()
    {
        StartScaling();
    }

    public void StartScaling()
    {
        StartCoroutine(ScaleOverTime(textList[cnt].transform));
    }

    private IEnumerator ScaleOverTime(Transform target)
    {

        float elapsedTime = 0;
        Vector3 startScale = target.localScale;
        Vector3 targetScaleVector = Vector3.one * targetScale;

        while (elapsedTime < scaleTime)
        {
            elapsedTime += Time.deltaTime;
            float percentage = elapsedTime / scaleTime;
            float curveValue = scaleCurve.Evaluate(percentage);
            target.localScale = Vector3.Lerp(startScale, targetScaleVector, curveValue);
            yield return null;
        }

        cnt++;

        target.localScale = Vector3.zero;

        if (cnt < textList.Count)
        StartCoroutine(ScaleOverTime(textList[cnt].transform));
    }
}