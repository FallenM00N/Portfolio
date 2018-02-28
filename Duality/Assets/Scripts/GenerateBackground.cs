using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateBackground : MonoBehaviour 
{
	#region Variables (private)
	[SerializeField] GameObject[] m_backgroundSprites;
	[SerializeField] Vector3 m_offset;
	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		Vector3 offset1 = new Vector3(m_offset.x * 2, m_offset.y, m_offset.z);
		Vector3 offset2 = new Vector3(-m_offset.x * 2, m_offset.y, m_offset.z);
		for (int i = 0; i < m_backgroundSprites.Length; i++)
		{
			Instantiate(m_backgroundSprites[i], offset1, Quaternion.identity, transform);
		}
		for (int i = 0; i < m_backgroundSprites.Length; i++)
		{
			Instantiate(m_backgroundSprites[i], offset2, Quaternion.identity, transform);
		}
	}
	
	void Update() 
	{
		
	}
	
	#endregion
	
	#region Methods
	
	#endregion
}
