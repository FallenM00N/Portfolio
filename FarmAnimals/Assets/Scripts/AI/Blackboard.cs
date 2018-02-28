using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : Singleton<Blackboard>
{
    Dictionary<string, object> m_posts;

    private void Awake()
    {
        m_posts = new Dictionary<string, object>();
    }

    public void NotifyAnimal(Animal target, AnimalActionData.ActionType type)
    {
        // This is used to talk between animals, and AI in general
        target.RecieveMessage(type);
    }

    public void Post(string key, object value)
    {
        m_posts[key] = value;
    }

    public T Read<T>(string key)
    {
        // Will return the value of the specified key in the dictionary
        // The value will be casted to the type of T, that way the caller doesn't have to cast it to something

        object value = null;

        if (PostExists(key))
        {
            if (m_posts[key].GetType().Equals(typeof(T)))
            {
                value = (T)m_posts[key];
            }
        }

        return (T)value;
    }

    public void DropPost(string key)
    {
        if (PostExists(key))
        {
            m_posts.Remove(key);
        }
    }

    public bool PostExists(string key)
    {
        return (m_posts.ContainsKey(key));
    }
}
