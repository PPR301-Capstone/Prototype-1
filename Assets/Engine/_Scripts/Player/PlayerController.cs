using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
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

    // Input Action System
    PlayerInput playerInput;
    InputActionMap PlayerActionMap;

    InputAction Move;
    InputAction Look;
    InputAction Jump;

    // Forces
    Vector2 currentForce = Vector2.zero;

	private void Awake()
	{
        playerInput = GetComponent<PlayerInput>();

        if (playerInput != null)
        {
			PlayerActionMap = playerInput.actions.FindActionMap("Player", true);
            Move = PlayerActionMap.FindAction("Move");
            Look = PlayerActionMap.FindAction("Look");
            Jump = PlayerActionMap.FindAction("Jump");
		}
        else
        {
            Debug.LogError("Error: PlayerInput component missing.");
        }
	}

	
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        rb2d = this.GetComponent<Rigidbody2D>();
    }

    void HandleInput()
    {
		if (PlayerActionMap != null)
		{
			currentForce = Move.ReadValue<Vector2>();
		}

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

	private void OnEnable()
	{
		PlayerActionMap?.Enable();
	}

	private void OnDisable()
	{
		PlayerActionMap?.Disable();
	}
}
