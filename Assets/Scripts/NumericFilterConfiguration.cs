using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NumericFilterConfiguration : MonoBehaviour
{
    private float _min;
    private float _max;
    private bool _inverter;

    public TextMeshProUGUI helper;
    public TMP_InputField inputMinValue; 
    public TMP_InputField inputMaxValue;
    public TMP_Text inputMinPlaceholder;
    public TMP_Text inputMaxPlaceholder;
    public Toggle selecaoInvertida;

    public void SetRange(Vector2 valorMinMax)
    {
        _min = valorMinMax.x;
        _max = valorMinMax.y;

        inputMinPlaceholder.text = _min.ToString();
        inputMaxPlaceholder.text = _max.ToString();
        inputMinValue.text = _min.ToString();
        inputMaxValue.text = _max.ToString();

        helper.text = $"Both value must be between " + _min + " and " + _max;
    }

    public string[] GetValores()
    {
        string input1 = string.IsNullOrEmpty(inputMinValue.text) ? _min.ToString() : inputMinValue.text;
        string input2 = string.IsNullOrEmpty(inputMaxValue.text) ? _max.ToString() : inputMaxValue.text;

        return new string[] { input1, input2, selecaoInvertida.isOn.ToString()};
    }

    public void ValidaValorInput()
    {
        float input1;
        float input2;

        float.TryParse(inputMinValue.text, out input1);
        float.TryParse(inputMaxValue.text, out input2);

        if (input1 < _min) inputMinValue.text = _min.ToString();
        if (input1 > _max) inputMinValue.text = _max.ToString();
        if (input2 < _min) inputMaxValue.text = _max.ToString();
        if (input2 > _max) inputMaxValue.text = _max.ToString();
    }
}
