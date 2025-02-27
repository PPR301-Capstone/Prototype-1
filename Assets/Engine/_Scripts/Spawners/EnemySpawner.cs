using System.Collections;
using UnityEngine;

public class EnemySpawner : Spawner
{
	public EnemySpawner() : base()
	{
		base.DebugColour = Color.red;
	}

	[SerializeField] GameObject[] enemies;
	[SerializeField] bool spawnOnActive = true;
	[SerializeField] float spawnDelay = 0.5f;

    public void SpawnEnemy()
    {
        int random = Random.Range(0, enemies.Length);

        GameObject spawnedEnemy = Instantiate(enemies[random]);
    }

	IEnumerator SpawnRoutine()
	{
		yield return new WaitForSeconds(spawnDelay);

		SpawnEnemy();
	}

	private void Start()
	{
		StartCoroutine(SpawnRoutine());
	}
}
