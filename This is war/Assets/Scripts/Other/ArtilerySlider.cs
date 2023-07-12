using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtilerySlider : MonoBehaviour
{
    public static float SliderValue;
    public Slider slider;

    private void FixedUpdate()
    {
        SliderValue = slider.value;
    }
}
