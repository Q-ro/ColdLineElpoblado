using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] GameObject objectToFollow;

    #endregion

    void Update()
    {
        //Follow the object Along the X axis
        this.transform.position = new Vector3(objectToFollow.transform.position.x,
                                                        objectToFollow.transform.position.y,
                                                        this.transform.position.z);
    }
}
