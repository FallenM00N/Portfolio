using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour 
{
	#region Variables (private)
	
	[SerializeField] private Camera m_playerCameraPrefab;
	[SerializeField] private RuntimeAnimatorController m_networkAnimatorController;

	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		if(isLocalPlayer)
		{
			this.gameObject.tag = "LocalPlayer";
			GetComponent<CharacterControllerLogic>().enabled = true;
			Camera playerCam = Instantiate(m_playerCameraPrefab, this.gameObject.transform);
			playerCam.GetComponent<ThirdPersonCamera>().SetupLocalCamera();
			playerCam.enabled = true;
			GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject player in players)
			{
				player.GetComponent<NetworkAnimationController>().ConnectPlayer(player);
				player.GetComponent<NetworkAnimationController>().enabled = true;
			}
		}
		else
		{
			//RuntimeAnimatorController networkController = (RuntimeAnimatorController)Resources.Load("Controller/NetworkController.controller");
			GetComponent<Animator>().runtimeAnimatorController = m_networkAnimatorController;
			GetComponent<NetworkAnimationController>().enabled = true;
			GetComponent<NetworkAnimationController>().ConnectPlayer(this.gameObject);
			GetComponent<NetworkAnimationController>().enabled = true;
		}
	}
	
	void Update() 
	{
		
	}
	
	#endregion
	
	#region Methods
	
	#endregion
}
