using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkAnimationController : MonoBehaviour 
{
	#region Variables (private)
	private List<Rigidbody> m_networkedPlayers = new List<Rigidbody>();
	private List<Animator> m_animators = new List<Animator>();

	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{

	}
	
	void Update() 
	{
		for (int i = 0; i < m_networkedPlayers.Count; i++)
		{
			m_animators[i].SetFloat("Speed", m_networkedPlayers[i].velocity.magnitude);
		}
	}
	
	#endregion
	
	#region Methods

	public void ConnectPlayer(GameObject newPlayer)
    {
        m_networkedPlayers.Add(newPlayer.GetComponent<Rigidbody>());
		m_animators.Add(newPlayer.GetComponent<Animator>());
    }
	
	#endregion
}
