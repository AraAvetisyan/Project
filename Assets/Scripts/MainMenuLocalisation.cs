using GeekplaySchool;
using TMPro;
using UnityEngine;

public class MainMenuLocalisation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title1, main1, main2, playButtonText, data;

    private void Start()
    {
        if(Geekplay.Instance.language == "ru")
        {
            title1.text = "ЧТО ГУГЛЯТ БОЛЬШЕ?";
            main1.text = "Вы уверены, что сможете разгадать все тайны интернета?";
            main2.text = "Давайте попробуем";
            playButtonText.text = "ИГРАТЬ";
            data.text = "Данные взяты с сайта Яндекс.Вордстат";
        }
        else if(Geekplay.Instance.language == "en")
        {
            title1.text = "WHAT'S GOOGLED MORE";
            main1.text = "Are You sure you can unravel all the mysteries of the internet?";
            main2.text = "Let's try";
            playButtonText.text = "PLAY";
            data.text = "Data taken from Yandex.Wordstat website";
        }
        else if(Geekplay.Instance.language == "tr")
        {
            title1.text = "Google'da NELER ARADIK DAHA FAZLA";
            main1.text = "Internetin tum gizemlerini cozebileceginizden emin misiniz?";
            main2.text = "Hadi deneyelim.";
            playButtonText.text = "OYNAT";
            data.text = "Yandex.Wordstat web sitesinden alinan veriler";
        }
    }
}
