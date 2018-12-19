using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenController : MonoBehaviour
{
    bool reload = false;
    // Update is called once per frame
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && reload == true)
        {
            SceneManager.LoadScene(1);
        }
    }
    public void restart()
    {
        reload = true;
    }
    public void quit()
    {
        Application.Quit();
    }
}