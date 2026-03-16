using System;
using System.Collections.Generic;

[Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class HighScoreTable
{
    public List<HighScoreEntry> entries = new List<HighScoreEntry>();
}