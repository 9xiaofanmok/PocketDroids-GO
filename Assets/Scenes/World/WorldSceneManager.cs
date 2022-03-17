using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldSceneManager : PocketDroidsSceneManager
{

    private GameObject droid;
    private AsyncOperation loadScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void droidTapped(GameObject droid)
    {
        //SceneManager.LoadScene(PocketDroidsConstants.SCENE_CAPTURE, LoadSceneMode.Additive);
        List<GameObject> list = new List<GameObject>();
        list.Add(droid);
        SceneTransitionManager.Instance.GoToScene(PocketDroidsConstants.SCENE_CAPTURE, list);
    }

    public override void playerTapped(GameObject player)
    {
    }

    public override void coinTapped(GameObject coin)
    {

    }
}
