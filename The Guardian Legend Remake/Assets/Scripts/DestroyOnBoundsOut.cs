using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class DestroyOnBoundsOut : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.BoundsOut, OnBoundsOut);
  }


  void OnBoundsOut(BoundsEventData boundsED)
  {
    Destroy(gameObject);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.BoundsOut, OnBoundsOut);
  }
}
