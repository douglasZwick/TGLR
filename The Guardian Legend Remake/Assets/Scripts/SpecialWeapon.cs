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

  public string DisplayName => m_DisplayName;
  public string Description => m_Description;


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
    // Make sure we can activate the weapon. Reasons we wouldn't be able to include not having
    //   enough ammo, the weapon cooling down, etc.
    // For now, though, we'll just send it through
    Activate(specialWeaponED);
  }


  void OnDeactivationRequest(SpecialWeaponEventData specialWeaponED)
  {
    var activationType = m_ActivationType;

    if (activationType != ActivationType.Continuous)
    {
      Zbug.Warn($"Tried to deactivate {DisplayName}, but its activation type is {activationType}");
      return;
    }


  }


  void Activate(SpecialWeaponEventData specialWeaponED)
  {
    ED.Dispatch(Events.Activated, specialWeaponED);
  }


  public string GetReport()
    => $"{m_DisplayName.B()} — {m_Description} ({m_ActivationType})";


  void OnDisable()
  {
    ED.RemoveListener(Events.ActivationRequest, OnActivationRequest);
    ED.RemoveListener(Events.DeactivationRequest, OnDeactivationRequest);
  }
}
