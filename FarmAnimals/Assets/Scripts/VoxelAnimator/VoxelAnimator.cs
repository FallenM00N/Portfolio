using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelAnimator : MonoBehaviour 
{
	#region Variables (private)
	[SerializeField]
	[Help("The animation in index 0 will be the default animation.")]
	private VoxelAnimation[] m_animations = null;
	[SerializeField] private FloatParameter[] m_floatParams = null;
	[SerializeField] private BoolParameter[] m_boolParams = null;
	[SerializeField] private TriggerParameter[] m_triggerParams = null;

	private int m_curAnimation = 0;
	private int m_lastFrameAnimation = 0;
	#endregion

	#region Properties (public)
	public FloatParameter[] FloatParams { get { return m_floatParams; } }
	public BoolParameter[] BoolParams { get { return m_boolParams; } }
	public TriggerParameter[] TriggerParams { get { return m_triggerParams; } }

	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		StartCoroutine(m_animations[m_curAnimation].BeginAnimation());
	}
	
	void Update() 
	{
		m_lastFrameAnimation = m_curAnimation;
		m_curAnimation = m_animations[m_curAnimation].CheckTransitions(m_curAnimation);
		if(m_curAnimation != m_lastFrameAnimation)
		{
			m_animations[m_lastFrameAnimation].EndAnimation();
			StartCoroutine(m_animations[m_curAnimation].BeginAnimation());
		}
		else if(!m_animations[m_curAnimation].IsPlaying)
		{
			StartCoroutine(m_animations[m_curAnimation].BeginAnimation());
		}
	}
	
	#endregion
	
	#region Methods
	public void UpdateMesh(Mesh mesh)
	{
		GetComponent<MeshFilter>().mesh = mesh;
	}

	public void SetBool(string parameter, bool value)
	{
		foreach (BoolParameter param in m_boolParams)
		{
			if(param.Name == parameter)
			{
				param.Value = value;
			}
		}
	}

	#endregion
}
