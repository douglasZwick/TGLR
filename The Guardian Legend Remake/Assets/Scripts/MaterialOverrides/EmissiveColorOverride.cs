using UnityEngine;


[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
public class EmissiveColorOverride : MonoBehaviour
{
  private static readonly int s_EmissiveColorId = Shader.PropertyToID("_EmissiveColor");
  private static readonly float s_IntensityExponent = 1.25f;

  [SerializeField]
  private Color m_EmissiveColor = Color.softYellow;
  [SerializeField]
  private float m_EmissionIntensity = 10;

  private Renderer m_Renderer;
  private MaterialPropertyBlock m_PropertyBlock;


  void OnEnable()
  {
    Apply();
  }


  void OnValidate()
  {
    Apply();
  }


  private void Apply()
  {
    m_Renderer ??= GetComponent<Renderer>();
    m_PropertyBlock ??= new MaterialPropertyBlock();

    if (m_Renderer == null) return; // probably not possible thanks to RequireComponent

    var curvedIntensity = Mathf.Pow(s_IntensityExponent, m_EmissionIntensity);
    var finalColor = m_EmissiveColor * curvedIntensity;

    m_Renderer.GetPropertyBlock(m_PropertyBlock);
    m_PropertyBlock.SetColor(s_EmissiveColorId, finalColor);
    m_Renderer.SetPropertyBlock(m_PropertyBlock);
  }
}
