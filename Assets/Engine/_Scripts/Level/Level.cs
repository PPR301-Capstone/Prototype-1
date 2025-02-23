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
    public GameObject SpawnerParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
