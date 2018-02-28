using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelConditionFloat : VoxelCondition
{
	#region Variables (private)
	#if UNITY_EDITOR
	[Help("Whether the parameter should be greater than or less than the value to transition. Select one.\n" +
			"(If both or neither are selected, greater than will be used)", UnityEditor.MessageType.None)]
	#endif
	[SerializeField] private bool m_paramGreaterThan;
	[SerializeField] private bool m_paramLessThan;
	[SerializeField] private float m_valueToTransition;

	private FloatParameter m_parameter;
	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		foreach (FloatParameter parameter in m_animator.FloatParams)
		{
			if(parameter.Name == m_paramName)
			{
				m_parameter = parameter;
				break;
			}
		}
	}
	
	#endregion
	
	#region Methods

	override
	public bool CheckCondition()
	{
		if(m_paramLessThan && !m_paramGreaterThan)
			return m_parameter.Value < m_valueToTransition;
		else
			return m_parameter.Value > m_valueToTransition;
	}
	
	#endregion
}
