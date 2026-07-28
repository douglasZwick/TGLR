using System.Collections.Generic;
using UnityEngine;


public class ColorDamageFlash : MonoBehaviour
{
  class TargetData
  {
    public Renderer m_Renderer;
    public BaseColorOverride m_BaseColorOverride;
  }

  [System.Serializable]
  class Appearance
  {
    public Material m_Material;
    public Color m_Color;
  }

  static public readonly float s_FlashDuration = 6.0f / 60.0f;

  [SerializeField]
  private Appearance m_ShieldFlash;
  [SerializeField]
  private Appearance m_HpFlash;
  [SerializeField]
  private List<Renderer> m_Targets;

  private List<TargetData> m_TargetData = new();
  private float m_Timer = 0;
  private readonly Dictionary<Renderer, Appearance> m_OriginalValues = new();
  private bool Flashing => m_Timer > 0;


  void Awake()
  {
    foreach (var target in m_Targets)
    {
      var bco = target.GetComponent<BaseColorOverride>();

      m_TargetData.Add(new()
      {
        m_Renderer = target,
        m_BaseColorOverride = bco,
      });

      m_OriginalValues.Add(target, new()
      {
        m_Material = target.material,
        m_Color = bco == null ? Color.white : bco.GetColor(),
      });
    }
  }


  void Update()
  {
    if (Flashing)
      UpdateFlash(Time.deltaTime);
  }


  public void OnReceivedShieldDamage(HealthEventData healthED)
  {
    BeginFlash(m_ShieldFlash);
  }


  public void OnReceivedHpDamage(HealthEventData healthED)
  {
    BeginFlash(m_HpFlash);
  }


  void BeginFlash(Appearance appearance)
  {
    foreach (var target in m_TargetData)
      Apply(target, appearance);

    m_Timer = s_FlashDuration;
  }


  void UpdateFlash(float dt)
  {
    m_Timer -= dt;

    if (Flashing) return;
    
    m_Timer = 0;
    EndFlash();
  }


  void EndFlash()
  {
    foreach (var target in m_TargetData)
      Apply(target, m_OriginalValues[target.m_Renderer]);
  }


  void Apply(TargetData target, Appearance appearance)
  {
    target.m_Renderer.material = appearance.m_Material;
    if (target.m_BaseColorOverride == null) return;
    target.m_BaseColorOverride.SetColor(appearance.m_Color);
  }
}
