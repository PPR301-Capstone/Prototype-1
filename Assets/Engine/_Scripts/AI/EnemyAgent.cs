using UnityEngine;
using System.Collections;

public class EnemyAgent : Agent
{
    public enum AgentState
    {
        Idle,
        Patrol,
        Hunt,
        Attack // New attack stake                              *NEW*
    }

    //  Agent Config
    [Header("Agent Config")]
    [SerializeField] AgentState defaultState;
    [SerializeField] Vector2[] PatrolPoints;
    int patrolIndex = 0;
    [SerializeField] float pollInterval;
    [SerializeField] string[] targets;

    AgentController agentController;

    private bool isActive = false;

    //  Sensors
    [Header("Sensors")]
    [SerializeField] AreaSensor playerTrigger; // using this for attack aswell :)
    [SerializeField] RaySensor eyeLine;

    [Header("Detection Protocol")]
    [SerializeField] float detectTime = 2.5f;

    public GameObject target;
    public bool isPlayerDetected = false;
    public bool isChasingPlayer = false;
    public bool isWithinRange = false;

    // bool and float for attack enum                           *NEW*
    private bool isAttacking = false;
    public float attackCooldown = 1.5f;

    AgentState currentState;

    //  Movement
    Rigidbody2D rb2d; // store rigidbody ref for later      *NEW*

    IEnumerator BehaviourPoll()
    {
        while (isActive)
        {
            if (currentState == AgentState.Idle)
            {
                if (isPlayerDetected)
                {
                    if (!isChasingPlayer)
                        isChasingPlayer = true;
                }
                else
                {
                    currentState = AgentState.Patrol;
                }
            }

            if (currentState == AgentState.Hunt && isWithinRange) // if agent is hunting and is within range it will attack           **NEW**
            {
                currentState = AgentState.Attack;
                StartCoroutine(AttackPlayer()); // Attack when in range
            }

            Debug.Log($"{this.name}: {currentState}");
            yield return new WaitForSeconds(pollInterval);
        }
    }

    IEnumerator AttackPlayer()
    {
        if (isAttacking) yield break; // prevents multiple attack sequences at the same time            *NEW*

        isAttacking = true;
        StopMovement(); // stops enemy movement when attacking      **NEW** so the player might have a chance to avoid or dodge instead of being chased and attacked

        if (target != null)
        {
            PlayerController player = target.GetComponent<PlayerController>();

            if (player != null)
            {
                Debug.Log($"{this.name} attacked Player");
                player.TakeDamage(1); // Inflicts Damage to player at the cost of 1 single life at a time               *NEW*    **requires Damage system**
            }
        }

        yield return new WaitForSeconds(attackCooldown); // so they enemy doesnt spring multi attacks before the player can move away
        isAttacking = false;

    }

    void StopMovement() // stops enemy movement when called during attack          *NEW*
    {
        rb2d.linearVelocity = Vector2.zero; // stop rigid body movement
        agentController.enabled = false; // Disable movement logic
    }
   
    void ResumeMovement()
    {
        agentController.enabled = true; // resume mvoement logic
    }




    void HandlePatrol()
    {
        if (currentState != AgentState.Patrol)
            return;

        if (agentController.targetDestination == null)
        {
            agentController.SetDestination(PatrolPoints[patrolIndex]);
        }
        else
        {

        }
    }

    void HandleDetection(float duration)
    {
        if (duration >= detectTime)
        {
            currentState = AgentState.Hunt;
        }
    }

    void HandleDetection()
    {
        currentState = AgentState.Idle;
    }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        agentController = GetComponent<AgentController>();
        rb2d = GetComponent<Rigidbody2D>(); // initialise rigidbody on start
        currentState = defaultState;
        isActive = true;
        StartCoroutine(BehaviourPoll());
    }

    // Update is called once per frame
    void Update()
    {
        HandlePatrol();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        if (other.CompareTag("Player"))
        {

            target = other.gameObject; // set the player as the target
            isWithinRange = true; // enemy can now attack the player        *NEW*

        }

    }

    private void OnTriggerExit2D(Collider2D other) // detect when player leaves enemy range ( IF AT ALL )       *NEW* redunant check
    {

        if (other.CompareTag("Player"))
        {
            isWithinRange = false; // enemy stops attack and returns to hunting otherwise theyll keep attacking even if the player isnt within range ( look weird )
            currentState = AgentState.Hunt; //  Return to Hunt stage so that the enemy can return to attacking when the player enters range again              *NEW*
        }

    }



    private void FixedUpdate()
	{
		
	}

	private void OnEnable()
	{
        playerTrigger.OnTriggerStay += HandleDetection;
        playerTrigger.OnTriggerEnd += HandleDetection;
	}

	private void OnDisable()
	{
		playerTrigger.OnTriggerStay -= HandleDetection;
        playerTrigger.OnTriggerEnd -= HandleDetection;
	}
}
