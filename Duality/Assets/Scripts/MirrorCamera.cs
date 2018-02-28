using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MirrorCamera : MonoBehaviour 
{
	#region Variables (private)
	private Camera m_camera;

	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		m_camera = GetComponent<Camera>();
	}
	
	void Update() 
	{
		
	}

	private void OnPreCull()
	{
		m_camera.ResetWorldToCameraMatrix();
		m_camera.ResetProjectionMatrix();
		m_camera.projectionMatrix = m_camera.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
	}

	private void OnPreRender()
	{
		GL.invertCulling = true;
	}

	private void OnPostRender()
	{
		GL.invertCulling = false;
	}
	
	#endregion
	
	#region Methods
	
	#endregion
}
