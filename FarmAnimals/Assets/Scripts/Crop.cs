using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Crop : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 20.0f)] float m_maxLife = 3.0f;
    [SerializeField] [Range(0.1f, 20.0f)] float m_respawnTime = 6.0f;
    [SerializeField] GameObject m_cropHUD;

    ParticleSystem m_particleSystem;
    SphereCollider m_collider;
    MeshRenderer m_meshRenderer;
    TextMeshProUGUI m_hudText;
    float m_life;
    float m_time;

    public bool Eaten { get { return m_life <= 0.0f; } }

    private void Start()
    {
        m_collider = GetComponent<SphereCollider>();
        m_meshRenderer = GetComponentInChildren<MeshRenderer>();
        m_particleSystem = GetComponentInChildren<ParticleSystem>();
        if (m_cropHUD)
        {
            m_hudText = m_cropHUD.GetComponentInChildren<TextMeshProUGUI>();
        }

        m_life = m_maxLife;
        m_time = 0.0f;

        StartCoroutine(ScaleUp());
    }

    private void Update()
    {
        if (Eaten)
        {
            m_time += Time.deltaTime;
            if (m_time >= m_respawnTime)
            {
                Respawn();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (other.CompareTag("Player") && !Eaten && player.Alive)
        {
            string button = player.m_FirstPlayer ? "Eat1" : "Eat2";
            if (Input.GetButton(button))
            {
                NibbleOnCrop(player);
                AudioManager.Instance.PlayClip("Eat", transform.position);
            }
            if (Input.GetButtonUp(button))
            {
                m_life = m_maxLife;
            }
            m_cropHUD.SetActive(true);

            Camera[] cameras = Camera.allCameras;

            Vector3 direction1 = cameras[0].transform.position - transform.position;
            Vector3 direction2 = cameras[1].transform.position - transform.position;

            bool camera1Closer = (direction1.magnitude < direction2.magnitude);
            GameObject target = camera1Closer ? cameras[0].gameObject : cameras[1].gameObject;

            Vector3 direction = target.transform.position - transform.position;
            direction.x = 0.0f;
            direction.z = 0.0f;
            m_hudText.transform.LookAt(target.transform.position - direction);
            m_hudText.transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        else if (other.CompareTag("AI"))
        {
            AIOnCrop(other.GetComponent<AnimalAI>());
            AudioManager.Instance.PlayClip("Eat", transform.position);
        }

        if (Eaten)
        {
            m_cropHUD.SetActive(false);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("AI"))
        {
            m_life = m_maxLife;

            m_cropHUD.SetActive(false);
        }
    }

    void AIOnCrop(AnimalAI ai)
    {
        m_life -= Time.deltaTime;
        if (Eaten)
        {
            Die();
            ai.FindNextCrop();
        }
        else
        {
            m_particleSystem.Play();
        }
    }

    void NibbleOnCrop(Player player)
    {
        if (player)
        {
            m_life -= Time.deltaTime;
            if (Eaten)
            {
                player.Score += (int)(m_maxLife * 2.0f);
                Die();
            }
            else
            {
                m_particleSystem.Play();
            }
        }
    }

    void Die()
    {
        m_collider.enabled = false;
        m_meshRenderer.enabled = false;
        m_cropHUD.SetActive(false);
    }

    void Respawn()
    {
        m_time = 0.0f;
        m_life = m_maxLife;
        m_collider.enabled = true;
        m_meshRenderer.enabled = true;
        StartCoroutine(ScaleUp());
    }

    IEnumerator ScaleUp()
    {
        transform.localScale = Vector3.zero;

        for (float i = 0.0f; i <= 1.0f; i += Time.deltaTime)
        {
            transform.localScale = new Vector3(i, i, i);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
