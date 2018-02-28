using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    // A generic animal class to be used on all the animals in the game.
    public enum eState
    {
        FIND_PLAYER,
        IDLE,
        WAIT,
        FIND_FOOD,
    }

    #region Variables (private)
	protected NavMeshAgent m_navMeshAgent = null;



	#endregion
	
	#region Properties (public)
	public virtual Vector3 Destination { get; set; }
    public virtual GameObject Target { get; set; }
    public virtual eState State { get; set; }
    public virtual bool inFormation { get; set; }

	#endregion
	
	#region Unity event functions
    void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        State = eState.IDLE;
        inFormation = false;
    }



    #endregion
	
	#region Methods
    private void SendMessage(Animal target, AnimalActionData.ActionType action)
    {
        // Used to talk to other animals. It will call the RecieveMessage method of the animal (The one below).
        Blackboard.Instance.NotifyAnimal(target, action);
    }
    public void RecieveMessage(AnimalActionData.ActionType action)
    {
        // Do something with value depending on the action
    }
	
	#endregion
}
