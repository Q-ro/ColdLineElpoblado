using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputMenuSelection : MonoBehaviour
{

    #region Inspector Properties

    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject selectedObject;

    #endregion

    #region Private Properties

    private bool buttonSelected;

    #endregion

    void Start()
    {
        // eventSystem.SetSelectedGameObject(null);
        // eventSystem.SetSelectedGameObject(selectedObject);
        // selectedObject.GetComponent<Button>().OnSelect(null);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0 && buttonSelected == false)
        {
            eventSystem.SetSelectedGameObject(selectedObject);
            buttonSelected = true;
        }
    }

    private void OnDisable()
    {
        buttonSelected = false;
    }

    void OnEnable()
    {
    }

    public void ShowGameoverMenu()
    {
        this.gameObject.SetActive(true);
        // eventSystem.SetSelectedGameObject(selectedObject);
        // selectedObject.GetComponent<Button>().OnSelect(null);

    }
}
