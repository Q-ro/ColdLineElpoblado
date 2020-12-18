using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager UIManagerInstance;

    #region Inspector Variables

    [SerializeField] Text weaponName;
    [SerializeField] Image weaponIcon;
    [SerializeField] Text ammoCount;
    [SerializeField] Text highScoreCounter;
    [SerializeField] Text scoreCounter;

    [SerializeField] InputMenuSelection gameOverMenu;
    #endregion

    int _highScore = 0;
    int _currentScore = 0;

    void Awake()
    {
        UIManagerInstance = this;
    }

    void Start()
    {
        PlayerPreferenceManager.SetScore(_currentScore);

        if (PlayerPreferenceManager.GetHighscore() > 0)
        {
            _highScore = PlayerPreferenceManager.GetHighscore();
            
        }
        else
        {
            PlayerPreferenceManager.SetHighscore(9000);
            _highScore = 9000;
        }
        
        this.UpdateHighScore(_highScore);
    }


    public void SetSelectedWeapon(WeaponPowerupController.GameWeapons weapon, int maxAmmo, bool hasAmo)
    {
        weaponName.text = weapon.ToString();
        weaponIcon.sprite = GameManager.Instance.GetspriteForWeapon(weapon);
        if (hasAmo)
            ammoCount.text = maxAmmo.ToString("D2") + "/" + maxAmmo.ToString("D2");
        else
            ammoCount.text = "∞";
    }

    public void UpdateAmmoCount(int maxAmmo, int currentAmmo, bool hasAmo)
    {
        if (hasAmo)
            ammoCount.text = currentAmmo.ToString("D2") + "/" + maxAmmo.ToString("D2");
        else
            ammoCount.text = "∞";
    }

    public void UpdateScore(int score)
    {

        _currentScore += score;
        PlayerPreferenceManager.SetScore(_currentScore);
        scoreCounter.text = _currentScore.ToString("D11");
        if (_currentScore > PlayerPreferenceManager.GetHighscore())
            UpdateHighScore(score);
    }

    public void UpdateHighScore(int score)
    {
        _highScore = score;
        PlayerPreferenceManager.SetHighscore(_highScore);
        highScoreCounter.text = _highScore.ToString("D11");
    }

    public void ShowGameOverMenu()
    {
        // gameOverMenu.ShowGameoverMenu();
    }
}
