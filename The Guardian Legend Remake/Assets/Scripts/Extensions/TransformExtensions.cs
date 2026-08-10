using UnityEngine;


public static class TransformExtensions
{
  public static void DestroyAllChildren(this Transform transform)
  {
    for (var i = transform.childCount - 1; i >= 0; --i)
      Object.Destroy(transform.GetChild(i).gameObject);
  }


  public static void DestroyAllChildrenImmediate(this Transform transform)
  {
    while (transform.childCount > 0)
      Object.DestroyImmediate(transform.GetChild(0).gameObject);
  }
}
