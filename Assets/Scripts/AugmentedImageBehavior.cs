using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AugmentedImageBehavior : MonoBehaviour
{
    public void setSprite(Sprite sp)
    {
        GetComponent<SpriteRenderer>().sprite = sp;
        turnOn();
    }

    public void turnOn()
    {
        gameObject.SetActive(true);
    }

    public void turnOff()
    {
        gameObject.SetActive(false);
    }
}
