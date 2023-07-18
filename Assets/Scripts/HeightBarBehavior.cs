using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightBarBehavior : MonoBehaviour
{
    public TextMesh _low;
    public TextMesh _mid;
    public TextMesh _high;

    public void DefineTextoEixoY(Vector3 values)
    {
        if (values.x > 1000)
            _low.text = (Math.Truncate(values.x / 1000)+  " mil").ToString();
        else
            _low.text = values.x.ToString();

        if (values.y > 1000)
            _mid.text = (Math.Truncate(values.y / 1000) + "mil").ToString();
        else
            _mid.text = values.y.ToString();

        if (values.z > 1000)
            _high.text = (Math.Truncate(values.z / 1000) + " mil").ToString();
        else
            _high.text = values.z.ToString();
    }
}
