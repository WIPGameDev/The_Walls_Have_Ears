using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof())]
public class Flashlight : MonoBehaviour
{
    private bool active = false;
    private Light flashlight;

    private void Awake()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Light"))
        {
            if (active)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
        }
    }

    public void TurnOn ()
    {
        active = true;
        flashlight.enabled = true;
    }

    public void TurnOff ()
    {
        active = false;
        flashlight.enabled = false;
    }
}
