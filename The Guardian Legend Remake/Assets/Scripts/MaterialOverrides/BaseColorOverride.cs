using UnityEngine;


// TODO:
//   Refactor with EmissiveColorOverride to use inheritance
[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
public class BaseColorOverride : MonoBehaviour
{
  private static readonly int s_BaseColorId = Shader.PropertyToID("_BaseColor");
  private static readonly int s_ColorId = Shader.PropertyToID("_Color");
  private static readonly int s_UnlitColorId = Shader.PropertyToID("_UnlitColor");

  [SerializeField]
  private Color m_BaseColor = Color.white;
  
  
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
    if (!m_Renderer)
      m_Renderer = GetComponent<Renderer>();
    m_PropertyBlock ??= new MaterialPropertyBlock();

    if (m_Renderer == null) return; // probably not possible thanks to RequireComponent

    m_Renderer.GetPropertyBlock(m_PropertyBlock);
    m_PropertyBlock.SetColor(s_BaseColorId, m_BaseColor);
    m_PropertyBlock.SetColor(s_ColorId, m_BaseColor);
    m_PropertyBlock.SetColor(s_UnlitColorId, m_BaseColor);
    m_Renderer.SetPropertyBlock(m_PropertyBlock);
  }
}
