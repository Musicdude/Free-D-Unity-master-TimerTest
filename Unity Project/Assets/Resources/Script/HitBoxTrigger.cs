using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class HitBoxTrigger : MonoBehaviour
{
	private void OnTriggerEnter(Collider _c)
	{
		if(_c.gameObject.GetComponent<PlanetAI>())
		{
			Debug.Log("Hit until Planet");
			PlanetAI planet = _c.gameObject.GetComponent<PlanetAI>();
			PlayerController.Instance.SubtractHealth(planet.Damage);
			#if UNITY_ANDROID || UNITY_IPHONE
				Handheld.Vibrate();
			#endif
			planet.IsEnabled = false;
			StartCoroutine( EffectManager.Instance.PlayExplosion(2.0f,_c.gameObject) );
		}
		if(_c.gameObject.GetComponent<NoteAI>())
		{
			Debug.Log("Hit until Notes");
			NoteAI note = _c.gameObject.GetComponent<NoteAI>();
			if(NoteSequence.Instance.GetCurrentNote() == note.Type)
			{
				NoteSequence.Instance.NextNote();
				switch(NoteSequence.Instance.GetCurrentNote())
				{
					case NoteType.typeA:	EffectManager.Instance.ChangeAuroraWaveColor(Color.cyan);	break;
					case NoteType.typeB:	EffectManager.Instance.ChangeAuroraWaveColor(Color.yellow);	break;
					case NoteType.typeC:	EffectManager.Instance.ChangeAuroraWaveColor(Color.green);	break;
					case NoteType.typeD:	EffectManager.Instance.ChangeAuroraWaveColor(Color.red);	break;
				}
				note.IsEnabled = false;
				StartCoroutine( EffectManager.Instance.PlayFireworks(1.0f) );
				PointsManager.Instance.CurrentScore += PointsManager.Instance.NotePoints;

			}
		}
	}
}