using UnityEngine;
using System;
using System.Collections;

public class DebugLogg : MonoBehaviour {
    public string condition = "";
    public string stackTrace = "";
    public string systemStackTrace = "";
    public LogType type;

    void Awake()
    {
        Application.RegisterLogCallback(LogCallBackHandler);
    }

    void LogCallBackHandler(string condition, string stackTrace, LogType type)
    {

        System.Diagnostics.StackTrace systemStackTrace = new System.Diagnostics.StackTrace(true);
        string systemStackTraceStr = systemStackTrace.ToString();

        SetLogData(condition, stackTrace, systemStackTraceStr, type);
    }

    void SetLogData(string condition, string stackTrace, string systemStackTrace, LogType type)
    {
        this.condition = condition;
        this.stackTrace = stackTrace;
        this.systemStackTrace = systemStackTrace;
        this.type = type;
    }

    void OnGUI()
    {

        string formatStr = "[condition]\n{0}\n\n[stackTrace]\n{1}\n\n[systemStackTrace]\n{2}\n\n[type]\n{3}";
        string labelStr = string.Format(formatStr, condition, stackTrace, systemStackTrace, type.ToString());
        GUI.Label(new Rect(10, 0, Screen.width - 20, Screen.height - 100), labelStr);

        if (GUI.Button(new Rect(Screen.width / 4 * 0, Screen.height - 100, Screen.width / 4, 100), "Log"))
        {
            Debug.Log("This is Log");
        }

        if (GUI.Button(new Rect(Screen.width / 4 * 1, Screen.height - 100, Screen.width / 4, 100), "Warning"))
        {
            Debug.LogWarning("This is Warning");
        }

        if (GUI.Button(new Rect(Screen.width / 4 * 2, Screen.height - 100, Screen.width / 4, 100), "Error"))
        {
            Debug.LogError("This is Error");
        }

        if (GUI.Button(new Rect(Screen.width / 4 * 3, Screen.height - 100, Screen.width / 4, 100), "Exception"))
        {
            Debug.LogException(new Exception("This is Exception"));
        }

    }
}
