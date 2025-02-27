using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController playerController;

    public void EnableControl()
    {
        playerController.IsPlayerControlDisabled = false;
    }

    public void DisableControl()
    {
		playerController.IsPlayerControlDisabled = true;
	}

	private void Awake()
	{
		playerController = GetComponent<PlayerController>();
	}
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
