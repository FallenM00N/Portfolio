using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour 
{
	#region Variables (private)
	private List<List<string>> m_inventory = new List<List<string>>();
	
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
	
	#endregion
	
	#region Methods

	public bool AquireItem(string itemTag)
	{
		bool collected = false;
		bool newType = true;
		foreach (List<string> list in m_inventory)
		{
			if(list.Contains("itemTag"))
			{
				list.Add("itmeTag");
				collected = true;
				newType = false;
			}
		}
		if(newType)
		{
			List<string> newList = new List<string>();
			newList.Add("itemTag");
			m_inventory.Add(newList);
			collected = true;	
		}
		return collected;
	}
	
	#endregion
}
