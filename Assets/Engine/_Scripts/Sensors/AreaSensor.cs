using System;
using System.Collections;
using UnityEngine;

public class AreaSensor : Sensor
{
    CircleCollider2D cc;
	public float triggerTime = 0.0f;

	GameObject triggeredObject;

	//	Events
	public event Action OnTriggerStart;
	public event Action<float> OnTriggerStay;
	public event Action OnTriggerEnd;

	private void Awake()
	{
		cc = GetComponent<CircleCollider2D>();
	}

	IEnumerator StartTriggerTimer()
	{
		while (base.isTriggered)
		{
			yield return null;
			triggerTime += Time.deltaTime;
		}
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (CheckLayers(collision.gameObject) && CheckTag(collision.gameObject))
		{
			isTriggered = true;
			OnTriggerStart?.Invoke();
			triggeredObject = collision.gameObject;
			StartCoroutine(StartTriggerTimer());
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (CheckLayers(collision.gameObject) && CheckTag(collision.gameObject))
		{
			isTriggered = true;

			Debug.Log(triggerTime);
			OnTriggerStay?.Invoke(triggerTime);
			Debug.DrawLine(this.transform.position, triggeredObject.transform.position, Color.magenta);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (CheckLayers(collision.gameObject) && CheckTag(collision.gameObject))
		{
			isTriggered = false;
			StopCoroutine(StartTriggerTimer());
			OnTriggerEnd?.Invoke();
			triggeredObject = null;
		}
	}

	private void OnDrawGizmos()
	{
		if (cc != null)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(this.transform.position, cc.radius);
		}
	}
}
