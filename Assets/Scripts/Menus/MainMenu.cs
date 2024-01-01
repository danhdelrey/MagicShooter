using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    void Start(){
        Time.timeScale = 1;
    }
    public void NewGame(){
        SceneManager.LoadScene("Level1");
    }

    public void Settings(){

    }

    public void Quit(){
        Application.Quit();
    }
}
