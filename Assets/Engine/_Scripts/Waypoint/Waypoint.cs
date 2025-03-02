using UnityEngine;

public class Waypoint : MonoBehaviour
{
	public float MovementDelay = 1.5f;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, 0.5f);
	}
}
