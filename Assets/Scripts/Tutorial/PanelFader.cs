using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelFader : MonoBehaviour
{
    [SerializeField]
    Image[] _imagesToFade;

    [SerializeField]
    TextMeshProUGUI[] _textsToFade;

    [SerializeField]
    private float _maxPanelOpacity;

    public void FadeIn(float fadeInTime)
    {
        StartCoroutine(StartFadingIn(Time.time, fadeInTime));
    }

    private IEnumerator StartFadingIn(float callTime, float fadeInTime)
    {
        float coroutineRunningTime;
        do
        {
             coroutineRunningTime = Time.time - callTime;

            float opacity = Mathf.Lerp(0f, 1f, coroutineRunningTime / fadeInTime);
            //Debug.Log("Fading in. opacity: " + opacity);

            foreach (Image image in _imagesToFade)
            {
                Color imageColor = image.color;
                imageColor.a = opacity * _maxPanelOpacity;
                image.color = imageColor;
            }

            foreach (TextMeshProUGUI textMsg in _textsToFade)
            {
                Color textColor = textMsg.color;
                textColor.a = opacity;
                textMsg.color = textColor;
            }
            yield return null;

        } while (coroutineRunningTime <= fadeInTime);
        
    }

    public void FadeOut(float fadeOutTime)
    {
        StartCoroutine(StartFadingOut(Time.time, fadeOutTime));
    }

    private IEnumerator StartFadingOut(float callTime, float fadeOutTime)
    {
        float coroutineRunningTime = Time.time - callTime;

        while (coroutineRunningTime <= fadeOutTime)
        {
            float opacity = Mathf.Lerp(1f, 0f, coroutineRunningTime / fadeOutTime);

            foreach (Image image in _imagesToFade)
            {
                Color imageColor = image.color;
                imageColor.a = opacity * _maxPanelOpacity;
                image.color = imageColor;
            }

            foreach (TextMeshProUGUI textMsg in _textsToFade)
            {
                Color textColor = textMsg.color;
                textColor.a = opacity;
                textMsg.color = textColor;
            }
            coroutineRunningTime = Time.time - callTime;
            yield return null;
        }
    }
}
