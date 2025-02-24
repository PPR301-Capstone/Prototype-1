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

    //  Components
    Rigidbody2D rb2d;

    // Configurable Settings
    [Header("Movement Config")]
    [SerializeField] float MovementSpeed = 3.0f;
    [SerializeField] float JumpModifier = 1.0f;
    [SerializeField] float MovementDecay = 0.5f;

    // Forces
    Vector2 currentForce = Vector2.zero;

    // Detection
    void HandleRays()
    {
        // Downray

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

    void HandleInput()
    {
        currentForce = InputHandler.Instance.MovementInput;


        if (currentForce == Vector2.zero)
            currentForce = currentForce * MovementDecay;
	}

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

	private void FixedUpdate()
	{
        rb2d.AddForce(currentForce * MovementSpeed);
	}
}
