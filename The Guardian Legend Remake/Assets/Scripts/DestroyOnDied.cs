using UnityEngine;


public class DestroyOnDied : MonoBehaviour
{
  public void OnDied(HealthEventData healthED)
  {
    DestroyThisObject();
  }


  void DestroyThisObject()
  {
    Destroy(gameObject);
  }
}
