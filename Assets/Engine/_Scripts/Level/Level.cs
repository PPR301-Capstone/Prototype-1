using UnityEngine;

public class Level : MonoBehaviour
{
    [Header("Level Info")]
    public string Name = "LevelTemplate";
    public int ID = 0;

    [Header("Music")]
    public AudioClip BGMusic;
    public AudioClip Atmospheric;

    [Header("Lighting")]
    public Color AmbientLighting;
    public float LightIntensity;

    [Header("Config")]
    public PlayerSpawner PlayerSpawn;
    public CameraController CameraController;

    public void StartLevel()
    {
		CameraController = Camera.main.GetComponent<CameraController>();
		Debug.Log("Starting Level: " + Name);

        PlayerSpawn.Spawn();

        GameManager.Instance.player = PlayerSpawn.SpawnPlayer().GetComponent<Player>();
        GameManager.Instance.player.EnableControl();

        CameraController.Instance.SetTarget(GameManager.Instance.player.gameObject);
		CameraController.Instance.SetType(CameraController.CameraType.Follow);
    }

    public void StopLevel()
    {
        GameManager.Instance.player.DisableControl();
        CameraController.SetType(CameraController.CameraType.Static);
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
