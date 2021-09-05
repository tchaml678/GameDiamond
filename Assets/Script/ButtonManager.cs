using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] GameObject btnResume;
    [SerializeField] GameObject panelMenu;
   

  
    
    private void Start()
    {
        Time.timeScale = 0;
    }
    public void BtnPause()
    {
        Time.timeScale = 0;
        btnResume.SetActive(true);
    }
    public void BtnResume()
    {
        btnResume.SetActive(false);
        Time.timeScale = 1;
    }
    public void BtnBack()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void BtnBackMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void BtnPlay()
    {
        panelMenu.SetActive(false);
        Time.timeScale = 1;      
    }
    public void BtnExit()
    {
        Application.Quit();
    }

}
