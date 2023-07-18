using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class FiducialMarkerController : MonoBehaviour
{
    public TextMeshProUGUI markerText;
    public Image image;
    public Canvas canvas;
    public Button button;

    private bool isClicked = false;
    private float doubleClickTimeThreshold = 0.3f; // Time threshold for a double-click (in seconds)


    //by default, canvas start the scene disabled;
    private void Start()
    {
        button.onClick.AddListener(OnClick);
        canvas.enabled = true;

        //SetModoImagem();
    }
    public void SetModoTexto()
    {
        markerText.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
    }
    public void SetModoImagem()
    {
        markerText.gameObject.SetActive(false);
        image.gameObject.SetActive(true);   
    }

    public void SetTexto(string text)
    {
        markerText.text = text;
    }

    public void SetImage(Sprite sprite)
    {
        image.sprite = sprite;
    }

    void OnMouseDown()
    {
        canvas.enabled = !canvas.enabled;
    }

    void OnClick()
    {
        if (isClicked) // Double-click detected
        {
            FindObjectOfType<FiducialMarkerManager>().setZoomedImageSprite(image.sprite);
            isClicked = false;
        }
        else // Start the timer for the double-click
        {
            isClicked = true;
            Invoke("ResetClick", doubleClickTimeThreshold);
        }
    }

    void ResetClick()
    {
        // Reset the click state after the time threshold
        isClicked = false;
    }

}
