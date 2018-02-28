using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VoxelCondition : MonoBehaviour 
{
	#region Variables (private)
	[SerializeField] protected VoxelAnimator m_animator;
	[SerializeField] protected string m_paramName;
	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		
	}
	
	#endregion
	
	#region Methods
	public abstract bool CheckCondition();
	
	#endregion
}
