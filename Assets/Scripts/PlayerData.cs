/*
 * Script to store player progress data
 **/

[System.Serializable]
public class PlayerData
{
    // Current level
    public int level;

    // Current life
    public float playerLife;

    // Current EXP
    public int exp;
    public int expToLevelUp;
    
    // Status Points
    public int statusPointsLife;
    public int statusPointsShield;
    public int statusPointsAttack;
    public int statusPointsAttackSpeed;
    public int statusPointsMovSpeed;
    public int pointsToSpend;
    public int statusPointsLuck;

    // Get data from PlayerController
    public PlayerData(PlayerController player)
    {
        level = player.level;
        exp = player.exp;
        playerLife = player.playerLife;
        expToLevelUp = player.expToLevelUp;
        statusPointsLife = player.statusPointsLife;
        statusPointsShield = player.statusPointsShield;
        statusPointsAttack = player.statusPointsAttack;
        statusPointsAttackSpeed =player.statusPointsAttackSpeed;
        statusPointsMovSpeed = player.statusPointsMovSpeed;
        pointsToSpend = player.pointsToSpend;
        statusPointsLuck = player.statusPointsLuck;
    }

}
