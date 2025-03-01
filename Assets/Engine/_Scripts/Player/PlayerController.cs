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
    private HealthSystem healthSystem; // Ref Health system ********NEW**********
    private KeySystem keySystem; // Ref Key System ^^^^^

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

    // Damage Cooldown                                      ********NEW*********
    private bool isInvincible = false;
    private float invincibilityDuration = 1.0f;


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
        healthSystem = GetComponent<HealthSystem>(); // Initialise Health System                    *****NEW******
        keySystem = GetComponent<KeySystem>(); // Initialise Key System ^^^^
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

    // Damage functionality below                                       ******NEW*******

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("Hazard")) && !isInvincible) // Ensure Enemy Tag Uppercase E
        {
            TakeDamage(1);
        }

        if (other.CompareTag("Key")) // ensure TAG "Key" upper case K 
        {
            CollectKey();
            Destroy(other.gameObject);
        }
    }

    /* gotta work out the enemy collider, so that when the player runs into an enemy they dont just get hit,
     * 
     * the enemy has to **HIT** them in order for them to lose health ( Max 3 lives == 3 Hits )
    */


    public void TakeDamage(int damage)
    {
        healthSystem.TakeDamage(damage); // Call Health Syster              ****NEW****

        if (healthSystem.currentHealth > 0)
        {
            StartCoroutine(InvincibilityFrames()); // add Invincibility after successfull hit

        }
    }

    private IEnumerator InvincibilityFrames() // Wait after getting hit before health comes back on         ****NEW****
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public void CollectKey() // Key Collection Update
    {
        if(keySystem != null)
        {
            keySystem.CollectKey(); // Update key UI                        *****NEW*****
        }
    }








}
