using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class virtualbarsbehavior : MonoBehaviour
{
    public Collider baseCollider;
    public Collider bar1;
    public Collider bar2;
    public Collider bar3;
    public Collider bar4;
    public Collider bar5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Object " + hit.transform.name + " hited!");
            }

        }
    }
}
