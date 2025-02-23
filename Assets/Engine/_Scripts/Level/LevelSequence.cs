using System.Collections;
using UnityEngine;

public class LevelSequence : MonoBehaviour
{
    //  Components
    LevelLoadManager LevelLoadManager;
    UIOverlay uiOverlay;

    IEnumerator StartLevelSequence()
    {
		LevelLoadManager.Load();

        while (!LevelLoadManager.levelLoaded)
        {
			yield return null;
		}
		
        uiOverlay.Hide();
    }

    public void LoadLevel()
    {
        StartCoroutine(StartLevelSequence());
    }

    public void StartLevel()
    {

    }

	void Awake()
	{
		LevelLoadManager = GetComponent<LevelLoadManager>();
        uiOverlay = GameObject.FindFirstObjectByType<UIOverlay>();
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
