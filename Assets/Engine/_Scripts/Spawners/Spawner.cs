using UnityEngine;
using System;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] float delay = 0.0f;
    [SerializeField] GameObject objectToSpawn;
    public Color DebugColour = Color.yellow;

    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(delay);
    }

    public virtual void Spawn()
    {
        if (objectToSpawn != null)
            StartCoroutine(SpawnRoutine());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnDrawGizmos()
	{
        Gizmos.color = DebugColour;
        Gizmos.DrawWireCube(this.transform.position, Vector3.one * 2f);
	}
}
