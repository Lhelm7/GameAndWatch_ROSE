using UnityEngine;

/// <summary>
/// Classe statique pour sauvegarder les données entre les scènes
/// </summary>
public static class GameData
{
    private static int finalScore = 0;
    private static bool isVictory = false;
    
    public static void SaveGameResult(int score, bool victory)
    {
        finalScore = score;
        isVictory = victory;
        
        Debug.Log($"[GameData] Saved - Score: {finalScore}, Victory: {isVictory}");
    }
    
    public static int GetFinalScore()
    {
        return finalScore;
    }
    
    public static bool IsVictory()
    {
        return isVictory;
    }
    
    public static void Reset()
    {
        finalScore = 0;
        isVictory = false;
        Debug.Log("[GameData] Reset");
    }
}