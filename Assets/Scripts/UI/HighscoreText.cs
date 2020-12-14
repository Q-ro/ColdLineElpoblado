using UnityEngine;
using UnityEngine.UI;

public class HighscoreText : MonoBehaviour
{
    #region Inspector Properties

    [SerializeField] Text highStoreText;

    #endregion

    void Start()
    {
        highStoreText = GetComponent<Text>();

        if (PlayerPreferenceManager.GetHighscore() > 0)
        {
            highStoreText.text = PlayerPreferenceManager.GetHighscore().ToString("D11");
        }
        else
        {
            PlayerPreferenceManager.SetHighscore(9000);
            highStoreText.text = 9000.ToString("D11");
        }

    }

}
