using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuController : UIMenuBase
{

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        if (Input.GetButtonDown("A_Button"))
        {
            if (_currentSelection == 0)
                GetComponent<SceneLoader>().LoadByIndex(0);
                
        }
    }
}
