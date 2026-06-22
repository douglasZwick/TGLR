using UnityEngine;


[RequireComponent(typeof(BoundsChecker2D))]
public class DestroyOnBoundsOut : MonoBehaviour
{
  void Awake()
  {
    var boundsChecker2D = GetComponent<BoundsChecker2D>();
    boundsChecker2D.m_Events.BoundsOut.AddListener(OnBoundsOut);
  }


  void OnBoundsOut(BoundsEventData boundsED)
  {
    Destroy(gameObject);
  }
}
