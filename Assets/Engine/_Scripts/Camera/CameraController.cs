using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

	public enum CameraType
	{
		Static,
		Follow,
		Cinematic
	}

	[Header("Follow Config")]
	[SerializeField] private GameObject Target;
	[SerializeField] private CameraType cameraType;
	[SerializeField] private float followSpeed = 5.0f;

	[Header("Curves")]
	[SerializeField] AnimationCurve EaseIn;

	private Vector3 targetPosition;
	private Vector3 velocity = Vector3.zero;

	private bool isMoving = false;
	private bool isCameraShake = false;
	Vector2 shakeOffset = Vector2.zero;

	public void SetTarget(GameObject target)
	{
		this.Target = target;
	}

	public void SetType(CameraType type)
	{
		this.cameraType = type;
	}

	void Start()
	{
		Instance = this;
	}

	void Update()
	{
		if (Target != null && cameraType == CameraType.Follow)
		{
			Follow();
		}
	}

	private void Follow()
	{
		targetPosition = Target.transform.position;

		Vector3 finalPosition = isCameraShake ? targetPosition + (Vector3)shakeOffset : targetPosition;

		float arrivalDamping = Mathf.Clamp01(Vector3.Distance(transform.position, targetPosition) / 0.5f);
		float shapedDamping = EaseIn.Evaluate(arrivalDamping);

		finalPosition.z = -10; // Set z position to prevent 2D plane being within nearClip plane

		transform.position = Vector3.Lerp(transform.position, finalPosition, shapedDamping * followSpeed * Time.unscaledDeltaTime);
	}
}
