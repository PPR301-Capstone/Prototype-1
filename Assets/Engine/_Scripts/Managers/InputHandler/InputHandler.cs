using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Collections;

// High priority for input
[DefaultExecutionOrder(-100)]
public class InputHandler : MonoBehaviour
{
	public static InputHandler Instance;
	// Input Action System
	PlayerInput playerInput;
	InputActionMap PlayerActionMap;
	InputActionMap UIActionMap;

	// Player Inputs
	InputAction Move;
	InputAction Look;
	InputAction Jump;

	// UI Inputs
	InputAction UISubmit;
	InputAction UINavigate;
	InputAction UIEscape;

	// Event Dispatchers

	//	Player Movement
	public event Action<Vector2> OnMove;
	public event Action<Vector2> OnLook;
	public event Action OnAction;
	//public event Action OnJump;
	public event Action<float> OnJumpHeld;
	public event Action OnJumpReleased;

	public bool isJumping = false;
	public float jumpHoldTime = 0;

	//	UI Navigation
	public event Action<Vector2> OnUINavigate;
	public event Action OnEscapePressed;
	public event Action OnUISubmit;

	// Attributes
	public Vector2 MovementInput = Vector2.zero;
	public Vector2 LookInput = Vector2.zero;

	// UI
	public Vector2 UINav = Vector2.zero;

	IEnumerator JumpTimer()
	{
		while (isJumping)
		{
			jumpHoldTime = InputHandler.Instance.jumpHoldTime;
			yield return null;
		}
	}

	private void HandleMove(InputAction.CallbackContext context)
	{
		Vector2 movementInput = context.ReadValue<Vector2>();
		OnMove?.Invoke(movementInput);
	}

	private void HandleLook(InputAction.CallbackContext context)
	{
		Vector2 lookInput = context.ReadValue<Vector2>();
		OnLook?.Invoke(lookInput);
	}

	private void HandleJumpStart(InputAction.CallbackContext context)
	{
		isJumping = true;
		jumpHoldTime = 0.0f;
	}

	private void handleJumpEnd(InputAction.CallbackContext context)
	{
		isJumping = false;
		jumpHoldTime = 0.0f;
		OnJumpReleased?.Invoke();
	}

	private void HandleUINavigation(InputAction.CallbackContext context)
	{
		Vector2 navigationInput = context.ReadValue<Vector2>();

		if (navigationInput != Vector2.zero)
			OnUINavigate?.Invoke(navigationInput);
    }

	private void HandleEscapePressed(InputAction.CallbackContext context)
	{
		if (context.performed)
			OnEscapePressed?.Invoke();
	}

	private void HandleUISubmit(InputAction.CallbackContext context)
	{
		if (context.performed)
			OnUISubmit?.Invoke();
	}

	private void Awake()
	{
		playerInput = GetComponent<PlayerInput>();

		if (playerInput != null)
		{
			// Player Input
			PlayerActionMap = playerInput.actions.FindActionMap("Player", true);
			Move = PlayerActionMap.FindAction("Move");
			Look = PlayerActionMap.FindAction("Look");
			Jump = PlayerActionMap.FindAction("Jump");

			Move.Enable();
			Look.Enable();
			Jump.Enable();

			// UI Input
			UIActionMap = playerInput.actions.FindActionMap("UI", true);
			UISubmit = UIActionMap.FindAction("Submit");
			UINavigate = UIActionMap.FindAction("Navigate", true);
			UIEscape = UIActionMap.FindAction("Escape", true);
		}
		else
		{
			Debug.LogError("Error: PlayerInput component missing.");
		}

		if (Move != null && Look != null && Jump != null)
		{
			Instance = this;
			Debug.Log("InputHandler: Ready");
		}
	}

	private void OnEnable()
	{
		if (Move != null)
			Move.performed += HandleMove;


		if (Look != null)
			Look.performed += HandleLook;

		if (Jump != null)
		{
			Jump.started += HandleJumpStart;
			//Jump.performed += HandleJump;
			Jump.canceled += handleJumpEnd;
		}

		if (UISubmit != null)
			UISubmit.performed += HandleUISubmit;

		if (UIEscape != null)
			UIEscape.performed += HandleEscapePressed;

		if (UINavigate != null)
			UINavigate.performed += HandleUINavigation;
	}

	private void OnDisable()
	{
		if (Move != null)
			Move.performed -= HandleMove;

		if (Look != null)
			Look.performed -= HandleLook;

		if (Jump != null)
		{
			Jump.started -= HandleJumpStart;
			//Jump.performed -= HandleJump;
			Jump.canceled -= handleJumpEnd;
		}

		if (UISubmit != null)
			UISubmit.performed -= HandleJumpStart; //	to-do: change to standalone event

		if (UIEscape != null)
			UIEscape.performed -= HandleEscapePressed;

		if (UINavigate != null)
			UINavigate.performed -= HandleUINavigation;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {

    }

	void Update()
	{
		MovementInput = Move.ReadValue<Vector2>();
		LookInput = Look.ReadValue<Vector2>();
		UINav = UINavigate.ReadValue<Vector2>();

		if (isJumping)
		{
			jumpHoldTime += Time.deltaTime;
			OnJumpHeld?.Invoke(jumpHoldTime);
		}
	}
}
