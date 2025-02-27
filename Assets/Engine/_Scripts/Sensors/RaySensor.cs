using UnityEngine;
using UnityEngine.UIElements;

public class RaySensor : Sensor
{

	[SerializeField] Vector2 RayDirection = Vector2.zero;
	[SerializeField] float RayDistance = 1.0f;

	void HandleRay()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, RayDirection.normalized, RayDistance, base.DetectionLayers);

		if (hit.collider != null)
		{
			
			isTriggered = true;
		}
		else
		{
			isTriggered = false;
		}

		base.DispatchEvent();

		Debug.DrawRay(transform.position, RayDirection.normalized * RayDistance, (isTriggered) ? Color.red : Color.green);
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		HandleRay();
	}
}
