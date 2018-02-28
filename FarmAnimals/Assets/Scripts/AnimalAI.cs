using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalAI : MonoBehaviour
{
    [SerializeField] [Range(1.0f, 50.0f)] float m_speed = 6.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_turnSpeed = 3.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_maxLookDistance = 25.0f;
    [SerializeField] [Range(1.0f, 50.0f)] float m_minLookDistance = 5.0f;
    [SerializeField] [Range(1.0f, 20.0f)] float m_maxLookTime = 5.0f;

    Crop m_targetCrop = null;
    float m_timeOnCurrentCrop = 0.0f;

    void Start()
    {
        FindNextCrop();
    }
    
    void Update()
    {
        if (m_targetCrop)
        {
            Vector3 direction = m_targetCrop.transform.position - transform.position;
            if (direction.magnitude > 2.0f)
            {
                transform.position += (direction.normalized * m_speed * Time.deltaTime);

                m_timeOnCurrentCrop += Time.deltaTime;
                if (m_timeOnCurrentCrop >= m_maxLookTime)
                {
                    FindNextCrop();
                }
            }

            direction.y = 0.0f;
            Quaternion rotation = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_turnSpeed);

            if (m_targetCrop && m_targetCrop.Eaten)
            {
                FindNextCrop();
            }
        }
    }

    public void FindNextCrop()
    {
        m_timeOnCurrentCrop = 0.0f;
        m_targetCrop = FindFood();
    }

    Crop FindFood()
    {
        Crop food = null;

        GameObject[] crops = GameObject.FindGameObjectsWithTag("Crop");

        foreach (GameObject crop in crops)
        {
            float distance = (crop.transform.position - transform.position).magnitude;
            Crop c = crop.GetComponent<Crop>();
            if (distance > m_minLookDistance && distance < m_maxLookDistance && !c.Eaten)
            {
                food = c;
                break;
            }
        }

        return food;
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
