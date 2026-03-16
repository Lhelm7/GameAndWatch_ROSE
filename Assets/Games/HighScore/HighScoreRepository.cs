using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

/// <summary>Gère la lecture/écriture des high scores par jeu via fichiers JSON.</summary>
public static class HighScoreRepository
{
    private const int MaxEntries = 10;

    private static string GetFilePath(string gameId)
    {
        return Path.Combine(Application.persistentDataPath, $"highscores_{gameId}.json");
    }

    /// <summary>Ajoute un score et retourne le tableau trié mis à jour.</summary>
    public static List<HighScoreEntry> AddScore(string gameId, string playerName, int score)
    {
        HighScoreTable table = Load(gameId);
        table.entries.Add(new HighScoreEntry { playerName = playerName, score = score });
        table.entries = table.entries
            .OrderByDescending(e => e.score)
            .Take(MaxEntries)
            .ToList();
        Save(gameId, table);
        return table.entries;
    }

    /// <summary>Charge le tableau de scores pour un jeu donné.</summary>
    public static List<HighScoreEntry> GetScores(string gameId)
    {
        return Load(gameId).entries;
    }

    private static HighScoreTable Load(string gameId)
    {
        string path = GetFilePath(gameId);
        if (!File.Exists(path))
            return new HighScoreTable();

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<HighScoreTable>(json) ?? new HighScoreTable();
    }

    private static void Save(string gameId, HighScoreTable table)
    {
        string json = JsonUtility.ToJson(table, true);
        File.WriteAllText(GetFilePath(gameId), json);
    }
}