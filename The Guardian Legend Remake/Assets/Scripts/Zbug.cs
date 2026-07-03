using UnityEngine;


public static class Zbug
{
  public static readonly bool s_LoggingEnabled = true;


  public static void Log(object message)
  {
    if (!s_LoggingEnabled) return;

    var prefix = $"{Time.frameCount, 8}".Color("#000") + " | ".B();

    Debug.Log($"{prefix}{message}");
  }
}
