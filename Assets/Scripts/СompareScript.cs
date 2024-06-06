using GeekplaySchool;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class СompareScript : MonoBehaviour
{

    float curentTime = 0;
    [SerializeField] float maxTime;

    [SerializeField] private GameObject card1, card2;
    [SerializeField] private ScaleCard1 _scaleCard1;
    [SerializeField] private ScaleCard2 _scaleCard2;

    [SerializeField] private Image card1Sprite; 
    [SerializeField] private Image card2Sprite; 

    public Button card1Button; 
    public Button card2Button;


    [SerializeField] private float card1Googled;
    [SerializeField] private float card2Googled;

    [SerializeField] private TextMeshProUGUI card1GoogledText;
    [SerializeField] private TextMeshProUGUI card2GoogledText;

    [SerializeField] private TextMeshProUGUI card1NameText;
    [SerializeField] private TextMeshProUGUI card2NameText; 
    
    [SerializeField] private CardListScript _cardListScript;


    [SerializeField] private GameObject gameOverPanel; 
    [SerializeField] private GameObject playPanel; 

    [SerializeField] private GameObject card1GoogledObject;
    [SerializeField] private GameObject card2GoogledObject;
    private bool card2Closed = false;
    private bool choisedTrue = false;

    public TextMeshProUGUI ScoreInGameText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    public int ScoreInGame;

    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI finalBestScore;

    private bool wait = true;
    private bool card1NumberShow = false;
    private bool card2NumberShow = false;

    private bool card1NumberShown = false;
    private bool card2NumberShown = false;

    public bool Card1GetSmoled = false;
    public bool Card2GetSmoled = false;

    [SerializeField] private GameObject wrongSigne;
    [SerializeField] private SignScale _wrongSignScale;
    [SerializeField] private SignScale _rightSignScale;

    [SerializeField] private GameObject rightSign;

    [SerializeField] private UIController _uIController;


    [SerializeField] private GameObject average1Card1, average2Card1, average1Card2, average2Card2;


    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource rightAnswer;
    [SerializeField] private AudioSource wrongAnswer;
    public void Start()
    {


        Init();
        Geekplay.Instance.SubscribeOnReward("Revive", _uIController.Revive);

    }
    
    private void Update()
    {
        

        if (card1NumberShown)
        {
            card1NumberShown = false;
            if (choisedTrue)
            {
                StartCoroutine(TrueSignAnim());
            }
            else
            {
                StartCoroutine(WaitForGameOver());
            }
            choisedTrue = false;
            
        }
        if (card2NumberShown)
        {
            card2NumberShown = false;
            if (choisedTrue)
            {
                StartCoroutine(TrueSignAnim());
            }
            else
            {
                StartCoroutine(WaitForGameOver());
            }
            choisedTrue = false;
        }
        if (Card1GetSmoled)
        {
            Card1GetSmoled = false;
            NewCardCreator();
        }
        if (Card2GetSmoled)
        {
            Card2GetSmoled = false;
            NewCardCreator();

        }

    }

    public void Init()
    {
        ScoreInGameText.text = ScoreInGame.ToString();

        bestScoreText.text = ScoreInGame.ToString();
        bestScoreText.text = Geekplay.Instance.PlayerData.BestScore.ToString();
        finalBestScore.text = Geekplay.Instance.PlayerData.BestScore.ToString();

        card2GoogledObject.SetActive(false);
        average1Card2.SetActive(false);
        average2Card2.SetActive(false);
        card2Closed = true;
        var culture = new CultureInfo("ru-RU");

        int firstCard = UnityEngine.Random.Range(0, _cardListScript.Cards.Count);
        card1Googled = _cardListScript.Cards[firstCard].Googled;
        card1Sprite.sprite = _cardListScript.Cards[firstCard].image;
        if (Geekplay.Instance.language == "ru")
        {
            card1NameText.text = _cardListScript.Cards[firstCard].RuName;
        }
        else if (Geekplay.Instance.language == "en")
        {
            card1NameText.text = _cardListScript.Cards[firstCard].Name;
        }
        else if (Geekplay.Instance.language == "tr")
        {
            card1NameText.text = _cardListScript.Cards[firstCard].TurName;
        }

      //  card1GoogledText.text = _cardListScript.Cards[firstCard].Googled.ToString();
       
        card1GoogledText.text = _cardListScript.Cards[firstCard].Googled.ToString("#,#", culture);
        _cardListScript.Cards.RemoveAt(firstCard);



        int secondCard = UnityEngine.Random.Range(0, _cardListScript.Cards.Count);
       
        card2Googled = _cardListScript.Cards[secondCard].Googled;
        
        card2Sprite.sprite = _cardListScript.Cards[secondCard].image;
        
        if (Geekplay.Instance.language == "ru")
        {
            card2NameText.text = _cardListScript.Cards[secondCard].RuName;
        }
        else if (Geekplay.Instance.language == "en")
        {
            card2NameText.text = _cardListScript.Cards[secondCard].Name;
        }
        else if (Geekplay.Instance.language == "tr")
        {
            card2NameText.text = _cardListScript.Cards[secondCard].TurName;
        }
        // card2GoogledText.text = _cardListScript.Cards[secondCard].Googled.ToString();

      
        card2GoogledText.text = _cardListScript.Cards[secondCard].Googled.ToString("#,#", culture);

        _cardListScript.Cards.RemoveAt(secondCard);
    }

    public void PressedCard1Button()
    {
        audioSource.Play();
        if (card1Googled > card2Googled)
        {
            ScoreInGame += 1;

            if (ScoreInGame > Geekplay.Instance.PlayerData.BestScore)
            {
                Geekplay.Instance.PlayerData.BestScore = ScoreInGame;
                Geekplay.Instance.Save();
                Geekplay.Instance.SetLeaderboard("Point", Geekplay.Instance.PlayerData.BestScore);
                
            }
            choisedTrue = true;
            ShowNumbers();
        }
        else
        {

            choisedTrue = false;
            ShowNumbers();

        }
        if (_cardListScript.Cards.Count<=3)
        {
            _cardListScript.Cards.Clear();
            for(int i = 0; i < _cardListScript.NewCards.Count; i++)
            {
                _cardListScript.Cards.Add(_cardListScript.NewCards[i]);
            }

        }
    }
    public void PressedCard2Button()
    {
        audioSource.Play();
        if (card2Googled > card1Googled)
        {
            ScoreInGame += 1;
           
            if (ScoreInGame > Geekplay.Instance.PlayerData.BestScore)
            {
                Geekplay.Instance.PlayerData.BestScore = ScoreInGame;
                Geekplay.Instance.Save();
                Geekplay.Instance.SetLeaderboard("Point", Geekplay.Instance.PlayerData.BestScore);

            }
            choisedTrue = true;
            ShowNumbers();
        }
        else
        {
            choisedTrue = false;
            ShowNumbers();
        }
        if (_cardListScript.Cards.Count<=3)
        {
            _cardListScript.Cards.Clear();
            for(int i = 0; i < _cardListScript.NewCards.Count; i++)
            {
                _cardListScript.Cards.Add(_cardListScript.NewCards[i]);
            }

        }
    }

    public float CalculateMoney(float startNumber, float endNumber, float currentTime, float maxTime)
    {
        if (currentTime == 1)
        {
            return startNumber;
        }
        else if (currentTime >= maxTime)
        {
            return endNumber;
        }
        else
        {
            float t = currentTime / maxTime;
            float interpolatedMoney = startNumber + (endNumber - startNumber) * t;
            return interpolatedMoney;
        }
        
    }
    public void ShowNumbers()
    {
        if (card2Closed)
        {
            card2NumberShow = true;
            StartCoroutine(NumberShowing());    
        }
        else
        {
            card1NumberShow = true;
            StartCoroutine(NumberShowing());
        }
    }
    public void ToSmoll()
    {
        if (!card2Closed)
        {
            _scaleCard1.GetSmoller = true;
        }
        else
        {
            _scaleCard2.GetSmoller = true;
        }       
    }
    public void NewCardCreator()
    {
        int newCard = UnityEngine.Random.Range(0, _cardListScript.Cards.Count);
        if (!card2Closed)
        {
            card1GoogledObject.SetActive(false);
            average1Card1.SetActive(false);
            average2Card1.SetActive(false);
            card1Googled = _cardListScript.Cards[newCard].Googled;
            card1Sprite.sprite = _cardListScript.Cards[newCard].image;
            if (Geekplay.Instance.language == "ru")
            {
                card1NameText.text = _cardListScript.Cards[newCard].RuName;
            }
            else if (Geekplay.Instance.language == "en")
            {
                card1NameText.text = _cardListScript.Cards[newCard].Name;
            }
            else if (Geekplay.Instance.language == "tr")
            {
                card1NameText.text = _cardListScript.Cards[newCard].TurName;
            }
           // card1GoogledText.text = _cardListScript.Cards[newCard].Googled.ToString();

            var culture = new CultureInfo("ru-RU");
            card1GoogledText.text = _cardListScript.Cards[newCard].Googled.ToString("#,#", culture);

            _cardListScript.Cards.RemoveAt(newCard);
        }
        else
        {
            card2GoogledObject.SetActive(false);
            average1Card2.SetActive(false);
            average2Card2.SetActive(false);
            card2Googled = _cardListScript.Cards[newCard].Googled;
            card2Sprite.sprite = _cardListScript.Cards[newCard].image;
            if (Geekplay.Instance.language == "ru")
            {
                card2NameText.text = _cardListScript.Cards[newCard].RuName;
            }
            else if (Geekplay.Instance.language == "en")
            {
                card2NameText.text = _cardListScript.Cards[newCard].Name;
            }
            else if (Geekplay.Instance.language == "tr")
            {
                card2NameText.text = _cardListScript.Cards[newCard].TurName;
            }
            //card2GoogledText.text = _cardListScript.Cards[newCard].Googled.ToString();

            var culture = new CultureInfo("ru-RU");
            card2GoogledText.text = _cardListScript.Cards[newCard].Googled.ToString("#,#", culture);

            _cardListScript.Cards.RemoveAt(newCard);
        }
    }

    public IEnumerator NumberShowing()
    {
        curentTime = 0;
        
        if (card1NumberShow)
        {
            card1GoogledObject.SetActive(true);
            average1Card1.SetActive(true);
            average2Card1.SetActive(true);
            while (curentTime < maxTime)
            {
                float currentValue = CalculateMoney(card1Googled/2, card1Googled, curentTime, maxTime);
                card1Button.interactable = false;
                card2Button.interactable = false;

                
                card1GoogledText.text = string.Format("{0:f0}", currentValue);
                var culture = new CultureInfo("ru-RU");
                card1GoogledText.text = currentValue.ToString("#,#", culture);
                yield return new WaitForSecondsRealtime(0.05f);
                curentTime += 0.05f;
               
            }
            if (curentTime >= maxTime)
            {
                ScoreInGameText.text = ScoreInGame.ToString();
                finalScoreText.text = ScoreInGame.ToString();
                bestScoreText.text = Geekplay.Instance.PlayerData.BestScore.ToString();
                finalBestScore.text = Geekplay.Instance.PlayerData.BestScore.ToString();
                if (card2Closed)
                {
                    card2Closed = false;
                }
                else
                {
                    card2Closed = true;
                }
                
                card1NumberShow = false;
                card1NumberShown = true;
                
            }

        }
        if (card2NumberShow)
        {
            card2GoogledObject.SetActive(true);
            average1Card2.SetActive(true);
            average2Card2.SetActive(true);
            while (curentTime < maxTime)
            {
                float currentValue = CalculateMoney(card2Googled/2, card2Googled, curentTime, maxTime);
                card1Button.interactable = false;
                card2Button.interactable = false;

                

                card2GoogledText.text = string.Format("{0:f0}", currentValue);
                var culture = new CultureInfo("ru-RU");
                card2GoogledText.text = currentValue.ToString("#,#", culture);

                yield return new WaitForSecondsRealtime(0.05f);
                curentTime += 0.05f;
                
            }
            if (curentTime >= maxTime)
            {
                ScoreInGameText.text = ScoreInGame.ToString();
                finalScoreText.text = ScoreInGame.ToString();
                bestScoreText.text = Geekplay.Instance.PlayerData.BestScore.ToString();
                finalBestScore.text = Geekplay.Instance.PlayerData.BestScore.ToString();
                if (card2Closed)
                {
                    card2Closed = false;
                }
                else
                {
                    card2Closed = true;
                }

                card2NumberShow = false;
                card2NumberShown = true;
                
            }
        }

    }
    public IEnumerator TrueSignAnim()
    {
        rightSign.SetActive(true);
        _rightSignScale.Scale();
        rightAnswer.Play();
        yield return new WaitForSecondsRealtime(1.5f);
        
        StartCoroutine(GetingSmoll());
    }
    public IEnumerator GetingSmoll()
    {
        yield return new WaitForSecondsRealtime(1f);
        rightSign.SetActive(false);
        rightSign.transform.localScale = new Vector2(0f, 0f);
        ToSmoll();
    }
    public IEnumerator WaitToClose()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        
    }

    public IEnumerator WaitForGameOver()
    {
        wrongSigne.SetActive(true);
        _wrongSignScale.Scale();
        wrongAnswer.Play();
        yield return new WaitForSecondsRealtime(1.5f);
        gameOverPanel.SetActive(true);
        playPanel.SetActive(false);
    }

   

    
}

