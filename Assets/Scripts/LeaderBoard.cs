using GeekplaySchool;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] namesInGame;
    [SerializeField] private TextMeshProUGUI[] scoresInGame;

    [SerializeField] private TextMeshProUGUI remainingTimeView;
    void Start()
    {
        Geekplay.Instance.leaderboard = this;

        Utils.GetLeaderboard("score", 0, "Point");
        Utils.GetLeaderboard("name", 0, "Point");
    }


    private void Update()
    {
        remainingTimeView.text = string.Format("{0:f0}", Geekplay.Instance.remainingTimeUntilUpdateLeaderboard);
        if (Geekplay.Instance.remainingTimeUntilUpdateLeaderboard <= 0)
        {
            Geekplay.Instance.remainingTimeUntilUpdateLeaderboard = Geekplay.Instance.timeToUpdateLeaderboard;
            Utils.GetLeaderboard("score", 0, "Point");
            Utils.GetLeaderboard("name", 0, "Point");
        }
    }

    public void SetLeadersView(string[] names, string[] scores, int count)
    {
        for(int i = 0; i < names.Length; i++)
        {
            namesInGame[i].text = names[i];
            scoresInGame[i].text = scores[i];
        }
    }

}
