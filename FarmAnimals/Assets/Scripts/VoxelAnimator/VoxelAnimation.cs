using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelAnimation : MonoBehaviour 
{
	#region Variables (private)
	[SerializeField] private VoxelAnimator m_animator;
	[SerializeField] private Mesh[] m_frames;
	[SerializeField] private float m_timeStep;
	[SerializeField] private VoxelTransition[] m_transitions = null;
	private int m_curFrame;
	private bool m_isPlaying = false;
	private float m_animationTime = 0;

	#endregion
	
	#region Properties (public)
	public Mesh[] Frames { get { return m_frames; } }
	public int CurrentFrame { get { return m_curFrame; } }
	public bool IsPlaying { get { return m_isPlaying; } }
	#endregion
	
	#region Unity event functions
	
	void Update() 
	{
		
	}
	
	#endregion
	
	#region Methods
	public IEnumerator BeginAnimation()
	{	
		m_isPlaying = true;
		m_curFrame = 0;
		m_animationTime = 0;
		while(m_animationTime < m_frames.Length * m_timeStep)
		{
			m_curFrame = (int)(m_animationTime / m_timeStep);
			m_animator.UpdateMesh(m_frames[m_curFrame]);
			m_animationTime += Time.deltaTime;
			yield return null;
		}
		m_isPlaying = false;
	}

	public void EndAnimation()
	{
		m_animationTime = m_frames.Length * m_timeStep;
	}

	public int CheckTransitions(int curAnimation)
	{
		int transitionAnimation;
		foreach (VoxelTransition transition in m_transitions)
		{
			///TODO
			transitionAnimation = transition.CheckConditions(curAnimation);
			if(transitionAnimation != curAnimation)
			{
				return transitionAnimation;
			}
		}
		return curAnimation;
	}

	#endregion
}
