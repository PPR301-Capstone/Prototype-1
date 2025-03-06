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
    bool isMoving = false;

    //  Components
    Rigidbody2D rb2d;
    CircleCollider2D circleCollider;

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

    PlayerAnim playerAnim;

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

    void HandleAttack(float duration)
    {
        float dst = duration / 2f;

        if (dst < 1)
        {
            // charging
        }
        else
        {
            // charged
        }
    }

    void HandleAttackReleased()
    {

        if (Player.Instance.currentWeapon)
        {
            Player.Instance.currentWeapon.Attack();
        }
	}

    void HandleJump(float duration)
    {
        float dst = duration / JumpHoldDuration;

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
        playerAnim = GetComponent<PlayerAnim>();
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

        isMoving = true;

        if (currentForce == Vector2.zero)
        {
            isMoving = false;
		}   
	}

    // Update is called once per frame
    void Update()
    {
        if (!IsPlayerControlDisabled)
            HandleInput();

        Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)currentForce, Color.magenta);

        playerAnim.FlipSprite(currentForce);
        playerAnim.Walk(isMoving);
		playerAnim.Jump(isJumping);
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

        InputHandler.Instance.OnAttackHeld += HandleAttack;
        InputHandler.Instance.OnAttackReleased += HandleAttackReleased;
	}

	private void OnDisable()
	{
		GroundSensor.OnSensorTriggered -= GroundDetection;

		InputHandler.Instance.OnJumpHeld -= HandleJump;
		InputHandler.Instance.OnJumpReleased -= HandleJumpReleased;

		InputHandler.Instance.OnAttackHeld -= HandleAttack;
		InputHandler.Instance.OnAttackReleased -= HandleAttackReleased;

		Instance = null;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "WeaponHitbox")
		{
			Debug.Log("Weapon hit player");
			Weapon weapon = collision.gameObject.transform.parent.GetComponent<Weapon>();

			Debug.Log(weapon);
			Player.Instance.TakeDamage(weapon.WeaponDamage);

			Debug.Log("Take Damage: Hitbox: " + weapon.WeaponDamage);
		}
	}
}
