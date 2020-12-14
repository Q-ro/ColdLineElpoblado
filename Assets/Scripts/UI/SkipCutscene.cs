using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SceneLoader))]
public class SkipCutscene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("A_Button"))
        {
            this.GetComponent<SceneLoader>().LoadByIndex(2);
        }
    }
}
