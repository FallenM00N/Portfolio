using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCamera : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 10.0f)] float m_distance;
    [SerializeField] [Range(1.0f, 20.0f)] float m_upOffset = 1.0f;
    [SerializeField] [Range(1.0f, 30.0f)] float m_lookSpeed = 1.0f;
    [SerializeField] [Range(1.0f, 90.0f)] float m_pitch = 1.0f;
    [SerializeField] [Range(0.1f, 10.0f)] float m_response = 1.0f;
    [SerializeField] Player m_target;

    float m_yaw = 0.0f;

    void Update()
    {
        if (m_target.m_FirstPlayer)
            m_yaw += Input.GetAxis("RightJoystickY1") * m_lookSpeed;
        else
            m_yaw += Input.GetAxis("RightJoystickY2") * m_lookSpeed;

        //m_pitch = Mathf.Clamp(m_pitch, m_pitchMin, m_pitchMax); //Used for camera rotation along the x axis. Currently a static variable for pitch.

        Quaternion view = /*m_target.transform.rotation **/ Quaternion.Euler(m_pitch, m_yaw, 0.0f); //Target transform rotation is used to stay snapped to the target object otherwise the camera stays static
        Vector3 position = m_target.transform.position;
        Vector3 offset = (view * Vector3.back * m_distance) + Vector3.up * m_upOffset;

        Vector3 targetPosition = Vector3.Lerp(transform.position, position + offset, Time.deltaTime * m_response);
        Quaternion targetRotation = Quaternion.Lerp(transform.rotation, view, Time.deltaTime * m_response);
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }
}
