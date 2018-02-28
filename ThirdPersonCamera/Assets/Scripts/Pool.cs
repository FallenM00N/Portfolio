using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] int m_size = 1;
    [SerializeField] GameObject m_gameObject;

    List<GameObject> m_gameObjects = new List<GameObject>();

    void Start()
    {
        for(int i = 0; i < m_size; i++)
        {
            GameObject go = Instantiate(m_gameObject, this.transform);
            m_gameObjects.Add(go);
        }
    }

    public GameObject Get()
    {
        for(int i = 0; i < m_gameObjects.Count; i++)
        {
            if(!m_gameObjects[i].activeInHierarchy)
            {
                return m_gameObjects[i];
            }
        }
        return null;
    }

}
