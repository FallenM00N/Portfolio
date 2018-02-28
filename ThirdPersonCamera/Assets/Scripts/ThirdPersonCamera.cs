using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CameraPosition
{
    private Vector3 position;

    private Transform xForm;

    public Vector3 Position { get { return position; } set { position = value; } }
    public Transform XForm { get { return xForm; } set { xForm = value; } }

    public void Init(string camName, Vector3 pos, Transform transform, Transform parent)
    {
        position = pos;
        xForm = transform;
        xForm.name = camName;
        xForm.parent = parent;
        xForm.localPosition = Vector3.zero;
        xForm.localPosition = position;
    }
}




[RequireComponent (typeof (BarsEffect))]
public class ThirdPersonCamera : MonoBehaviour
{
    #region Variables (private)
    [SerializeField] private float m_distanceAway;
    [SerializeField] private float m_distanceUp;
    private Vector3 m_offset;
    private CameraPosition m_firstPersonCamPosition;

    [SerializeField] private float m_camSmoothDampTime = 0.1f;
    private Vector3 m_velocityLookDir = Vector3.zero;
    private Vector3 m_velocityCamSmooth = Vector3.zero;
    [SerializeField] private float m_lookDirDampTime = 0.1f;
    [SerializeField] private float m_smooth;
    [SerializeField] private Transform m_followXForm;

    [SerializeField] private float m_targetingTime = 0.5f;
    [SerializeField] private float m_widescreen = 0.2f;
    private BarsEffect m_barsEffect;

    [SerializeField] private float m_firstPersonThreshold = 0.5f;
    [SerializeField] private float m_firstPersonLookSpeed = 3.0f;
    [SerializeField] private float m_fPRotationDegreesPerSecond = 30.0f;
    [SerializeField] private Vector2 m_firstPersonXAxisClamp = new Vector2(-70.0f, 90.0f);
    [SerializeField] private float m_targetingThreshold = 0.5f;

    private CamStates m_camState = CamStates.Behind;

    [SerializeField] CharacterControllerLogic m_follow;
    private float m_xAxisRot = 0.0f;
    private float m_lookWeight;
    private Vector3 m_characterOffset;
    private Vector3 m_curLookDir;
    private Vector3 m_lookDir;
    private Vector3 m_targetPosition;

    #endregion

    #region Properties (public)

    public CamStates CamState { get { return m_camState; } }
    public CameraPosition FirstPersonCamPosition { get { return m_firstPersonCamPosition; } }
    public float LookWeight { get { return m_lookWeight; } }
    public enum CamStates
    {
        Behind,
        FirstPerson,
        Target,
        Free
    }

    #endregion

    #region Unity event functions
    void Start()
    {
        //m_follow = GetComponentInParent<CharacterControllerLogic>();
        //m_followXForm = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Transform>();

        m_lookDir = m_followXForm.forward;
		m_curLookDir = m_followXForm.forward;

        m_barsEffect = GetComponent<BarsEffect>();
        if(m_barsEffect == null)
        {
            Debug.LogError("Attach a widescreen BarsEffect Script to the camera.", this);
        }

        m_firstPersonCamPosition = new CameraPosition();
        m_firstPersonCamPosition.Init
            (
            "First Person Camera",
            new Vector3(0f, 1.6f, 0.2f),
            new GameObject().transform,
            m_followXForm
            );

        m_characterOffset = m_followXForm.position + new Vector3(0f, m_distanceUp, 0f);

        m_curLookDir = m_followXForm.forward;
    }

    void Update()
    {

    }

    private void LateUpdate()
    {
        float rightX = Input.GetAxis("RightStickX");
        float rightY = Input.GetAxis("RightStickY");
        float leftX = Input.GetAxis("Horizontal");
        float leftY = Input.GetAxis("Vertical");


        m_characterOffset = m_followXForm.position + (m_distanceUp * m_followXForm.up);
        Vector3 lookAt =  m_characterOffset;
        m_targetPosition = Vector3.zero;

        if(Input.GetAxis("Target") > m_targetingThreshold)
        {
            m_lookWeight = 0.0f;
            m_barsEffect.coverage = Mathf.SmoothStep(m_barsEffect.coverage, m_widescreen, m_targetingTime);

            m_camState = CamStates.Target;
        }
        else
        {
            m_barsEffect.coverage = Mathf.SmoothStep(m_barsEffect.coverage, 0f, m_targetingTime);

            if (rightY > m_firstPersonThreshold && !m_follow.IsInLocomotion())
            {
                m_xAxisRot = 0;
                m_lookWeight = 1.0f;
                m_camState = CamStates.FirstPerson;
            }
            if ((m_camState == CamStates.FirstPerson && Input.GetButton("ExitFPV")) || (m_camState == CamStates.Target && (Input.GetAxis("Target") <= m_targetingThreshold)))
            {
                m_lookWeight = 0.0f;
                m_camState = CamStates.Behind;
            }
        }

        switch (m_camState)
        {
            case CamStates.Behind:
                ResetCamera();
                if(m_follow.Speed > m_follow.LocomotionThreshold && m_follow.IsInLocomotion() && !m_follow.IsInPivot())
                {
                    Debug.Log("Hello");
                    m_lookDir = Vector3.Lerp(m_followXForm.right * (leftX < 0 ? 1f : -1f), m_followXForm.forward * (leftY < 0 ? -1f : 1f), Mathf.Abs(Vector3.Dot(transform.forward, m_followXForm.forward)));

                    m_curLookDir = Vector3.Normalize(m_characterOffset - transform.position);
                    m_curLookDir.y = 0f;
                    
                    m_curLookDir = Vector3.SmoothDamp(m_curLookDir, m_lookDir, ref m_velocityLookDir, m_lookDirDampTime);
                }

                // m_lookDir = characterOffset - transform.position;
                // m_lookDir.y = 0;
                // m_lookDir.Normalize();
                // Debug.DrawRay(transform.position, m_lookDir, Color.green);
                m_targetPosition = m_characterOffset + m_followXForm.up * m_distanceUp - Vector3.Normalize(m_curLookDir) * m_distanceAway;
                // Debug.DrawLine(m_followXForm.position, m_targetPosition, Color.magenta);
                break;
            case CamStates.FirstPerson:
                m_xAxisRot += (leftY * m_firstPersonLookSpeed);
                m_xAxisRot = Mathf.Clamp(m_xAxisRot, m_firstPersonXAxisClamp.x, m_firstPersonXAxisClamp.y);
                m_firstPersonCamPosition.XForm.localRotation = Quaternion.Euler(m_xAxisRot, 0, 0);

                Quaternion rotationShift = Quaternion.FromToRotation(transform.forward, m_firstPersonCamPosition.XForm.forward);
                transform.rotation = rotationShift * transform.rotation;

                Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, m_fPRotationDegreesPerSecond * (leftX < 0f ? -1f : 1f), 0f), Mathf.Abs(leftX));
                Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
                m_follow.transform.rotation = m_follow.transform.rotation * deltaRotation;

                m_targetPosition = m_firstPersonCamPosition.XForm.position;

                lookAt = Vector3.Lerp(m_targetPosition + m_followXForm.forward, transform.position + transform.forward, m_camSmoothDampTime * Time.deltaTime);

                lookAt = Vector3.Lerp(transform.position + transform.forward, lookAt, Vector3.Distance(transform.position, m_firstPersonCamPosition.XForm.position));
                break;
            case CamStates.Target:
                ResetCamera();

                m_lookDir = m_followXForm.forward;
                m_curLookDir = m_followXForm.forward;


                m_targetPosition = m_characterOffset + m_followXForm.up * m_distanceUp - m_lookDir * m_distanceAway;
                break;
            case CamStates.Free:
                break;
            default:
                break;
        }

        CompensateForWalls(m_characterOffset, ref m_targetPosition);

        SmoothPosition(this.transform.position, m_targetPosition);

        transform.LookAt(lookAt);

    }

    #endregion

    #region Methods

    private void SmoothPosition(Vector3 fromPos, Vector3 toPos)
    {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref m_velocityCamSmooth, m_camSmoothDampTime);
    }

    private void CompensateForWalls(Vector3 fromObject, ref Vector3 toTarget)
    {
        Debug.DrawLine(fromObject, toTarget, Color.cyan);

        RaycastHit wallHit = new RaycastHit();
        if(Physics.Linecast(fromObject, toTarget, out wallHit))
        {
            Debug.DrawRay(wallHit.point, Vector3.left, Color.red);
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }

    private void ResetCamera()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
    }

    public void SetupLocalCamera()
    {
        m_follow = GetComponentInParent<CharacterControllerLogic>();
        m_followXForm = GameObject.FindGameObjectWithTag("LocalPlayer").GetComponent<Transform>(); 
    }

    #endregion






}
