using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterControllerLogic : MonoBehaviour
{
    #region Variables (private)
    private Animator m_animator;
    private AnimatorStateInfo m_stateInfo;
    [SerializeField] private float m_directionDampTime = 0.25f;
    [SerializeField] private float m_speedDampTime = 0.05f;
    [SerializeField] private float m_directionSpeed = 3.0f;
    [SerializeField] private ThirdPersonCamera m_gameCamera;
    [SerializeField] private float m_rotationDegreesPerSecond = 120f;

    private float m_direction = 0.0f;
    private float m_charAngle = 0.0f;
    private float m_speed = 0.0f;
    private float m_horizontal = 0.0f;
    private float m_vertical = 0.0f;

    private int m_locomotionId = 0;
    private int m_locomotionPivotLId = 0;
    private int m_locomotionPivotRId = 0;
    private int m_idlePivotRId = 0;
    private int m_idlePivotLId = 0;

	#endregion
	
	#region Properties (public)
	public Animator Animator { get { return m_animator; } }

    public float Speed { get { return m_speed; } }

    public float LocomotionThreshold { get { return 0.1f; } }

	#endregion
	
	#region Unity event functions
	
	void Start()
	{
        m_animator = GetComponent<Animator>();
        if(m_animator.layerCount >= 2)
        {
            m_animator.SetLayerWeight(1, 1);
        }

        m_locomotionId = Animator.StringToHash("Base Layer.Locomotion");
        m_locomotionPivotLId = Animator.StringToHash("Base Layer.LocomotionPivotL");
        m_locomotionPivotRId = Animator.StringToHash("Base Layer.LocomotionPivotR");
        m_idlePivotLId = Animator.StringToHash("Base Layer.IdlePivotL");
        m_idlePivotRId = Animator.StringToHash("Base Layer.IdlePivotR");
        m_gameCamera = GetComponentInChildren<ThirdPersonCamera>();
	}
	
	void Update() 
	{
        m_stateInfo = m_animator.GetCurrentAnimatorStateInfo(0);
	    if(m_animator && m_gameCamera.CamState != ThirdPersonCamera.CamStates.FirstPerson)
        {
            m_horizontal = Input.GetAxis("Horizontal");
            m_vertical = Input.GetAxis("Vertical");

            m_charAngle = 0f;
            m_direction = 0f;
            // m_speed = m_horizontal * m_horizontal + m_vertical * m_vertical;
            
            StickToWorldSpace(this.transform, m_gameCamera.transform, ref m_direction, ref m_speed, ref m_charAngle, IsInPivot());
            m_animator.SetFloat("Direction", m_direction, m_directionDampTime, Time.deltaTime);
            m_animator.SetFloat("Speed", m_speed, m_speedDampTime, Time.deltaTime);

            if(m_speed > LocomotionThreshold)
            {
                if(!IsInPivot())
                {
                    m_animator.SetFloat("Angle", m_charAngle);
                }
            }
            else if(Mathf.Abs(m_horizontal) < 0.05f)
            {
                m_animator.SetFloat("Direction", 0f);
                m_animator.SetFloat("Angle", 0f);
            }
        }
	}

    private void FixedUpdate()
    {
        if(IsInLocomotion() && m_gameCamera.CamState != ThirdPersonCamera.CamStates.Free && !IsInPivot() && ((m_direction >= 0 && m_vertical >= 0) || (m_direction < 0 && m_horizontal < 0)))
        {
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, m_rotationDegreesPerSecond * (m_horizontal < 0f ? -1f : 1f), 0f), Mathf.Abs(m_horizontal));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            this.transform.rotation = (this.transform.rotation * deltaRotation);
        }
    }

    private void OnAnimatorIK()
    {


        switch (m_gameCamera.CamState)
        {
            case ThirdPersonCamera.CamStates.FirstPerson:
                Animator.SetLookAtWeight(m_gameCamera.LookWeight);
                Animator.SetLookAtPosition(m_gameCamera.FirstPersonCamPosition.XForm.position + m_gameCamera.FirstPersonCamPosition.XForm.forward);
                break;
            default:
                Animator.SetLookAtWeight(m_gameCamera.LookWeight);
                Animator.SetLookAtPosition(transform.forward);
                break;
        }



    }

    #endregion

    #region Methods
    public void StickToWorldSpace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, bool isPivoting)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(m_horizontal, 0, m_vertical);

        speedOut = stickDirection.sqrMagnitude;

        //Get Camera Rotation
        Vector3 cameraDirection = camera.forward;
        cameraDirection.y = 0.0f;
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(cameraDirection));

        //Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.0f, root.position.z), moveDirection, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.0f, root.position.z), axisSign, Color.green);
        Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.0f, root.position.z), rootDirection, Color.magenta);
        //Debug.DrawRay(new Vector3(root.position.x, root.position.y + 2.0f, root.position.z), stickDirection, Color.blue);
        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);

        if(!isPivoting)
        {
            angleOut = angleRootToMove;
        }
        
        angleRootToMove /= 180f;

        directionOut = angleRootToMove * m_directionSpeed;
    }

    public bool IsInLocomotion()
    {
        bool result = m_stateInfo.fullPathHash == m_locomotionId;
        return result;
    }

    public bool IsInPivot()
    {
        bool result = m_stateInfo.fullPathHash == m_locomotionPivotLId || m_stateInfo.fullPathHash == m_locomotionPivotRId
                        || m_stateInfo.fullPathHash == m_idlePivotLId || m_stateInfo.fullPathHash == m_idlePivotRId;
        return result;
    }

	#endregion
}
