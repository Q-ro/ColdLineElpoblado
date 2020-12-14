using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuBase : MonoBehaviour
{

    #region Inspector Properties

    [SerializeField] protected Text[] ArrowTextOptions;

    #endregion

    protected int _currentSelection = 0;
    protected bool _axisKeyDown = false;

    // Use this for initialization
    void Start()
    {
        SelectMeunItem();
    }

    protected void Update()
    {
        if (Input.GetAxisRaw("Vertical") != 0)
        {
            if (!_axisKeyDown)
            {
                _axisKeyDown = true;

                UnselectMeunItem();

                if (Input.GetAxisRaw("Vertical") > 0)
                    _currentSelection = Util.wrap(_currentSelection - 1, 0, ArrowTextOptions.Length - 1);
                if (Input.GetAxisRaw("Vertical") < 0)
                    _currentSelection = Util.wrap(_currentSelection + 1, 0, ArrowTextOptions.Length - 1);

                SelectMeunItem();
            }
        }
        else
        {
            _axisKeyDown = false;
        }

    }


    void UnselectMeunItem()
    {
        ArrowTextOptions[_currentSelection].color = Color.black;
    }

    void SelectMeunItem()
    {
        ArrowTextOptions[_currentSelection].color = Color.white;
    }
}
