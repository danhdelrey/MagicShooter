using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManger : MonoBehaviour
{
    public static MenuManger menuMangerInstance;

    public GameObject pauseMenu;
    private bool isPause = false;

    public GameObject deadMenu;
    public GameObject winMenu;

    void Awake(){
        menuMangerInstance = this;
    }

    void Start(){
        Time.timeScale = 1;
    }

    void Update(){
        showPauseMenu();
    }

    void showPauseMenu(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!isPause){
                Player.playerInstance.disableAttachToMouse();
                Cursor.visible = true;
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                isPause = true;
            }else{
                Player.playerInstance.enableAttachToMouse();
                Cursor.visible = false;
                Time.timeScale = 1f;
                pauseMenu.SetActive(false);
                isPause = false;
            }
        }
    }

    public void showDeadMenu(){
        Player.playerInstance.disableAttachToMouse();
        Cursor.visible = true;
        Time.timeScale = 0f;
        deadMenu.SetActive(true);
        
    }

    public void showWinMenu(){
        Player.playerInstance.disableAttachToMouse();
        Cursor.visible = true;
        Time.timeScale = 0f;
        winMenu.SetActive(true);
    }

    public void Replay(){
        SceneManager.LoadScene("Level1");
    }
    public void NewGame(){
        SceneManager.LoadScene("Level1");
    }
    public void MainMenu(){
        SceneManager.LoadScene("MainMenu");
    }
    public void Quit(){
        Application.Quit();
    }
    public void Continue(){
        Player.playerInstance.enableAttachToMouse();
        Cursor.visible = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPause = false;
    }

}
