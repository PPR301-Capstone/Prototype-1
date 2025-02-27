using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public enum MovementStates
    {
        IDLE,
        MOVING,
        JUMPING,
        FALLING,
    }

    public bool IsPlayerControlDisabled = false;
    public bool CanJump = false;
    bool isJumping = false;

    //  Components
    Rigidbody2D rb2d;

    // Configurable Settings
    [Header("Movement Config")]
    [SerializeField] float MovementSpeed = 3.0f;
    [SerializeField] float MovementDecay = 0.5f;
	[SerializeField] AnimationCurve JumpCurve;

    [Header("Jump Config")]
    [SerializeField] float MinJumpForce = 5.0f;
    [SerializeField] float MaxJumpForce = 10.0f;
    [SerializeField] float JumpHoldDuration = 0.5f;
    [SerializeField] float HorizontalLinearDamping = 0.75f;

	[Header("Detection")]
    [SerializeField] Sensor GroundSensor;

    // Forces
    Vector2 currentForce = Vector2.zero;
    Vector2 jumpForce = Vector2.zero;

    // Events
    IEnumerator JumpFinished()
    {
        float t = 0.0f;

        while (t < 1)
        {
            t += Time.deltaTime;

            jumpForce *= JumpCurve.Evaluate(t);
            yield return null;
        }
    }

    void GroundDetection(bool isGrounded)
    {
        CanJump = isGrounded;
        isJumping = false;
    }

    void HandleJump(float duration)
    {
        float dst = duration / JumpHoldDuration;

        Debug.Log(CanJump + " " + dst);

        if (CanJump && dst < 1)
        {
            jumpForce = new Vector2(0, Mathf.Lerp(MinJumpForce, MaxJumpForce, dst));
            isJumping = true;
		}
        else
        {
            HandleJumpReleased();
        }
    }

    void HandleJumpReleased()
    {
        isJumping = false;

        if (jumpForce.y > 0)
        {
            StartCoroutine(JumpFinished());
		}
    }

	private void Awake()
	{
        Instance = this;
	}
	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    void LinearDamping()
    {
        if (isJumping)
        {
            currentForce.x *= HorizontalLinearDamping;
        }
    }

	void HandleInput()
    {
        currentForce = new Vector2(InputHandler.Instance.MovementInput.x, 0) + jumpForce;

        LinearDamping();

        if (currentForce == Vector2.zero)
            currentForce = currentForce * MovementDecay;
	}

    // Update is called once per frame
    void Update()
    {
        if (!IsPlayerControlDisabled)
            HandleInput();

        Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)currentForce, Color.magenta);
    }

	private void FixedUpdate()
	{
        rb2d.AddForce(currentForce * MovementSpeed);
	}

	private void OnEnable()
	{
		GroundSensor.OnSensorTriggered += GroundDetection;

		InputHandler.Instance.OnJumpHeld += HandleJump;
		InputHandler.Instance.OnJumpReleased += HandleJumpReleased;
	}

	private void OnDisable()
	{
		GroundSensor.OnSensorTriggered -= GroundDetection;

		InputHandler.Instance.OnJumpHeld -= HandleJump;
		InputHandler.Instance.OnJumpReleased -= HandleJumpReleased;
	}
}
