using UnityEngine;

public class AgentController : MonoBehaviour
{
	Rigidbody2D rb2d;
	Enemy enemy;


	[Header("Movement Config")]
	[SerializeField] float movementSpeed = 3.0f;
	[SerializeField] float movementDecay = 0.5f;
	[SerializeField] float distanceThreshold = 0.1f;

	private bool isMoving = false;
	public Vector3 targetDestination;
	private Waypoint currentWaypoint;

	private Vector3 moveDirection;
	public bool reachedDestination = true;

	EnemyAnim spriteAnim;

	public void SetDestination(Waypoint destination)
	{
		if (destination == null)
		{
			currentWaypoint = null;
			targetDestination = Vector3.zero;
			return;
		}

		currentWaypoint = destination;

		Debug.Log($"Moving to: {destination.name}: {destination.transform.position}");
		SetDestination(destination.transform.position);
	}

	public void SetDestination(Vector3 destination)
	{
		targetDestination = destination;

		moveDirection = (destination - transform.position).normalized;
		reachedDestination = false;
		isMoving = true;

		Debug.DrawLine(this.transform.position, destination, Color.red, 1.5f);
	}

	public bool HasReachedDestination()
	{
		if (targetDestination != null)
			return Vector3.Magnitude(transform.position - targetDestination) <= distanceThreshold;

		return true;
	}

	public void Attack(bool attackState)
	{
		enemy.CurrentWeapon.Attack();
	}

	public void StopMovement()
	{
		isMoving = false;
		moveDirection = Vector3.zero;
	}

	// Start is called before the first frame update
	void Start()
	{
		enemy = GetComponent<Enemy>();
		spriteAnim = GetComponent<EnemyAnim>();
		rb2d = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (HasReachedDestination())
		{
			StopMovement();
			reachedDestination = true;
		}

		
		spriteAnim.FlipSprite(moveDirection);
		spriteAnim.PlayWalk(isMoving);

		Debug.DrawLine(this.transform.position, this.transform.position + (moveDirection * movementSpeed), Color.yellow);
	}

	void FixedUpdate()
	{
		if (this.GetComponent<EnemyAgent>().currentState == EnemyAgent.AgentState.Dead)
			return;

		if (isMoving && !this.GetComponent<EnemyAgent>().isAttacking)
		{
			rb2d.AddForce(moveDirection * movementSpeed);
		}
		else
		{
			rb2d.linearVelocity *= movementDecay;
		}
	}
}
