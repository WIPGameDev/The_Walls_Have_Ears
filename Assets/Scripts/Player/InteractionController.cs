using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{

    [Header("Input")]
    [SerializeField] private string interactButton = "Interact";
    [SerializeField] private string holdButton = "Hold";

    [Header("Interactions")]
    [SerializeField] private float interactionDistance = 3.0f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private LayerMask holdableMask;
    [SerializeField] private Transform holdPositionTransform;
    [SerializeField] private float holdCenteringForce = 5f;
    [SerializeField] private float throwingForce = 10f;
    private GameObject heldObject;
    private Coroutine CenterObjectCoroutine;

    private bool holding = false;

    void Update()
    {
        if (Input.GetButtonDown(interactButton))
        {
            RaycastHit result;
            if (Physics.Raycast(transform.position, transform.forward, out result, interactionDistance, interactableMask))
            {
                result.transform.gameObject.SendMessage("Activate");
            }
        }

        if (Input.GetButtonDown(holdButton) && !holding)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance, holdableMask))
            {
                if (hit.transform.gameObject.GetComponent<Rigidbody>() != null)
                {
                    heldObject = hit.transform.gameObject;
                    HoldObject();
                }
            }
        }
        if (Input.GetButtonUp(holdButton) && holding)
        {
            DropObject();
        }

        if (Input.GetButtonDown("MouseLeft") && holding)
        {
            ThrowObject();
        }
    }

    private void HoldObject()
    {
        holding = true;
        heldObject.GetComponent<Rigidbody>().useGravity = false;
        heldObject.GetComponent<Rigidbody>().drag = 10f;
        heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        heldObject.transform.position = holdPositionTransform.position;
        CenterObjectCoroutine = StartCoroutine(CenterHeldObject());
    }

    private void DropObject()
    {
        holding = false;
        StopCoroutine(CenterObjectCoroutine);
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject.GetComponent<Rigidbody>().drag = 0f;
            heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }

    private void ThrowObject ()
    {
        holding = false;
        StopCoroutine(CenterObjectCoroutine);
        if (heldObject != null)
        {
            heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            heldObject.GetComponent<Rigidbody>().useGravity = true;
            heldObject.GetComponent<Rigidbody>().drag = 0f;
            heldObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            heldObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwingForce, ForceMode.Impulse);
        }
    }

    private IEnumerator CenterHeldObject()
    {
        Rigidbody rgdbdy = heldObject.GetComponent<Rigidbody>();
        while (holding && rgdbdy != null)
        {
            float dist = Vector3.Distance(rgdbdy.position, holdPositionTransform.position);
            if (dist > 2f)
            {
                DropObject();
            } else if (dist > 0.01f)
            {
                rgdbdy.MoveRotation(holdPositionTransform.rotation);
                rgdbdy.AddForce((holdPositionTransform.position - rgdbdy.position) * holdCenteringForce, ForceMode.VelocityChange);
            } else
            {
                rgdbdy.MovePosition(holdPositionTransform.position);
            }
            yield return new WaitForFixedUpdate();
        }
    }
}