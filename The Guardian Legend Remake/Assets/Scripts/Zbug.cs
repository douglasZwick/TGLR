using UnityEngine;


public static class Zbug
{
  public static readonly bool s_LoggingEnabled = true;
  public static string Prefix => $"{Time.frameCount, 8}".Color("#000") + " | ".B();


  public static void Log(object message)
  {
    if (!s_LoggingEnabled) return;
    Debug.Log($"{Prefix}{message}");
  }


  public static void Warn(object message)
  {
    if (!s_LoggingEnabled) return;
    Debug.LogWarning($"{Prefix}{message}");
  }


  public static void Error(object message)
  {
    if (!s_LoggingEnabled) return;
    Debug.LogError($"{Prefix}{message}");
  }
}
