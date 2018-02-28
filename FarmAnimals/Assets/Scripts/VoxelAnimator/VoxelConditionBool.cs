using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelConditionBool : VoxelCondition
{
	#region Variables (private)
	[Help("Whether the parameter should be true or false to transition.")]
	[SerializeField] bool m_valueToTransition;

	private BoolParameter m_parmeter;
	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		foreach (BoolParameter parameter in m_animator.BoolParams)
		{
			if(parameter.Name == m_paramName)
			{
				m_parmeter = parameter;
				break;
			}
		}
	}
	
	#endregion
	
	#region Methods
	override
	public bool CheckCondition()
	{
		return m_parmeter.Value == m_valueToTransition;
	}

	#endregion
}
