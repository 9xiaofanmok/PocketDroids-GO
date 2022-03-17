using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private int xp = 0;
    [SerializeField] private int requireXp = 100;
    [SerializeField] private int levelBase = 100;
    [SerializeField] private int coins = 100;
    [SerializeField] private List<GameObject> droids = new List<GameObject>();

    private int lvl = 1;

    public int Xp
    {
        get { return xp; }
    }

    public int RequiredXp
    {
        get { return requireXp; }
    }

    public int LevelBase
    {
        get { return levelBase; }
    }

    public int Coins
    {
        get { return coins; }
    }

    public List<GameObject> Droids
    {
        get { return droids; }
    }

    public int Lvl
    {
        get { return lvl; }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitLevelData();
    }

    public void AddXp(int xp)
    {
        this.xp += Mathf.Max(0, xp);
    }

    public void AddCoins(int coins)
    {
        this.coins += Mathf.Max(0, coins);
    }

    public void AddDroid(GameObject droid)
    {
        this.droids.Add(droid);
    }

    private void InitLevelData()
    {
        lvl = (xp / levelBase) + 1;
        requireXp = levelBase * lvl;
    }
    
} // class
