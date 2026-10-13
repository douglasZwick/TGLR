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
  private EventChannel m_EventChannel;
  [SerializeField]
  private Transform m_InventoryNode;
  private List<SpecialWeapon> m_Arsenal = new();
  private int m_WeaponIndex = 0;
  [SerializeField]
  private Transform m_FiringPoint;
  private InputAction m_SecondaryFireAction;
  private InputAction m_NextWeaponAction;
  private InputAction m_PreviousWeaponAction;
  [SerializeField]
  private int m_MaxChips = 50;
  private int m_Chips;
  [SerializeField]
  private AudioItem m_ChangeWeaponSound;
  
  private bool Empty => m_Arsenal.Count <= 0;
  private SpecialWeapon CurrentWeapon => m_Arsenal[m_WeaponIndex];


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    var playerInput = GetComponent<PlayerInput>();
    m_SecondaryFireAction = playerInput.actions.FindAction("SecondaryFire");
    m_NextWeaponAction = playerInput.actions.FindAction("NextWeapon");
    m_PreviousWeaponAction = playerInput.actions.FindAction("PreviousWeapon");

    Initialize();
  }


  void OnEnable()
  {
    
  }


  void Start()
  {
    var gaugeED = new GaugeEventData
    {
      m_MaxValue = m_MaxChips,
      m_CurrentValue = m_Chips,
    };
    ED.Dispatch(Events.GaugeSetup, gaugeED);

    var specialWeaponED = new SpecialWeaponEventData
    {
      m_CurrentWeapon = CurrentWeapon,
    };
    m_EventChannel.Dispatch(Events.SpecialWeaponSetup, specialWeaponED);
  }


  void Update()
  {
    if (m_SecondaryFireAction.WasPerformedThisFrame())
      RequestActivation();
    if (m_NextWeaponAction.WasPerformedThisFrame())
      SelectNextWeapon();
    if (m_PreviousWeaponAction.WasPerformedThisFrame())
      SelectPrevWeapon();
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
    
    var newIndex = m_WeaponIndex + 1;
    if (newIndex >= m_Arsenal.Count)
      newIndex = 0;
    
    SelectWeaponIndex(newIndex);
  }


  void SelectPrevWeapon()
  {
    if (Empty) return;
    
    var newIndex = m_WeaponIndex - 1;
    if (newIndex < 0)
      newIndex = m_Arsenal.Count - 1;

    SelectWeaponIndex(newIndex);
  }


  void SelectWeaponIndex(int index)
  {
    if (index < 0 || index >= m_Arsenal.Count)
      throw new System.IndexOutOfRangeException($"Tried to select special weapon index {index}");
    // We don't bother if the incoming index is equal to the current index
    if (index == m_WeaponIndex) return;

    m_WeaponIndex = index;
    PlayChangeWeaponSound();

    var specialWeaponED = new SpecialWeaponEventData
    {
      m_CurrentWeapon = CurrentWeapon,
    };
    m_EventChannel.Dispatch(Events.SwitchedWeapons, specialWeaponED);
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

      ED.Dispatch(Events.SpentChips, specialWeaponED);
    }
  }


  void PlayChangeWeaponSound()
  {
    if (m_ChangeWeaponSound == null) return;

    AudioManager.Instance.Play(m_ChangeWeaponSound);
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

/// TODO:
///   I should probably make this component interface directly with the EventChannel for all the
///   chip-related events, in addition to doing so for the icon updates. The reason I'm not already
///   doing that is that I modeled the HUD updating paradigm after what the Health component uses.
///   Since Health can be present on things that don't update HUD elements (e.g. enemies etc.), I
///   wanted to write it so that it would dispatch to itself and then we would rely on the Hero
///   having another separate component that would receive that event and forward "translate" it,
///   so to speak, into a gauge update event that it would forward along using an EventChannel.
///   That's all well and good for that component, but I ought to expect that nobody but the Hero
///   will ever have special weapons that cost chips, at least the way I'm defining them now.
/// 
///   In the case that I do ever have a non-Hero object that uses special weapons, I can just do a
///   null check on the EventChannel before I dispatch anything. Honestly I refactor Health and
///   Shield to do exactly the same thing.

/// TODO:
///   Add "NO USE" or its equivalent. Make it the "current weapon" by default.
