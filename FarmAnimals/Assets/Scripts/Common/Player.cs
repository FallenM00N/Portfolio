using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 100.0f)] float m_speed = 5.0f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_turnSpeed = 1.0f;
    [SerializeField] bool m_firstPlayer;

    [SerializeField] VoxelAnimator m_animator;

    public bool m_FirstPlayer { get { return m_firstPlayer; } }
    public int Score { get; set; }
    public bool Alive { get; set; }

    void Start()
    {

    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 rotate = Vector3.zero;
        float reverse = 1.0f;

        if (m_firstPlayer)
        {
            velocity.z = Input.GetAxis("Vertical1") * m_speed;

            if (velocity.z < -0.1f)
                reverse = -1.0f;

            rotate.y = (Input.GetAxis("Horizontal1") * m_turnSpeed) * reverse;
        }
        else
        {
            velocity.z = Input.GetAxis("Vertical2") * m_speed;

            if (velocity.z < -0.1f)
                reverse = -1.0f;

            rotate.y = (Input.GetAxis("Horizontal2") * m_turnSpeed) * reverse;
            if(velocity.magnitude > 0.2)
            {
                //m_animator.SetBool("Walking", true);
            }
            else
            {
                //m_animator.SetBool("Walking", false);
            }
        }

        transform.rotation = transform.rotation * Quaternion.Euler(rotate * Time.deltaTime);

        transform.position += ((transform.rotation * velocity) * Time.deltaTime);
    }
}
