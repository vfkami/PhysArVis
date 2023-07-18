using UnityEngine;

public class CloseButtonBehavior : MonoBehaviour
{
    public GameObject Image;

    private void Start()
    {
        HandleTouch();
    }

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse click hits this object
            if (IsMouseClickOverObject())
            {
                // Handle the click event
                HandleClick();
            }
        }

        // Check for touch input
        if (Input.touchCount > 0)
        {
            // Iterate through all the touches
            foreach (Touch touch in Input.touches)
            {
                // Check if the touch hits this object
                if (IsTouchOverObject(touch))
                {
                    // Handle the touch event
                    HandleTouch();
                }
            }
        }
    }

    private bool IsMouseClickOverObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast a ray from the mouse position and check if it hits this object
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private bool IsTouchOverObject(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        // Cast a ray from the touch position and check if it hits this object
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private void HandleClick()
    {
        Debug.Log("Mouse click detected on " + gameObject.name);
        Image.SetActive(false);
    }

    private void HandleTouch()
    {
        Debug.Log("Touch detected on " + gameObject.name);
        Image.SetActive(false);
    }
}