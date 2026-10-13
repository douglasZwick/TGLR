using UnityEngine;
using UnityEngine.UI;


public class SpecialWeaponDisplay : MonoBehaviour
{
  [SerializeField]
  private EventChannel m_EventChannel;
  [SerializeField]
  private Image m_IconImage;


  void OnEnable()
  {
    m_EventChannel.AddListener(Events.SpecialWeaponSetup, OnSpecialWeaponSetup);
    m_EventChannel.AddListener(Events.SwitchedWeapons, OnSwitchedWeapons);
  }


  void OnSpecialWeaponSetup(SpecialWeaponEventData eventData)
  {
    SetCurrent(eventData.m_CurrentWeapon);
  }


  void OnSwitchedWeapons(SpecialWeaponEventData eventData)
  {
    SetCurrent(eventData.m_CurrentWeapon);
  }


  void SetCurrent(SpecialWeapon specialWeapon)
  {
    m_IconImage.color = Color.white;
    m_IconImage.sprite = specialWeapon.Icon;
  }


  void OnDisable()
  {
    m_EventChannel.RemoveListener(Events.SpecialWeaponSetup, OnSpecialWeaponSetup);
    m_EventChannel.RemoveListener(Events.SwitchedWeapons, OnSwitchedWeapons);
  }
}
