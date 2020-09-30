using System.Collections.Generic;
using UnityEngine;

public class HeroGun : MonoBehaviour
{
  public GameObject m_FiringPattern;
  public float m_Cooldown = 0.25f;
  public int m_MaxWaves = 4;
  public HeroForm m_HeroForm = HeroForm.Humanoid;

  Transform m_Tx;
  float m_CooldownTimer = 0;
  bool IsCoolingDown { get { return m_CooldownTimer < m_Cooldown; } }
  bool WavesAreAvailable { get { return m_BulletWaves.Count < m_MaxWaves; } }
  List<GameObject> m_BulletWaves = new List<GameObject>();


  void Start()
  {
    m_Tx = transform;
  }


  void Update()
  {
    UpdateCooldown();
    RemoveDeadWaves();
  }


  public void AttemptFire()
  {
    if (!IsCoolingDown && WavesAreAvailable)
      Fire();
  }


  void Fire()
  {
    ResetCooldown();

    var waveWorldPosition = m_Tx.position;
    var wave = Instantiate(m_FiringPattern, waveWorldPosition, Quaternion.identity);
    m_BulletWaves.Add(wave);
  }


  void UpdateCooldown()
  {
    m_CooldownTimer += Time.deltaTime;
  }


  void ResetCooldown()
  {
    m_CooldownTimer = 0;
  }


  void RemoveDeadWaves()
  {
    var newList = new List<GameObject>();

    foreach (var wave in m_BulletWaves)
      if (wave != null)
        newList.Add(wave);

    m_BulletWaves = newList;
  }
}
