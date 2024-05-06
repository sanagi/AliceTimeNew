using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using UnityEngine;

public static class GameDefine
{
    #region physics

    public const float Z_POS_0 = 0f;

    #endregion
    
    #region ObjectName
    
    public static string UI_CANVAS = "GameUICanvas";
    public static string SUB_CAMERA = "SubCamera";
    public static string ALICE_INPUT_MANAGER = "Alice Input Manager";
    public static string RIWIRED_INPUT_MANAGER = "Rewired Input Manager";
    public static string CAMERA_RIG = "CameraRig";
    public static string VIRTUAL_CAMERA_FOLLOW = "VirtualCameraFollow";
    public static string PLAYER = "Player_D";
    public static string SCENE_MANAGER = "SceneManager";
    public static string SAVE_MANAGER = "SaveManager";
    public static string DEBUG_MANAGER = "DebugManager";
    public static string AUDIO_MANAGER = "AudioManager";
    public static string EFFECT_MANAGER = "EffectManager";
    public static string PREFAB_MANAGER = "PrefabPoolManager";
    public static string CAMERA_MANAGER = "CameraManager";
    public static string MAIN_MANAGER = "PrefabPoolManager";    
    public static string TITLE_UI_MANAGER = "TitleUIManager";    
    public static string LOAD_MANAGER = "LoadManager";
    public static string FADE_MANAGER = "LoadManager";
    
    #endregion

    #region tag

    public static string HISTORY_TAG_PAST = "HistoryColliderForPast";
    public static string HISTORY_TAG_NOW = "HistoryColliderForNow";
    
    #endregion

    #region triggers

    public static string TRIGGER_LAYER = "Triggers";

    #endregion

    #region Input

    #region UI

    public const string ACTION_MOVE_HORIZONTAL_UI = "UIHorizontal";
    public const string ACTION_MOVE_VERTICAL_UI = "UIVertical";
    public const string CANCEL = "UICancel";
    public const string UIRIGHT = "UIRight";
    public const string UILEFT = "UILeft";

    #endregion
    
    #region Game
    public const string ACTION_MOVE_HORIZONTAL = "ACTION_MOVE_HORIZONTAL";
    public const string ACTION_MOVE_VERTICAL = "ACTION_MOVE_VERTICAL";
    public const string ACTION_JUMP = "JUMP";
    public const string ACTION_POINT = "POINT";
    public const string ACTION_POINT_EXPLORE = "POINT_EXPLORE";
    public const string ACTION_POINT_GEAR = "POINT_GEAR";
    public const string ACTION_MOVE_KEYPOINT_HORIZONTAL = "ACTION_MOVE_HORIZONTAL_KEYPOINT";
    public const string ACTION_MOVE_KEYPOINT_VERTICAL = "ACTION_MOVE_VERTICAL_KEYPOINT0";
    public const string RIGHT_ROTATE= "ROTATE_GEAR_Right";
    public const string LEFT_ROTATE = "ROTATE_GEAR_Left";
    public const string AUTO_ROTATE = "ROTATE_GEAR_Auto";

    #endregion

    /// <summary>
    /// 連打対策
    /// </summary>
    public const float STOP_TOP_HITS_SECONDS = 0.1f;
    
    /// <summary>
    /// 長押しした判定
    /// </summary>
    public const float LONG_PRESS_SECONDS = 0.3f;

    /// <summary>
    /// 点滅速度
    /// </summary>
    public const float DISPLAY_BLINK = 0.5f;

    /// <summary>
    /// 巻き戻す時間
    /// </summary>
    public const float REWIND_TIME = 3.0f;

    public readonly static Vector3 XY_ROT = new Vector3(0, 270f, 0);
    public readonly static Vector3 YZ_ROT = new Vector3(0, 360f, 0);
    
    #endregion

    #region LayerName

    public const string DEFAULT_LAYER = "Default"; //通常の足場系
    public const string PLAYER_LAYER = "Player"; //通常の足場系

    #endregion

    #region TitlePhase

    

    #endregion
    
    #region GamePhase

    public const string BOOT = "Boot";

    #region Title
    
    public const string TITLE = "Title";
    public const string TITLE_INIT = "Title_Init";
    public const string TITLE_LOGO = "Title_Logo";
    public const string TITLE_START = "Title_Start";
    public const string TITLE_STORY = "Title_Story";
    public const string TITLE_DIALOG = "Title_Dialog";
    public const string TITLE_OPTION = "Title_Option";
    public const string TITLE_FINAL = "Title_Final";

    #endregion

    #region Game

    public const string GAME = "Game";
    public const string GAME_INIT = "Game_Init";
    public const string GAME_START = "Game_Start";
    public const string GAME_MAIN = "Game_Main";
    public const string GAME_PAUSE = "Game_Pause";
    public const string GAME_DEATH = "Game_Death";
    public const string GAME_EVENT = "Game_Event";
    public const string GAME_GIMICK = "Game_Gimick";
    public const string GAME_CAMERA = "Game_CameraMove";   

    #endregion
    
    #region MapSelect

    public const string AreaSelect = "AreaSelect";
    public const string AREASELECT_INIT = "AreaSelect_Init";
    public const string AREASELECT_START = "AreaSelect_Start";
    public const string AREASELECT_MAIN = "AreaSelect_Main";
    public const string AREASELECT_PAUSE = "AreaSelect_Pause";
    public const string AREASELECT_EVENT = "AreaSelect_Event";
    public const string AREASELECT_NEXT = "AreaSelect_NextStage";
    public const string AREASELECT_CAMERA = "AreaSelect_CameraMove";

    public const string AREA_INIT_ID = "01";

    #endregion
    
    #region Explore

    public const string Explore = "Explore";
    public const string EXPLORE_INIT = "Explore_Init";
    public const string EXPLORE_START = "Explore_Start";
    public const string EXPLORE_MAIN = "Explore_Main";
    public const string EXPLORE_PAUSE = "Explore_Pause";
    public const string EXPLORE_EVENT = "Explore_Event";
    public const string EXPLORE_GIMICK = "Explore_Gimick";
    public const string EXPLORE_CAMERA = "Explore_CameraMove";
    public const string EXPLORE_NEXT = "Explore_NextStage";    

    #endregion    

    #endregion

    #region DebugConst

    public const int DEBUG_STAGE = -1;

    #endregion

    #region RewiredMap

    public const string GEAR_GAME_REWIRED = "GearGame";
    public const string EXPLORE_MAPS_REWIRED = "ExploreGame";
    public const string SYSTEM_REWIRED = "System";

    #endregion

    #region tag

    public const string PlayerTag = "Player";
    
    #endregion

    #region UIParts

    #region Title

    public const string FRONT_CANVAS = "FrontCanvas";
    public const string LOGO_PANEL = "Logo_Panel";
    public const string START_PANEL = "Start_Panel";
    public const string TEAM_LOGO_OBJ = "Team_Logo";

    #endregion

    #endregion

    #region Shader

    public const string _ROTATION = "_Rotation";


    #endregion
}
