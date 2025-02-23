using System;

[System.Serializable]

public class HighscoreData
{
    public int arrayLimit = 5;
    public string[] scores;
    public string[] pseudo;
    public string[] time;

    public HighscoreData()
    {
        scores = new string[arrayLimit];
        pseudo = new string[arrayLimit];
        time = new string[arrayLimit];
        SetVariables(pseudo, "pseudo");
        SetVariables(scores, "scores");
        SetVariables(time, "time");
    }

    private void SetVariables(string[] variable, string type)
    {
        for (int i = 0; i < variable.Length; i++)
        {
            if (type == "pseudo")
            {
                variable[i] = "NEW NAME";
            }
            else if (type == "scores")
            {
                variable[i] = "0000";
            }
            else
            {
                variable[i] = "00 : 00";
            }
            
        }
    }

    public HighscoreData DeepCopy()
    {
        HighscoreData copy = new HighscoreData();
        Array.Copy(scores, copy.scores, scores.Length);
        Array.Copy(pseudo, copy.pseudo, pseudo.Length);
        Array.Copy(time, copy.time, time.Length);

        return copy;
    }
}
