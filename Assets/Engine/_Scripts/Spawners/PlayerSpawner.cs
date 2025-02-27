using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : Spawner
{
    [SerializeField] GameObject Player;
	
	public PlayerSpawner() : base()
	{
		base.DebugColour = Color.green;
	}

	public GameObject SpawnPlayer()
	{
		GameObject spawnedPlayer = Instantiate(Player, this.transform);
		PlayerInput playerInput = spawnedPlayer.GetComponent<PlayerInput>();

		if (playerInput != null)
		{
			playerInput.ActivateInput();
		}
		else
		{
			Debug.LogError("Player instantiated with no PlayerInput component!");
		}

        spawnedPlayer.transform.parent = null;

		return spawnedPlayer;
	}
}
