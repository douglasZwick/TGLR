using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EventDispatcher))]
public class SpecialWeaponsSystem : MonoBehaviour
{
  public EventDispatcher ED { get; private set; }

  [SerializeField]
  private Transform m_InventoryNode;
  private List<SpecialWeapon> m_Arsenal = new();


  protected virtual void Awake()
  {
    ED = GetComponent<EventDispatcher>();

    Initialize();
  }


  void OnEnable()
  {
    
  }


  void Initialize()
  {
    foreach (Transform child in m_InventoryNode)
    {
      if (!child.TryGetComponent<SpecialWeapon>(out var specialWeapon)) continue;

      AddToArsenal(specialWeapon);
    }
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


  void OnDisable()
  {
    
  }
}
