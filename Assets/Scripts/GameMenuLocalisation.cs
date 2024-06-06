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
            title.text = "����� �� ��� �������� ����";
            yourScore.text = "��� ���������";
            bestScore.text = "������ ���������";
            leaderBoard.text = "������� �������";
            restart.text = "������ �������";
            timer.text = "������� ��������� �����";
            googled1.text = "������";
            googled2.text = "������";
            average1.text = "� ������� �� �����";
            average2.text = "� ������� �� �����";
            leaders.text = "������";
            adv.text = "����������";
            vs.text = "���";
            leaders2.text = "������";
            data.text = "������ ����� � ����� ������.��������";
            points.text = "����";
            record.text = "������";
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
