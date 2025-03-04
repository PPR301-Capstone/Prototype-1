using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelLoadManager : MonoBehaviour
{
    public GameObject[] levels;
    GameObject Parent;
    public bool levelLoaded = false;
    public int currentLevel = 0;

    bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true; // scene is already loaded ( DONT LOAD )
            }
        }
        return false; // Scene is not loaded, we can now load it ( Persistant_UI_Overlay_Hearts_Key )
    }

    public void Load()
    {
        StartCoroutine(LoadLevelAsync(levels[currentLevel].name));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        Debug.Log("Loading: " + levelName);
        ResourceRequest request = Resources.LoadAsync<GameObject>($"Levels/{levelName}");
        yield return request;

        if (request.asset != null)
        {
            Instantiate(request.asset);
            Debug.Log($"Loaded level");
            levelLoaded = true;
        }
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SceneManager.GetSceneByName("Persistant_UI_Overlay_Hearts_Key").isLoaded == false)
        {
            SceneManager.LoadScene("Persistant_UI_Overlay_Hearts_Key", LoadSceneMode.Additive);
        }

        Parent = GameObject.Find("Level");

        //Load();
    }
}
