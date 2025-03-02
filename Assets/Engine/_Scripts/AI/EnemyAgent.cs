using UnityEngine;
using System.Collections;

public class EnemyAgent : Agent
{
    public enum AgentState
    {
        Idle,
        Patrol,
        Hunt,
        Attack
    }

    //  Agent Config
    [Header("Agent Config")]
    [SerializeField] AgentState defaultState;
    [SerializeField] Waypoint[] PatrolPoints;
    int patrolIndex = -1;
    [SerializeField] float pollInterval;
    [SerializeField] string[] targets;

    AgentController agentController;

    private bool isActive = false;

    //  Sensors
    [Header("Sensors")]
    [SerializeField] AreaSensor playerTrigger;
    [SerializeField] RaySensor eyeLine;

    [Header("Detection Protocol")]
    [SerializeField] float detectTime = 2.5f;

    public GameObject target;
    public bool isPlayerDetected = false;
    public bool isChasingPlayer = false;
    public bool isWithinRange = false;


    private bool isAttacking = false;
    public float attackCooldown = 1.5f;

    AgentState currentState;

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
					HandlePatrol();
				}
            }

            if (currentState == AgentState.Patrol)
            {

            }

            if (currentState == AgentState.Hunt)
            {
                
            }

            if (currentState == AgentState.Hunt && isWithinRange)
            {
                currentState = AgentState.Attack;

                if (!isAttacking)
                    StartCoroutine(AttackPlayer());
            }

            Debug.Log($"{this.name}: {currentState}");
            yield return new WaitForSeconds(pollInterval);
        }
    }

    IEnumerator AttackPlayer()
    {
        if (isAttacking) yield break;

        isAttacking = true;
        //StopMovement();

        if (target != null)
        {
            PlayerController player = target.GetComponent<PlayerController>();

            if (player != null)
            {
                Debug.Log($"{this.name} attacked Player");
                player.TakeDamage(1);
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    IEnumerator PatrolRoutine()
    {
		agentController.SetDestination(PatrolPoints[patrolIndex]);

		while (!agentController.reachedDestination)
        {
			yield return null;
		}

        Debug.Log($"{this.name} reached: {PatrolPoints[patrolIndex].name}");
        ResetState();

        yield return new WaitForSeconds(PatrolPoints[patrolIndex].MovementDelay);
    }

    IEnumerator HuntRoutine()
    {
        yield return null;
    }

    public void ResetState()
    {
        currentState = defaultState;
    }

    void HandlePatrol()
    {
        Debug.Log($"{this.name}: state: {currentState}");

        if (currentState != AgentState.Patrol)
            return;

		patrolIndex = (patrolIndex + 1) % PatrolPoints.Length;

		StartCoroutine(PatrolRoutine());
	}

    void HandleHunt()
    {
        if (currentState != AgentState.Hunt)
            return;

        agentController.reachedDestination = true;
        isChasingPlayer = true;
        StopCoroutine(PatrolRoutine());
        agentController.SetDestination(target.transform.position);
    }

    void HandleDetection(float duration)
    {
        if (duration >= detectTime)
        {
            Debug.Log("Target detected");
            currentState = AgentState.Hunt;

            target = playerTrigger.triggeredObject;
            HandleHunt();
        }
    }

    void HandleDetection()
    {
        if (playerTrigger.isTriggered)
        {

        }
        else
        {
            //if (Vector3.Distance(this.transform.position, ))
            currentState = AgentState.Idle;
            isChasingPlayer = false;
            target = null;
        }
    }

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        agentController = GetComponent<AgentController>();

        currentState = defaultState;
        isActive = true;
        StartCoroutine(BehaviourPoll());
    }

    // Update is called once per frame
    void Update()
    {
        
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
