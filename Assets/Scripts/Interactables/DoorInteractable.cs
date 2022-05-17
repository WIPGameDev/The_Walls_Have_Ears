using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : LockableInteractable
{
    [Header("Door Settings")]
    [SerializeField] private bool opened = false;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform openTransform;
    [SerializeField] private Transform closeTransform;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float moveSpeed = 10f;

    private Coroutine movingCoroutine;

    public override void OnActivation()
    {
        if (opened)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }

    public void OpenDoor ()
    {
        opened = true;
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
        }
        movingCoroutine = StartCoroutine(MovingDoor(openTransform));
    }

    public void CloseDoor()
    {
        opened = false;
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
        }
        movingCoroutine = StartCoroutine(MovingDoor(closeTransform));
    }

    private IEnumerator MovingDoor (Transform matchTransform)
    {
        while ( !(Vector3.Equals(targetTransform.position, matchTransform.position) && Quaternion.Equals(targetTransform.rotation, matchTransform.rotation)) )
        {
            targetTransform.position = Vector3.MoveTowards(targetTransform.position, matchTransform.position, moveSpeed * Time.deltaTime);
            targetTransform.rotation = Quaternion.RotateTowards(targetTransform.rotation, matchTransform.rotation, rotationSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }
}
