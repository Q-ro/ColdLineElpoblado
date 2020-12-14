using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : UIMenuBase
{

    // Update is called once per frame
    void FixedUpdate()
    {
        //base.Update();

        if (Input.GetButtonDown("A_Button"))
        {
            if (_currentSelection == 0)
                GetComponent<SceneLoader>().LoadByIndex(1);
            else if (_currentSelection == 1)
                GetComponent<SceneLoader>().LoadByIndex(5);
            else if (_currentSelection == 2)
                GetComponent<QuitGame>().Quit();
        }
    }


}
