using UnityEngine;
using UnityEngine.InputSystem;
using System;

// High priority for input
[DefaultExecutionOrder(-100)]
public class InputHandler : MonoBehaviour
{
	public static InputHandler Instance;
	// Input Action System
	PlayerInput playerInput;
	InputActionMap PlayerActionMap;

	InputAction Move;
	InputAction Look;
	InputAction Jump;

	// Event Dispatchers
	public event Action<Vector2> OnMove;
	public event Action<Vector2> OnLook;
	public event Action OnAction;

	// Attributes
	public Vector2 MovementInput = Vector2.zero;
	public Vector2 LookInput = Vector2.zero;

	private void HandleMove(InputAction.CallbackContext context)
	{
		Vector2 movementInput = context.ReadValue<Vector2>();
		OnMove?.Invoke(movementInput);

		Debug.Log(movementInput);
	}

	private void HandleLook(InputAction.CallbackContext context)
	{
		Vector2 lookInput = context.ReadValue<Vector2>();
		OnLook?.Invoke(lookInput);

		Debug.Log(lookInput);
	}

	private void HandleJump(InputAction.CallbackContext context)
	{
		if (context.performed)
			OnAction?.Invoke();
	}

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		if (playerInput != null)
		{
			PlayerActionMap = playerInput.actions.FindActionMap("Player", true);
			Move = PlayerActionMap.FindAction("Move");
			Look = PlayerActionMap.FindAction("Look");
			Jump = PlayerActionMap.FindAction("Jump");

			Move.Enable();
			Look.Enable();
			Jump.Enable();
		}
		else
		{
			Debug.LogError("Error: PlayerInput component missing.");
		}
	}

	private void OnEnable()
	{
		if (Move != null)
			Move.performed += HandleMove;


		if (Look != null)
			Look.performed += HandleLook;

		if (Jump != null)
			Jump.performed += HandleJump;
	}

	private void OnDisable()
	{
		if (Move != null)
			Move.performed -= HandleMove;

		if (Look != null)
			Look.performed -= HandleLook;

		if (Jump != null)
			Jump.performed -= HandleJump;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        if (Move != null && Look != null && Jump != null)
		{
			Instance = this;
			Debug.Log("InputHandler: Ready");
		}
    }

	void Update()
	{
		MovementInput = Move.ReadValue<Vector2>();
		LookInput = Look.ReadValue<Vector2>();
	}
}
