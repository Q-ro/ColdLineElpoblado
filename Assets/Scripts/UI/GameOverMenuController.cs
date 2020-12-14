using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuController : UIMenuBase
{

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (Input.GetButtonDown("A_Button"))
        {
            if (_currentSelection == 0)
                GetComponent<SceneLoader>().LoadByIndex(2);
            else if (_currentSelection == 1)
                GetComponent<SceneLoader>().LoadByIndex(0);
                
        }
    }
}
