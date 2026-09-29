using UnityEngine;


[RequireComponent(typeof(Renderer))]
[ExecuteAlways]
public class MaterialOverride : MonoBehaviour
{
  private static readonly int s_BaseColorId = Shader.PropertyToID("_BaseColor");
  private static readonly int s_EmissiveColorId = Shader.PropertyToID("_EmissiveColor");

  [SerializeField][ColorUsage(showAlpha: true, hdr: true)]
  Color m_BaseColor = Color.white;
  [SerializeField][ColorUsage(showAlpha: true, hdr: true)]
  Color m_EmissiveColor = Color.clear;

  private Renderer m_Renderer;
  private Renderer Renderer
  {
    get
    {
      if (m_Renderer == null) m_Renderer = GetComponent<Renderer>();

      return m_Renderer;
    }
  }


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
    Renderer.material.SetColor(s_BaseColorId, m_BaseColor);
    Renderer.material.SetColor(s_EmissiveColorId, m_EmissiveColor);
  }
}
