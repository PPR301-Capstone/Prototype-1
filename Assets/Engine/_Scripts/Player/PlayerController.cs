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

        if (currentForce == Vector2.zero)
            currentForce = currentForce * MovementDecay;
	}

    // Update is called once per frame
    void Update()
    {
        if (!IsPlayerControlDisabled)
            HandleInput();

        Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)currentForce, Color.magenta);

        playerAnim.FlipSprite(currentForce);

        if (Input.GetKeyUp(KeyCode.X))
        {
            Attack();
        }

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

        Instance = null;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "WeaponHitbox")
		{
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();

            Debug.Log(weapon);
			Player.Instance.TakeDamage(weapon.WeaponDamage);

			Debug.Log("Take Damage: Hitbox: " + weapon.WeaponDamage);
		}
        else if (collision.gameObject.tag == "Enemy")
        {
            Player.Instance.TakeDamage(15);
        }
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







	void Attack() // Please remove X Key after testing and add functionality 
    {
        Debug.Log("Player Just Attacked Yo");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                //enemy.GetComponent<EnemyAgent>().TakeDamage(); // player pressed key X all enemies die instantly ( FUNNNN TIMES )) ** TESTING ONLY ***
            }
        }
    }

    /* heath required for additive (TODO) when merged
     * // Knockback effect when hit by enemy ( Facing enemy direction ) **NEW** maybe implement
        Vector2 knockbackDirection = (transform.position - target.transform.position).normalized; // direction away from enemy
        Vector2 knockbackForce = knockbackDirection * 5f + Vector2.up * 2f; // Push player back slightly up and away

        rb2d.velocity = Vector2.zero; // Resets the player velocity before applying force ( redundant check so they arent knockback while being knocked back
        rb2d.AddForce(knockbackForce, ForceMode2D.Impulse);
    */
}
