using GeekplaySchool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMenuLocalisation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title, yourScore, bestScore, leaderBoard, restart, timer, 
        googled1, googled2, average1, average2, leaders, adv, vs, leaders2, data, points, record;
    void Start()
    {
        if (Geekplay.Instance.language == "ru")
        {
            title.text = "Какой из них выбирают чаще";
            yourScore.text = "Ваш результат";
            bestScore.text = "Лучший результат";
            leaderBoard.text = "Таблица Лидеров";
            restart.text = "Начать сначала";
            timer.text = "Таблица обновится через";
            googled1.text = "Гуглят";
            googled2.text = "Гуглят";
            average1.text = "В среднем за месяц";
            average2.text = "В среднем за месяц";
            leaders.text = "Лидеры";
            adv.text = "Продолжить";
            vs.text = "Или";
            leaders2.text = "Лидеры";
            data.text = "Данные взяты с сайта Яндекс.Вордстат";
            points.text = "Очки";
            record.text = "Рекорд";
        }
        else if (Geekplay.Instance.language == "en")
        {
            title.text = "Which one choose more often";
            yourScore.text = "Your Score";
            bestScore.text = "Best Score";
            leaderBoard.text = "Leader Board";
            restart.text = "Restart";
            timer.text = "The table will be updated in";
            googled1.text = "Googled ";
            googled2.text = "Googled ";
            average1.text = "In an average month";
            average2.text = "In an average month";
            leaders.text = "Leaders";
            adv.text = "Continue";
            vs.text = "VS";
            leaders2.text = "Leaders";
            data.text = "Data taken from Yandex.Wordstat website";
            points.text = "Points";
            record.text = "Record";
        }
        else if (Geekplay.Instance.language == "tr")
        {
            title.text = "Kullanicilar daha sik tercih ediyor?";
            yourScore.text = "Puaniniz";
            bestScore.text = "En Iyi Skor";
            leaderBoard.text = "Lider Panosu";
            restart.text = "Yeniden baslat";
            timer.text = "Tablo su sekilde guncellenecektir";
            googled1.text = "Google'da";
            googled2.text = "Google'da";
            average1.text = "Ortalama bir ayda";
            average2.text = "Ortalama bir ayda";
            leaders.text = "Liderler";
            adv.text = "Devam et";
            vs.text = "VS";
            leaders2.text = "Liderler";
            data.text = "Yandex.Wordstat web sitesinden alinan veriler";
            points.text = "Noktalar";
            record.text = "Kayit";
        }
    }

   
}
