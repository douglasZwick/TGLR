using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class SpecialWeapon : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }


  protected virtual void Awake()
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
