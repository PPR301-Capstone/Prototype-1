using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSceneManager : MonoBehaviour
{
    public enum Scenes
    {
        Initialize,
        MainMenu,
        Level,
    }

    public static GameSceneManager Instance;

    public string NextScene;
    public string CurrentScene;

    IEnumerator ChangeSceneAsync()
    {
        yield return new WaitForSeconds(1); // to-do: wait until level loaded

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextScene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        CurrentScene = NextScene;
    }

    public void ChangeScene(Scenes sceneID)
    {
        Debug.Log($"GameSceneManager: Changing Scene [{sceneID}]");
        NextScene = sceneID.ToString();
        StartCoroutine(ChangeSceneAsync());
    }


	private void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }
}
