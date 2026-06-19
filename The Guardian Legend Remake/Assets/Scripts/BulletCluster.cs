using System;
using System.Collections.Generic;
using UnityEngine;


public class BulletCluster
{
  private Action<BulletCluster> m_DestroyClusterCallback;
  private List<ClusterMember> m_Members = new();


  public BulletCluster(Action<BulletCluster> destroyClusterCallback)
  {
    m_DestroyClusterCallback = destroyClusterCallback;
  }


  public void Add(ClusterMember member)
  {
    member.Setup(this);
    m_Members.Add(member);
  }


  public void MemberDestroyed(ClusterMember member)
  {
    m_Members.Remove(member);

    if (m_Members.Count <= 0)
      LastMemberDestroyed();
  }


  private void LastMemberDestroyed()
  {
    m_DestroyClusterCallback(this);
  }
}
