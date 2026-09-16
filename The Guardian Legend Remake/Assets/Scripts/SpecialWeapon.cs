using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class SpecialWeapon : MonoBehaviour
{
  public enum ActivationType
  {
    Discrete,
    Continuous,
  }

  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private string m_DisplayName = "Special Weapon";
  [SerializeField]
  private Sprite m_Icon;
  [SerializeField]
  private string m_Description;
  [SerializeField]
  private ActivationType m_ActivationType = ActivationType.Discrete;


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();
  }


  void OnEnable()
  {
    ED.AddListener(Events.ActivationRequest, OnActivationRequest);
    ED.AddListener(Events.DeactivationRequest, OnDeactivationRequest);
  }


  void OnActivationRequest(SpecialWeaponEventData specialWeaponED)
  {
    
  }


  void OnDeactivationRequest(SpecialWeaponEventData specialWeaponED)
  {
    
  }


  void OnDisable()
  {
    ED.RemoveListener(Events.ActivationRequest, OnActivationRequest);
    ED.RemoveListener(Events.DeactivationRequest, OnDeactivationRequest);
  }
}
