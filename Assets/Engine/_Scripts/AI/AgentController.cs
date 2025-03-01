using UnityEngine;

public class AgentController : MonoBehaviour
{
    Rigidbody2D rb2d;

    [Header("Movement Config")]
    [SerializeField] float movementSpeed = 3.0f;
    [SerializeField] float movementDecay = 0.5f;
    [SerializeField] float distanceThreshold = 0.5f;

    private bool isMoving = false;
    public Vector3 targetDestination = Vector3.zero;
    Vector3 currentForce = Vector3.zero;

    public void SetDestination(Vector3 destination)
    {
        targetDestination = destination;
        Debug.DrawLine(this.transform.position, targetDestination, Color.red, 1.0f);
    }

    public bool hasReachedDestination()
    {
        return (Vector3.Distance(this.transform.position, targetDestination) <= distanceThreshold);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        currentForce = (this.transform.position - targetDestination).normalized;

        if (hasReachedDestination())
        {
            isMoving = false;
            currentForce *= movementDecay;
        }
        else
        {
            isMoving = true;
        }

        Debug.DrawLine(this.transform.position, this.transform.position + currentForce, Color.magenta);
    }

	private void FixedUpdate()
	{
		rb2d.AddForce(currentForce * movementSpeed * Time.deltaTime);
	}
}
