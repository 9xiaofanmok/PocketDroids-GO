using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DroidFactory : Singleton<DroidFactory>
{
    [SerializeField] private Droid[] availableDroids;
    [SerializeField] private Player player;
    [SerializeField] private float waitTime = 180.0f;
    [SerializeField] private int startingDroids = 5;
    [SerializeField] private float minRange = 5.0f;
    [SerializeField] private float maxRange = 50.0f;

    // to persist data btw scenes
    private List<Droid> liveDroids = new List<Droid>();
    private Droid selectedDroid;

    public List<Droid> LiveDroids
    {
        get { return liveDroids; }
    }

    public Droid SelectedDroid
    {
        get { return selectedDroid; }
    }

    private void Awake()
    {
        Assert.IsNotNull(availableDroids);
        Assert.IsNotNull(player);
    }

    void Start()
    {
        for (int i = 0; i < startingDroids; i++)
        {
            InstantiateDroids();
        }

        StartCoroutine(GenerateDroids());
    }

    public void DroidSelected(Droid droid)
    {
        selectedDroid = droid;
    }

    // have a coroutine to generate droids every 3mins
    private IEnumerator GenerateDroids()
    {
        InstantiateDroids();
        yield return new WaitForSeconds(waitTime);
    }

    private void InstantiateDroids()
    {
        int index = Random.Range(0, availableDroids.Length);
        float x = player.transform.position.x + GenerateRange();
        float y = player.transform.position.y;
        float z = player.transform.position.z + GenerateRange();

        liveDroids.Add(Instantiate(availableDroids[index], new Vector3(x, y, z), Quaternion.identity));
    }

    private float GenerateRange()
    {
        float randomNum = Random.Range(minRange, maxRange);

        // give a 50% change of whether number is positive or negative
        // this is so that we can get negative range around the radius of the player as well
        bool isPositive = Random.Range(0, 10) < 5;
        return randomNum * (isPositive ? 1 : -1);
    }

}
