using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int statusPointsLife;
    public int statusPointsShield;
    public int statusPointsAttack;
    public int pointsToSpend;

    public PlayerData(PlayerController player)
    {
        level = player.level;
        statusPointsLife = player.statusPointsLife;
        statusPointsShield = player.statusPointsShield;
        statusPointsAttack = player.statusPointsAttack;
        pointsToSpend = player.pointsToSpend;
    }

}
