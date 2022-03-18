using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private int xp = 0;
    [SerializeField] private int requireXp = 100;
    [SerializeField] private int levelBase = 100;
    [SerializeField] private int coins = 100;
    [SerializeField] private List<GameObject> droids = new List<GameObject>();

    private int lvl = 1;
    private string path;

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
        path = Application.persistentDataPath + "/player.dat";
        Load();
    }

    public void AddXp(int xp)
    {
        this.xp += Mathf.Max(0, xp);
        InitLevelData();
        Save();
    }

    public void AddCoins(int coins)
    {
        this.coins += Mathf.Max(0, coins);
    }

    public void AddDroid(GameObject droid)
    {
        this.droids.Add(droid);
        Save();
    }

    private void InitLevelData()
    {
        lvl = (xp / levelBase) + 1;
        requireXp = levelBase * lvl;
    }

    private void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        PlayerData data = new PlayerData(this);
        bf.Serialize(file, data);
        file.Close();
    }

    private void Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            xp = data.Xp;
            requireXp = data.RequiredXp;
            levelBase = data.LevelBase;
            lvl = data.Lvl;

            // import player droids
            droids = data.Droids;


            foreach (DroidData droidData in data.Droids)
            {

                Droid droid = gameObject.AddComponent<Droid>();
            }
        }
        else
        {
            InitLevelData();
        }
    }


} // class
