using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControl : MonoBehaviour
{
    public GameObject ExitPanel;



    private void Start()
    {
        if(Time.timeScale==0) // Oyun durmu�sa tekrar aktif etmek i�in
        {
            Time.timeScale = 1;
        }

    }

    public void GameStart()
    {
        SceneManager.LoadScene("Level1");
       // SceneManager.LoadScene(PlayerPrefs.GetInt("kaldigiLevel")); // kaldigi yerden devam etmesi i�in
        
    }


    //Exit button i�in
    public void ExitGame()
    {
        ExitPanel.SetActive(true);
    }

    public void Answer(string answer)
    {
        if(answer=="Yes")
        {
            Application.Quit();
        }
        else
        {
            ExitPanel.SetActive(false);
        }

    }
}
