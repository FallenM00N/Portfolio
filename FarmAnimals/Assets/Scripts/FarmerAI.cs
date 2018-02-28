using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerAI : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 50.0f)] float m_speed = 6.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_jumpForce = 6.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_jumpCooldown = 1.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_turnSpeed = 3.0f;
    [SerializeField] LayerMask m_jumpMask;

    GameObject m_targetAnimal = null;
    Rigidbody m_rigidbody;
    float m_jumpTime = 0.0f;

    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();

        FindNextAnimal();
    }

    void Update()
    {
        if (m_targetAnimal)
        {
            Vector3 direction = m_targetAnimal.transform.position - transform.position;
            transform.position += (direction.normalized * m_speed * Time.deltaTime);

            direction.y = 0.0f;
            Quaternion rotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_turnSpeed);

            m_jumpTime += Time.deltaTime;
            RaycastHit raycastHit;
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 10.0f, m_jumpMask))
            {
                Debug.DrawLine(transform.position, raycastHit.point, Color.red, 1.0f);
                if (raycastHit.distance <= 1.0f && m_jumpTime >= m_jumpCooldown)
                {
                    m_jumpTime = 0.0f;
                    Jump();
                }
            }
        }

        if (!m_targetAnimal)
        {
            FindNextAnimal();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Alive = false;
            FindNextAnimal();
        }
        else if (collision.gameObject.CompareTag("AI"))
        {
            collision.gameObject.GetComponent<AnimalAI>().Die();
        }
    }

    private void Jump()
    {
        m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        //print("JUMP");
    }

    public void FindNextAnimal()
    {
        m_targetAnimal = FindAnimal();
    }

    GameObject FindAnimal()
    {
        GameObject animal = null;

        GameObject[] animals = GameObject.FindGameObjectsWithTag("AI");

        float distance = float.MaxValue;
        foreach (GameObject anim in animals)
        {
            float curDist = (anim.transform.position - transform.position).magnitude;
            if (anim && curDist < distance)
            {
                distance = curDist;
                animal = anim;
            }
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        
        foreach (GameObject player in players)
        {
            float curDist = (player.transform.position - transform.position).magnitude;
            if (player && curDist < distance && player.GetComponent<Player>().Alive)
            {
                distance = curDist;
                animal = player;
            }
        }

        return animal;
    }
}
