using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WorldState
{
    Light,
    Dark
}

public class WorldStateController : MonoBehaviour 
{
	private static readonly IDictionary<WorldState, WorldState> ToggleStateMap = new Dictionary<WorldState, WorldState>
	{
		{WorldState.Light, WorldState.Dark},
		{WorldState.Dark, WorldState.Light}
	};
	    private static readonly IDictionary<WorldState, Color> StateColorMap = new Dictionary<WorldState, Color>
    {
        {WorldState.Dark, Color.black},
        {WorldState.Light, Color.white}
    };
	private static IDictionary<WorldState, PlayerController> ActivePlayerStateMap;

	#region Variables (private)
	[SerializeField] [Range(0.1f, 3.0f)] private float m_flipDuration = 1.0f;
	[SerializeField] private Camera m_mainCamera;
    [SerializeField] private Camera[] m_renderCameras = new Camera[2];
    [SerializeField] private AudioListener[] m_audioListeners = new AudioListener[2];
	[SerializeField] private WorldCanvasController m_worldCanvasController = null;
	[SerializeField] private static PlayerController s_lightPlayerController;
	[SerializeField] private PlayerController m_setLightPlayerController;
	[SerializeField] private static PlayerController s_darkPlayerController;
	[SerializeField] private PlayerController m_setDarkPlayerController;
	private WorldState m_worldState;

	#endregion
	
	#region Properties (public)
	public WorldState State { get { return m_worldState; } }

	#endregion
	
	#region Unity event functions
	
	void Awake()
	{
		s_darkPlayerController = m_setDarkPlayerController;
		s_lightPlayerController = m_setLightPlayerController;
		ActivePlayerStateMap = new Dictionary<WorldState, PlayerController>
		{
			{WorldState.Dark, s_darkPlayerController},
			{WorldState.Light, s_lightPlayerController}
		};
	}

	void Start()
	{
		m_worldState = WorldState.Light;
		s_lightPlayerController.SetActive(true);
	}
	
	void Update() 
	{
		if(Input.GetButtonDown("Flip"))
		{
			if(!m_worldCanvasController.IsFlipping)
			{
				Flip();
			}
		}
	}
	
	#endregion
	
	#region Methods

	private void Flip()
	{	
		ActivePlayerStateMap[m_worldState].SetActive(false);
		m_worldState = ToggleStateMap[m_worldState];
		ActivePlayerStateMap[m_worldState].SetActive(true);
		m_worldCanvasController.FlipCanvas(m_worldState, m_flipDuration);
		StartCoroutine(SetBackgroundColor(m_flipDuration, m_worldState));
		StartCoroutine(s_darkPlayerController.PauseDuringFlip(m_flipDuration));
		StartCoroutine(s_lightPlayerController.PauseDuringFlip(m_flipDuration));
		SwitchAudioListener();
	}

	    private void SwitchAudioListener()
    {
        m_audioListeners[(int)ToggleStateMap[m_worldState]].enabled = false;
        m_audioListeners[(int)m_worldState].enabled = true;
    }
	private IEnumerator SetBackgroundColor(float duration, WorldState worldState)
    {
        Color startColor = m_mainCamera.backgroundColor;
        Color endColor = StateColorMap[worldState];
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            Color color = Color.Lerp(startColor, endColor, t / duration);
            m_mainCamera.backgroundColor = color;
            m_renderCameras[0].backgroundColor = color;
            m_renderCameras[1].backgroundColor = color;
            yield return null;
        }
    }
	
	#endregion
}
