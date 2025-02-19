using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public enum Scenes
    {
        Initialize,
        MainMenu,
        Level,
    }

    public static SceneManager Instance;

    public string NextScene;
    public string CurrentScene;

    IEnumerator ChangeSceneAsync()
    {
        yield return new WaitForSeconds(1); // to-do: wait until level loaded
    }

    public void ChangeScene(int ID)
    {

    }


	private void Awake()
	{
		Instance = this;
	}

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }
}
