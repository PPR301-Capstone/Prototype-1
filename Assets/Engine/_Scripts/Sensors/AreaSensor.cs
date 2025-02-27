using System;
using System.Collections;
using UnityEngine;

public class AreaSensor : Sensor
{
    CircleCollider2D cc;
	[SerializeField] float radius = 1.0f;
	public float triggerTime = 0.0f;

	//	Events
	public event Action OnTriggerStart;
	public event Action<float> OnTriggerStay;
	public event Action OnTriggerEnd;

	private void Awake()
	{
		cc = GetComponent<CircleCollider2D>();
	}

	private void OnValidate()
	{
		if (cc != null)
			cc.radius = radius;
	}

	IEnumerator StartTriggerTimer()
	{
		while (base.ObstacleDetected)
		{
			triggerTime += Time.deltaTime;
			yield return null;
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
			ObstacleDetected = true;
			OnTriggerStart?.Invoke();
			StartCoroutine(StartTriggerTimer());
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (CheckLayers(collision.gameObject) && CheckTag(collision.gameObject))
		{
			ObstacleDetected = true;
			OnTriggerStay?.Invoke(triggerTime);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (CheckLayers(collision.gameObject) && CheckTag(collision.gameObject))
		{
			ObstacleDetected = false;
			StopCoroutine(StartTriggerTimer());
			OnTriggerEnd?.Invoke();
		}
	}
}
