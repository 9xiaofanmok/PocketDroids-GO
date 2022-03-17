using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text xpText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text coinText;
    [SerializeField] private GameObject menu;
    [SerializeField] private AudioClip menuBtnSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
        Assert.IsNotNull(xpText);
        Assert.IsNotNull(levelText);
        Assert.IsNotNull(coinText);
        Assert.IsNotNull(menu);
        Assert.IsNotNull(menuBtnSound);
        Assert.IsNotNull(audioSource);
    }

    private void Update()
    {
        updateLevel();
        updateXp();
        updateCoin();
    }

    public void updateLevel()
    {
        levelText.text = GameManager.Instance.CurrentPlayer.Lvl.ToString();
    }

    public void updateXp()
    {
        xpText.text = GameManager.Instance.CurrentPlayer.Xp.ToString()
            + " / " + GameManager.Instance.CurrentPlayer.RequiredXp.ToString();
    }

    public void updateCoin()
    {
        coinText.text = GameManager.Instance.CurrentPlayer.Coins.ToString();
    }

    public void MenuBtnClicked()
    {
        audioSource.PlayOneShot(menuBtnSound);
        //toggleMenu();
    }

    private void toggleMenu()
    {
        menu.SetActive(!menu.activeSelf); 
    }


} // class
