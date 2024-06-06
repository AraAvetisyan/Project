using GeekplaySchool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject  gamePanel, gameOverPanel, leaderBoard;
    [SerializeField] private CardListScript _cardListScript;
    [SerializeField] private СompareScript _compareScript;
    [SerializeField] private GameObject wrongSign;
    [SerializeField] private GameObject adwButton;
    [SerializeField] private AudioSource audioSource;
    private int adwCount;
    private void Start()
    {
        Geekplay.Instance.ShowInterstitialAd();
    }
    private void Update()
    {
        if(adwCount != 0)
        {
            adwButton.SetActive(false);
        }
        else
        {
            adwButton.SetActive(true);
        }
    }

    

    public void PressedRestartButton()
    {
        audioSource.Play();
        adwCount = 0;
        Geekplay.Instance.ShowInterstitialAd();
        _compareScript.card1Button.interactable = true;
        _compareScript.card2Button.interactable = true;
        if (_cardListScript.Cards.Count>=3)
        {
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
            wrongSign.SetActive(false);
            wrongSign.transform.localScale = new Vector2(0f, 0f);
            _compareScript.Init();
            _compareScript.ScoreInGame = 0;
            _compareScript.ScoreInGameText.text = _compareScript.ScoreInGame.ToString();

        }
        else
        {
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
            wrongSign.SetActive(false);
            _cardListScript.Cards.Clear();
            for(int i = 0; i < _cardListScript.NewCards.Count; i++)
            {
                _cardListScript.Cards.Add(_cardListScript.NewCards[i]);
            }

        }
    }

    public void ContinueFromAdw()
    {

        if (adwCount == 0)
        {
            Geekplay.Instance.ShowRewardedAd("Revive");


        }

        adwCount++;
    }
    public void Revive()
    {
        audioSource.Play();
        _compareScript.card1Button.interactable = true;
        _compareScript.card2Button.interactable = true;
        if (_cardListScript.Cards.Count >= 3)
        {
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
            wrongSign.SetActive(false);
            wrongSign.transform.localScale = new Vector2(0f, 0f);
            _compareScript.Init();

        }
        else
        {
            gamePanel.SetActive(true);
            gameOverPanel.SetActive(false);
            wrongSign.SetActive(false);
            _cardListScript.Cards.Clear();
            for (int i = 0; i < _cardListScript.NewCards.Count; i++)
            {
                _cardListScript.Cards.Add(_cardListScript.NewCards[i]);
            }

        }
    }
    public void Leaders()
    {
        audioSource.Play();
        leaderBoard.SetActive(true);
    }
    public void CloseLeaders()
    {
        audioSource.Play();
        leaderBoard.SetActive(false);
    }
}
