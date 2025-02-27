using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController playerController;

    public void EnableControl()
    {
		Debug.Log("Player enabled");
        playerController.IsPlayerControlDisabled = false;
    }

    public void DisableControl()
    {
		Debug.Log("Player disabled");
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

	private void OnEnable()
	{
		GameManager.Instance.OnPlayerEnable += EnableControl;
		GameManager.Instance.OnPlayerDisable += DisableControl;
	}

	private void OnDisable()
	{
		if (GameManager.Instance != null)
		{
			GameManager.Instance.OnPlayerEnable -= EnableControl;
			GameManager.Instance.OnPlayerDisable -= DisableControl;
		}
	}
}
