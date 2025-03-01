using UnityEngine;
using System.Collections;

public class EnemyAgent : Agent
{
    public enum AgentState
    {
        Idle,
        Patrol,
        Hunt,
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
    [SerializeField] AreaSensor playerTrigger;
    [SerializeField] RaySensor eyeLine;

    [Header("Detection Protocol")]
    [SerializeField] float detectTime = 2.5f;

    public GameObject target;
    public bool isPlayerDetected = false;
    public bool isChasingPlayer = false;
    public bool isWithinRange = false;

    AgentState currentState;

    //  Movement


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

            Debug.Log($"{this.name}: {currentState}");
			yield return new WaitForSeconds(pollInterval);
		}
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
        currentState = defaultState;
        isActive = true;
        StartCoroutine(BehaviourPoll());
    }

    // Update is called once per frame
    void Update()
    {
        HandlePatrol();
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
