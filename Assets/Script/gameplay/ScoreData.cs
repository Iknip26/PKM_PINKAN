[System.Serializable]
public class ScoreData
{
    public int status;
    public string message;
    public Data[] data;
}

[System.Serializable]
public class Data
{
    public int id;
    public int score;
}