using UnityEngine;

public class README_Brennan : MonoBehaviour
{


      // I have altered the LevelLoadManager to account for (Check Below)


    /* bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i<SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
           if (scene.name == sceneName)
           {
               return true; // scene is already loaded ( DONT LOAD )
           }
       }
    return false; // Scene is not loaded, we can now load it ( Persistant_UI_Overlay_Hearts_Key )
    }
    }


     void Start()
     {
        if (SceneManager.GetSceneByName("Persistant_UI_Overlay_Hearts_Key").isLoaded == false)
        {
            SceneManager.LoadScene("Persistant_UI_Overlay_Hearts_Key", LoadSceneMode.Additive);
        }
    
        Parent = GameObject.Find("Level");
    
        //Load();
    }



      this is the only thing ive changed in this script :)

    */


    // Also PLAYERCONTROLLER added functionality for health system + KeySystem added *****NEW***** to all parts ive added plus some comments



    // player death logic (TODO)
    // Health Pickups (TODO)
    // Damage Animation (TODO) perhaps just add a red version of the player sprite as a quick animation change



}