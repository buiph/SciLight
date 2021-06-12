using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderScoreContainer
{
    public LeaderboardScore[] leaders; // Array of top five scorers

    public LeaderScoreContainer()
    {
        leaders = new LeaderboardScore[5];
        for (int i = 0; i < leaders.Length; i++)
        {
            leaders[i] = new LeaderboardScore();
        }
    }
}
