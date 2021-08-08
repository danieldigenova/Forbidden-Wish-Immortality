/*
 * Script to store game progress data
 **/

[System.Serializable]
public class GameData
{
    // Current level
    public int level;

    // Maximum level reached
    public int max_level;

    // Get data from GameController
    public GameData(GameController game)
    {
        level = game.level;
        max_level = game.max_level;
    }
}
