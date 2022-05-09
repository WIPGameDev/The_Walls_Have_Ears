using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private Transform headTransform;
    private Vector3 headDefaultLocalPosition;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask whatIsCollidable;

    [Header("Keys names")]
    [SerializeField] private string jumpKey;
    [SerializeField] private string crouchKey;
    [SerializeField] private string runKey;
    [SerializeField] private string leanKey;

    [Header("Movement Settings")]
    [SerializeField] private bool canRun = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 10f;
    private bool isRunning = false;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetVelocity = Vector3.zero;
    private float moveSpeed = 0f;

    [Header("Crouching")]
    private Coroutine crouchingCoroutine;
    private bool isCrouching = false;
    [SerializeField] private float crouchMoveSpeed = 5f;
    [SerializeField] private float crouchingSpeed = 1f;
    [SerializeField] private float crouchingHeight = 1f;
    private float standingHeight;

    [Header("Jump")]
    private float yVelocity = 0f;
    [SerializeField] private float gravityMultiplier = 1f;
    [SerializeField] private float terminalVelocity = 90f;
    [SerializeField] private float jumpHeight = 2f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        standingHeight = characterController.height;
        moveSpeed = walkSpeed;
        headDefaultLocalPosition = headTransform.localPosition;
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Get the previous y velocity so that gravity can be added to it
        yVelocity = targetVelocity.y;
        //Get Input move direction
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.z = Input.GetAxisRaw("Vertical");

        //Leaning
        Vector3 leanPosition = headTransform.localPosition;
        float leanFactor = Input.GetAxis(leanKey);
        if ((leanFactor > 0 && CanLean(true)) || (leanFactor < 0 && CanLean(false)))
        {
            leanPosition.x = leanFactor;
        } else
        {
            leanPosition.x = 0f;
        }
        headTransform.localPosition = leanPosition;

        //Modifies move velocity if running
        if (Input.GetButtonDown(runKey) && canRun && !isRunning)
        {
            StartRunning();
        }
        if (Input.GetButtonUp(runKey))
        {
            StopRunning();
        }

        //Set target velocity vector for move()
        targetVelocity = moveDirection.normalized * moveSpeed;
        targetVelocity.y = yVelocity;

        //Process jump input or add gravity. Modifies tagetVelocity Vector y axis.
        if (characterController.isGrounded)
        {
            //Jump or Y move to keep grounded
            if (Input.GetButtonDown(jumpKey) && canJump)
            {
                targetVelocity.y = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics.gravity.y * gravityMultiplier * 2.0f));
            } else
            {
                targetVelocity.y = -10f;
            }
        } else
        {
            //Gravity acceleration
            targetVelocity.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }

        if (Input.GetButtonDown(crouchKey) && canCrouch)
        {
            if (!isCrouching)
            {
                Crouch();
            } else if (CanStand())
            {
                Stand();
            }
        }

        //Clamps y axis velocity to terminalVelocity
        targetVelocity.y = Mathf.Clamp(targetVelocity.y, -terminalVelocity, terminalVelocity);

        //Converts velocity to world Space
        targetVelocity = transform.TransformDirection(targetVelocity);
        //Applies move delta
        characterController.Move(targetVelocity * Time.deltaTime);
    }

    private void StartRunning()
    {
        if (!isCrouching)
        {
            isRunning = true;
            moveSpeed = runSpeed;
        }
    }

    private void StopRunning()
    {
        if (!isCrouching)
        {
            isRunning = false;
            moveSpeed = walkSpeed;
        }
    }

    private void Crouch()
    {
        moveSpeed = crouchMoveSpeed;
        if (crouchingCoroutine != null)
        {
            StopCoroutine(crouchingCoroutine);
        }
        crouchingCoroutine = StartCoroutine(Crouching(crouchingHeight));
    }

    private void Stand()
    {
        moveSpeed = walkSpeed;
        if (crouchingCoroutine != null)
        {
            StopCoroutine(crouchingCoroutine);
        }
        crouchingCoroutine = StartCoroutine(Crouching(standingHeight));
    }

    /// <summary>
    /// SphereCast from the top semisphere of the CharacterController up to StandingHeight to check for collisions.
    /// </summary>
    /// <returns>True if there is no collision, false otherwise</returns>
    private bool CanStand()
    {
        RaycastHit hit;
        Vector3 position = transform.position + characterController.center;
        //Sets y coordinate to top semisphere plus a small offset to make it start from within the controller's collider.
        position.y = position.y + (characterController.height / 2.0f) - characterController.radius - 0.1f;
        if (Physics.SphereCast(position, characterController.radius - characterController.skinWidth, Vector3.up, out hit, standingHeight - characterController.height + 0.1f, whatIsCollidable, QueryTriggerInteraction.Ignore))
        {
            return false;
        }
        return true;
    }

    private IEnumerator Crouching(float targetHeight)
    {
        float startHeight = characterController.height;
        float smoothingFactor = crouchingSpeed * Time.deltaTime;
        isCrouching = !isCrouching;

        Vector3 headPosition = headTransform.localPosition;

        while (characterController.height != targetHeight)
        {
            characterController.height = Mathf.Lerp(startHeight, targetHeight, smoothingFactor);
            headPosition.y = (characterController.height / 2.0f) - characterController.radius;
            headDefaultLocalPosition.y = headPosition.y;
            headTransform.localPosition = headPosition;
            smoothingFactor += crouchingSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }


    /// <summary>
    /// A SphereCast right or left to check for collisions at the leaning positions to the sides of the player.
    /// </summary>
    /// <param name="right">True if is cast to the right, false to cast leftwards.</param>
    /// <returns>Whether or not a collider was hit.</returns>
    private bool CanLean(bool right)
    {
        Vector3 direction = right ? transform.right : transform.right * -1;
        RaycastHit hit;
        if (Physics.SphereCast(transform.position + headDefaultLocalPosition, characterController.radius - characterController.skinWidth, direction, out hit, 1.1f, whatIsCollidable, QueryTriggerInteraction.Ignore))
        {
            return false;
        }
        return true;
    }
}