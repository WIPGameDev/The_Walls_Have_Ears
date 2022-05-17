using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField] private Transform bodyTarget;

    [Header("Input")]
    [SerializeField] private string mouseXAxis = "Mouse X";
    [SerializeField] private string mouseYAxis = "Mouse Y";
    private float xInput = 0f;
    private float yInput = 0f;
    private float pitchRotation = 0f;

    [Header("Sensitivity")]
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float yawModifier = 1f;
    [SerializeField] private float pitchModifier = 1f;

    private float pitch;

    // Start is called before the first frame update
    void Start()
    {
        bodyTarget = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //Change this to something better for pausing the first person camera
        if (Time.timeScale == 0f)
        {
            return;
        }

        xInput = Input.GetAxisRaw(mouseXAxis);
        yInput = Input.GetAxisRaw(mouseYAxis);

        pitchRotation -= yInput * pitchModifier * sensitivity;
        pitchRotation = Mathf.Clamp(pitchRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(pitchRotation, 0f, 0f);
        bodyTarget.Rotate(Vector3.up * xInput * yawModifier * sensitivity);
    }
}