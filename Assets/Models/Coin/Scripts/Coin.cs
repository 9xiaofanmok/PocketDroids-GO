using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    private void OnMouseDown()
    {
        PocketDroidsSceneManager[] managers = FindObjectsOfType<PocketDroidsSceneManager>();
        foreach (PocketDroidsSceneManager pocketDroidsSceneManager in managers)
        {
            if (pocketDroidsSceneManager.gameObject.activeSelf)
            {
                pocketDroidsSceneManager.coinTapped(this.gameObject);
            }
        }
    }

    private void Start()
    {
    }
}
