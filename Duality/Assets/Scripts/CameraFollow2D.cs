using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] Transform m_target = null;
	[SerializeField] Vector3 m_offset;
    [SerializeField] [Range(1.0f, 10.0f)] float m_response = 1.0f;
    [SerializeField] [Range(1.0f, 10.0f)] float m_threshold = 1.0f;
    [SerializeField] [Range(0.1f, 10.0f)] float m_shakeAmplitude = 1.0f;
    [SerializeField] [Range(0.1f, 50.0f)] float m_shakeRate = 1.0f;

    Vector3 m_position;
	bool isFollowingX = false;
	bool isFollowingY = false;
    Vector3 m_shake = Vector3.zero;
    float m_shakeAmount = 0.0f;


    void Start()
    {
        m_position = transform.position;
    }

    private void Update()
    {
        m_shakeAmount -= Time.deltaTime;
        m_shakeAmount = Mathf.Clamp01(m_shakeAmount);

        float time = Time.time * m_shakeRate;
        m_shake.x = m_shakeAmount * m_shakeAmplitude * (Mathf.PerlinNoise(time, 0.0f) * 2.0f - 1.0f);
        m_shake.y = m_shakeAmount * m_shakeAmplitude * (Mathf.PerlinNoise(0.0f, time) * 2.0f - 1.0f);
        m_shake.z = 0.0f;
    }

    void LateUpdate()
    {
        Vector3 targetPosition = m_target.position + m_offset;
        targetPosition.z = transform.position.z;
        float distanceX = Mathf.Abs(targetPosition.x - transform.position.x);
        float distanceY = Mathf.Abs(targetPosition.y - transform.position.y);
        Vector3 newTransform = transform.position;
        if (distanceX >= m_threshold || isFollowingX)
		{
			isFollowingX = true;
            newTransform.x = Mathf.Lerp(transform.position.x, targetPosition.x, m_response * Time.deltaTime);
			if(distanceX < m_threshold - 0.1f)
				isFollowingX = false;
		}
        if (distanceY >= m_threshold || isFollowingY)
		{
			isFollowingY = true;
            newTransform.y = Mathf.Lerp(transform.position.y, targetPosition.y, m_response * Time.deltaTime);
			if(distanceY < m_threshold -0.1f)
				isFollowingY = false;
		}
        m_position = Vector3.Lerp(m_position, newTransform, m_response * Time.deltaTime);
        transform.position = m_position + m_shake;
    }

    public void Shake(float amount)
    {
        m_shakeAmount += amount;
    }
}
