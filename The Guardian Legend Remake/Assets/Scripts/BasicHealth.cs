using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class BasicHealth : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    
  }


  void OnDisable()
  {
    
  }
}
