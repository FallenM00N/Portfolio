using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCanvasController : MonoBehaviour
{
    #region Variables (private)
    private bool m_isFlipping = false;

    #endregion

    #region Properties (public)
    public bool IsFlipping { get { return m_isFlipping; } }

    #endregion

    #region Unity event functions

    void Start()
    {

    }

    void Update()
    {

    }

    #endregion

    #region Methods

    public void FlipCanvas(WorldState worldState, float flipDuration)
    {
        m_isFlipping = true;
        StartCoroutine(FlipCanvas(flipDuration));
    }

    private IEnumerator FlipCanvas(float duration)
    {
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 180.0f;
        float t = 0.0f;
        while (t < duration)
        {
            // if(t > duration / 2)
            // {
            // 	SetBackgroundColor();
            // }
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            yield return null;
        }
        m_isFlipping = false;
    }

    #endregion
}
