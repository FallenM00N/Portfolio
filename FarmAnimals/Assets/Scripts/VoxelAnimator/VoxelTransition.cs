using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelTransition : MonoBehaviour 
{
	#region Variables (private)
	[SerializeField] private int m_targetAnimation;
	[SerializeField] private VoxelCondition[] m_conditions = null;

	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		
	}
	
	void Update() 
	{
		
	}
	
	#endregion
	
	#region Methods
	public int CheckConditions(int curAnimation)
	{
		bool shouldTransition = false;
		foreach (VoxelCondition condition in m_conditions)
		{
			shouldTransition = condition.CheckCondition();
			if(shouldTransition)
			{
				return m_targetAnimation;
			}
		}
		return curAnimation;
	}
	
	#endregion
}
