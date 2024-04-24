using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField] private Transform look; //pivot

	[Header("Player Movement")]
    [SerializeField] private float moveSpeed = 4.0f;
    [SerializeField] private float sprintSpeed = 6.0f;
    [SerializeField] private float rotationSpeed = 1.0f;

	[Space(10)]
    [SerializeField] private float jumpHeight = 1.2f;

	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] private float gravity = -15.0f;
    [SerializeField] private float terminalVelocity = 53.0f;

	[Header("Player Grounded")]
    [SerializeField] private bool grounded = true;
    [SerializeField] private float groundedOffset = 0.85f;

	[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    [SerializeField] private float groundedRadius = 0.5f;

    [SerializeField] private LayerMask groundLayers;

	[Header("Camera Limits")]
    [SerializeField] private float minCameraAngle = -90F;
    [SerializeField] private float maxCameraAngle = 90F;

	private CharacterController controller;

	private Quaternion characterTargetRot;
	private Quaternion cameraTargetRot;

	private float verticalVelocity;

	private bool sprint;
	private bool jump = false;

    private Vector3 setDirection;
	private Vector2 lookDirection;

    private void Start()
	{
		controller = GetComponent<CharacterController>();
		characterTargetRot = transform.localRotation;

		if(!look)
		{
            Debug.LogError($"{name}: Camera Pivot Point not assigned!");
            Debug.LogError($"Disabling component");
            enabled = false;
        }

		else
		cameraTargetRot = look.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
    }

	private void Update()
	{		
        GroundedCheck();
        JumpAndGravity();
        Move();
		LookRotation();
    }

    private void GroundedCheck()
	{
		Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundedOffset, transform.position.z);
		grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
	}

	public void Jump()
	{
		jump = true;
	}

	public void Sprint(bool isSprinting)
	{
		sprint = isSprinting;
	}

    public void SetDirection(Vector3 direction)
    {
        setDirection = direction;
    }

	public void Look(Vector2 lookInput)
	{
		lookDirection = lookInput;

    }

    private void JumpAndGravity()
	{
		if (grounded && jump)
		{
			// the square root of H * -2 * G = how much velocity needed to reach desired height
			verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
		}
		else
		{
			// if we are not grounded, do not jump
			jump = false;
		}

		// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
		if (verticalVelocity < terminalVelocity)
			verticalVelocity += gravity * Time.deltaTime;
	}

	private void Move()
	{
		Vector3 direction = new Vector3(setDirection.x, 0, setDirection.z);

        direction = direction.x * transform.right + direction.z * transform.forward;

        // set target speed based on move speed, sprint speed and if sprint is pressed
        float targetSpeed = sprint ? sprintSpeed : moveSpeed;

        // move the player
        controller.Move(direction.normalized * (targetSpeed * Time.deltaTime) + new Vector3(0.0f, verticalVelocity, 0.0f) * Time.deltaTime);
    }

	public void LookRotation()
	{
		float yRot = lookDirection.x * rotationSpeed;
		float xRot = lookDirection.y * rotationSpeed;

		characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
		cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

		cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

		transform.localRotation = characterTargetRot;
		look.localRotation = cameraTargetRot;
	}

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
		angleX = Mathf.Clamp(angleX, minCameraAngle, maxCameraAngle);

		q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}
}
