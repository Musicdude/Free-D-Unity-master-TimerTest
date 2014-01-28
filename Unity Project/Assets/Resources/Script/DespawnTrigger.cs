using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class DespawnTrigger : MonoBehaviour 
{
	private void Awake()	{		gameObject.collider.isTrigger = true;	}

	private void OnTriggerEnter(Collider _c)
	{
		if(_c.gameObject.GetComponent<PlanetAI>())	
			_c.gameObject.GetComponent<PlanetAI>().IsEnabled = false;
		if(_c.gameObject.GetComponent<NoteAI>())
			_c.gameObject.GetComponent<NoteAI>().IsEnabled = false;
	}
}
