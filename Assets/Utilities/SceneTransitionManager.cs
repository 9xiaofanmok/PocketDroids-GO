using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : Singleton<SceneTransitionManager>
{

    private AsyncOperation sceneAsync;

    public void GoToScene(string sceneName, List<GameObject> objectsToMove)
    {
        StartCoroutine(LoadScene(sceneName, objectsToMove)); 
    }

    private IEnumerator LoadScene(string sceneName, List<GameObject> objectsToMove)
    {
        // load new scene asynchronously
        SceneManager.LoadSceneAsync(sceneName);

        // trigger event when scene has been loaded
        SceneManager.sceneLoaded += (newScene, mode) =>
        {
            // set active scene to the new scene
            SceneManager.SetActiveScene(newScene);
        };

        Scene sceneToLoad = SceneManager.GetSceneByName(sceneName);

        foreach(GameObject obj in objectsToMove)
        {
            // move game objects to the new scene
            SceneManager.MoveGameObjectToScene(obj, sceneToLoad);
        }

        yield return null;
    }
}
