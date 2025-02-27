using UnityEngine;
using System.Collections;

public class LevelLoadManager : MonoBehaviour
{
    public GameObject[] levels;
    GameObject Parent;
    public bool levelLoaded = false;
    public int currentLevel = 0;

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
        Parent = GameObject.Find("Level");
        //Load();
    }
}
