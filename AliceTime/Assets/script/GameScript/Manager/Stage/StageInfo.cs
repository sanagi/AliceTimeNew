using UnityEngine;
using System.Collections;

public class StageInfo : ScriptableObject
{
    public enum PLAYER_TYPE
    {
        Kureha
    }

    public enum Stage_TYPE
    {
        Normal, Fire, Aqua, Ghost, Wind, Last
    }

    public PLAYER_TYPE PlayerType;
    public Stage_TYPE StageType;
    public int AreaLength;
    public Vector2 PlayerPosition;
}