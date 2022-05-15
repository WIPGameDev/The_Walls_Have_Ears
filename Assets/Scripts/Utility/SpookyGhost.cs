using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpookyGhost : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    private Transform ghost;
    private Vector3 moveDirection;
    private Vector3 adjustedDirection;
    private Camera mainCam;
    
    // Start is called before the first frame update
    void Start()
    {
        ghost = GetComponent<Transform>();
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.z = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();
        adjustedDirection = mainCam.transform.TransformDirection(moveDirection);

        ghost.transform.position += adjustedDirection * Time.deltaTime * moveSpeed;

        if (Input.GetKey(KeyCode.Space))
        {
            ghost.transform.position += Vector3.up * Time.deltaTime * moveSpeed;
        }
        else if(Input.GetKey(KeyCode.LeftShift))
        {
            ghost.transform.position -= Vector3.up * Time.deltaTime * moveSpeed;
        }
    }
}
