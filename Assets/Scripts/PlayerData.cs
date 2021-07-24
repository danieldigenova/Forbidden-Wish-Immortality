/*
 * Script to store player progress data
 **/

[System.Serializable]
public class PlayerData
{
    // Current level
    public int level;
    
    // Status Points
    public int statusPointsLife;
    public int statusPointsShield;
    public int statusPointsAttack;
    public int pointsToSpend;

    // Get data from PlayerController
    public PlayerData(PlayerController player)
    {
        level = player.level;
        statusPointsLife = player.statusPointsLife;
        statusPointsShield = player.statusPointsShield;
        statusPointsAttack = player.statusPointsAttack;
        pointsToSpend = player.pointsToSpend;
    }

}
