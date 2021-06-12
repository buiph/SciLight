using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardScore // Saves the _name and _score for the top five all time scorers.
{
    private string _name;
    private int _score;

    public LeaderboardScore()
    {
        _name = "";
        _score = 0;
    }
    public LeaderboardScore(string changename, int changeScore)
    {
        _name = changename;
        _score = changeScore;
    }
    public string GetName()
    {
        return _name;
    }
    public void SetName(string changeName)
    {
        _name = changeName;
    }
    public int GetScore()
    {
        return _score;
    }
    public void SetScore(int value)
    {
        _score = value;
    }
}
