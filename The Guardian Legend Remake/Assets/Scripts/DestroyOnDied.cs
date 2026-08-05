using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class DestroyOnDied : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private GameObject m_DestructionTarget;


  void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    if (m_DestructionTarget == null)
      m_DestructionTarget = gameObject;
  }


  void OnEnable()
  {
    ED.AddListener(Events.Died, OnDied);
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.Died, OnDied);
  }


  public void OnDied(HealthEventData healthED)
  {
    DestroyThisObject();
  }


  void DestroyThisObject()
  {
    Destroy(m_DestructionTarget);
  }
}
