/*
 * Script to store game progress data
 **/

[System.Serializable]
public class GameData
{
    // Current level
    public int level;

    // Get data from GameController
    public GameData(GameController game)
    {
        level = game.level;
    }
}
