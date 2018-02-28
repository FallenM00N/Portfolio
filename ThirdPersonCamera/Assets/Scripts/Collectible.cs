using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour 
{
	#region Variables (private)
	
	#endregion
	
	#region Properties (public)
	
	#endregion
	
	#region Unity event functions
	
	void Start()
	{
		
	}
	
	void Update() 
	{
		
	}

	private void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "LocalPlayer")
		{
			Debug.Log("Collected");
			bool collected = collider.gameObject.GetComponent<PlayerInventory>().AquireItem(gameObject.tag);
			gameObject.SetActive(!collected);
		}
	}

	#endregion
	
	#region Methods
	
	#endregion
}
