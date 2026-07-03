public static class RichTextExtensions
{
  public static string B(this string text) => $"<b>{text}</b>";
  public static string I(this string text) => $"<i>{text}</i>";
  public static string Color(this string text, string color) => $"<color={color}>{text}</color>";
  public static string Size(this string text, int size) => $"<size={size}>{text}</size>";
}
