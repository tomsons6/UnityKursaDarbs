using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    Canvas MainMenu;
    bool start = false;


	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0) && start == true)
        {
            SceneManager.LoadScene(1);
        }
	}
    public void Play()
    {
        MainMenu = Canvas.FindObjectOfType<Canvas>();
        start = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    
}
