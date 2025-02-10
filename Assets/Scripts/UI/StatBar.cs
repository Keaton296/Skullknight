using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;

public class StatBar : MonoBehaviour
{
    [SerializeField] Slider valueSlider;
    [SerializeField] Slider changeSlider;
    [SerializeField] float changeSpeed;
    [SerializeField] float changeDelay;
    Tween changeSliderTween;
    
    public void OnValueChange(float newValue)
    {
        if (newValue < valueSlider.value) 
        {
            if (changeSliderTween != null) 
            { 
                changeSliderTween.Kill();
                changeSliderTween = null;
            }
            changeSliderTween = changeSlider.DOValue(newValue, changeSpeed, false).SetEase(Ease.OutSine);
        } 
        else changeSlider.value = newValue;
        valueSlider.value = newValue;
    }

    public void SetValue(float newValue)
    {
        if (changeSliderTween != null) 
        { 
            changeSliderTween.Kill();
            changeSliderTween = null;
        }
        changeSlider.value = newValue;
        valueSlider.value = newValue;
    }
}
