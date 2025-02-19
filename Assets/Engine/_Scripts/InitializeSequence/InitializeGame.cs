using UnityEngine;
using System.Collections;

public class InitializeGame : MonoBehaviour
{
    bool loadGame = false;

    IEnumerator Poll()
    {
        yield return new WaitForSeconds(0.25f);
        Debug.Log("Changing Scene");
        GameSceneManager.Instance.ChangeScene(GameSceneManager.Scenes.MainMenu);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Poll());
    }
}
