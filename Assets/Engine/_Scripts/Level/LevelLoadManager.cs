using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;

public class LevelLoadManager : MonoBehaviour
{
    public string levelPrefabAddress;
    GameObject Parent;
    public bool levelLoaded = false;

    public void Load()
    {
        StartCoroutine(LoadLevelAsync());
    }

    private IEnumerator LoadLevelAsync()
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(levelPrefabAddress);

        yield return handle;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject levelInstance = Instantiate(handle.Result);

            levelInstance.transform.parent = Parent.transform;
            Debug.Log($"Level loaded: {handle.DebugName}");

            levelLoaded = true;
        }
        else
        {
            Debug.LogError("Failed to load level: " + levelPrefabAddress);
        }

        Addressables.Release(handle);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Parent = GameObject.Find("Level");
        //Load();
    }
}
