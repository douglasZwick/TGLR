using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(EventDispatcher))]
[RequireComponent(typeof(PlayerInput))]
public class SpecialWeaponsSystem : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private Transform m_InventoryNode;
  private List<SpecialWeapon> m_Arsenal = new();
  private int m_WeaponIndex = 0;
  [SerializeField]
  private Transform m_FiringPoint;
  private InputAction m_SecondaryFireAction;
  [SerializeField]
  private int m_MaxChips = 50;
  private int m_Chips;
  
  private bool Empty => m_Arsenal.Count <= 0;
  private SpecialWeapon CurrentWeapon => m_Arsenal[m_WeaponIndex];


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    var playerInput = GetComponent<PlayerInput>();
    m_SecondaryFireAction = playerInput.actions.FindAction("SecondaryFire");

    Initialize();
  }


  void OnEnable()
  {
    
  }


  void Update()
  {
    if (m_SecondaryFireAction.WasPerformedThisFrame())
      RequestActivation();
  }


  void Initialize()
  {
    m_Chips = m_MaxChips;

    foreach (Transform child in m_InventoryNode)
    {
      if (!child.TryGetComponent<SpecialWeapon>(out var specialWeapon)) continue;

      AddToArsenal(specialWeapon);
    }

    var report = GetReport();
    Zbug.Log(report);
  }


  void AddNew(SpecialWeapon specialWeapon)
  {
    AddToInventory(specialWeapon.transform);
    AddToArsenal(specialWeapon);
  }


  void AddToInventory(Transform tx)
  {
    tx.SetParent(m_InventoryNode);
  }


  void AddToArsenal(SpecialWeapon specialWeapon)
  {
    m_Arsenal.Add(specialWeapon);
  }


  void SelectNextWeapon()
  {
    if (Empty) return;
    
    m_WeaponIndex = (m_WeaponIndex + 1) % m_Arsenal.Count;
  }


  void SelectPrevWeapon()
  {
    if (Empty) return;
    
    m_WeaponIndex = (m_WeaponIndex - 1) % m_Arsenal.Count;
  }


  void SelectWeaponIndex(int index)
  {
    if (index < 0 || index >= m_Arsenal.Count)
      throw new System.IndexOutOfRangeException($"Tried to select special weapon index {index}");

    m_WeaponIndex = index;
  }


  void RequestActivation()
  {
    // Probably no warning or error needed here -- the user will be likely to press the special
    //   weapon button at times when their arsenal is empty, and I'll probably just send that
    //   straight through to this function when that happens, so I don't think there's any problem
    //   with doing things this way
    if (Empty) return;

    var specialWeaponED = new SpecialWeaponEventData()
    {
      m_FiringPoint = m_FiringPoint,
      m_AvailableChips = m_Chips,
    };

    // TODO:
    //   Perhaps do this with a direct function call. The special weapons system is a trusted
    //   authority over special weapons, so it makes sense for it to have access like that, and
    //   furthermore, "request" type events like this are notoriously limited in their scope in
    //   terms of other components that might want to listen in and do something, so there's
    //   probably very little that I'd be missing out on there
    CurrentWeapon.ED.Dispatch(Events.ActivationRequest, specialWeaponED);

    if (specialWeaponED.m_AvailableChips != m_Chips)
    {
      // feedback for spending chips, probably dispatch an event here
      m_Chips = specialWeaponED.m_AvailableChips;
    }
  }


  string GetReport()
  {
    var msg = new StringBuilder();
    msg.AppendLine($"Current Special Weapon Arsenal ({m_Arsenal.Count} total):");
    foreach (var specialWeapon in m_Arsenal)
      msg.AppendLine("  " + specialWeapon.GetReport());
    
    msg.AppendLine($"Selected: {CurrentWeapon.DisplayName}");

    return msg.ToString();
  }


  void OnDisable()
  {
    
  }
}


/// TODO:
///   Annoying problem to deal with: the dang ol firing point I made for special weapons ain't
///   attached to any dang ol node that the dang ol facer rotates when the hero needs to be
///   faced. The to-do here is that I need to reevaluate my approach to rotating my hero. I'm
///   leaning at this moment toward consolidating the way firing works between my the primary
///   gun and the special weapons system, but I'm not in a position to make a decision on the
///   matter right now.
/// 
///   As a band-aid fix, I'm simply pointing the SpecialWeaponsSystem's m_FiringPoint
///   reference at the primary gun's central firing point for now.
