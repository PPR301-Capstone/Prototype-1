using System;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    public enum SensorType
    {
        GROUND,
        ENEMY,
        OBSTACLE
    }

    public SensorType type;
    [SerializeField] Vector2 RayDirection = Vector2.zero;
    [SerializeField] float RayDistance = 1.0f;
    [SerializeField] LayerMask DetectionLayers;

    BoxCollider2D bc;

    public bool ObstacleDetected = false;

    //  Sensor Events
    public event Action<bool> OnDetectGround;
    public event Action<bool> OnDetectObstacle;

    void DispatchEvent()
    {
		switch (type)
		{
			// Special case for ground - for sound events
			case SensorType.GROUND:
                OnDetectGround?.Invoke(ObstacleDetected);
				break;
			case SensorType.OBSTACLE:
                OnDetectObstacle?.Invoke(ObstacleDetected);
				break;
		}
	}

    void HandleRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDirection.normalized, RayDistance, DetectionLayers);

        if (hit.collider != null)
        {
            ObstacleDetected = true;
        }
        else
        {
            ObstacleDetected = false;
        }


		Debug.DrawRay(transform.position, RayDirection.normalized * RayDistance, (ObstacleDetected) ? Color.red : Color.green);
	}

    public void IsTriggered()
    {
        
    }

	private void Awake()
	{
	    bc = GetComponent<BoxCollider2D>();	
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleRay();
        DispatchEvent();
    }

	private void OnDrawGizmos()
	{
		//Debug.DrawLine(this.transform.position, this.transform.position + (Vector3)RayDirection * 1.0f, Color.magenta);
	}
}
