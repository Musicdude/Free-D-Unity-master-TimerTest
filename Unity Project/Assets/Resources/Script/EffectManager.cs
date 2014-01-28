using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectManager : MonoBehaviour 
{
	[SerializeField] private List<GameObject> mList = new List<GameObject>();	// Effects List

	#region Singleton
	private static EffectManager mInstance;
	public static EffectManager Instance
	{
		get
		{
			if(mInstance == null)
				mInstance = GameObject.Find("EffectManager").GetComponent<EffectManager>();
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		// Checking if there are any duplicates
		if(mInstance == null)			mInstance = this;
		else if(mInstance != this)		
		{
			if(mInstance.gameObject != this.gameObject)	Destroy(gameObject);
			else 										Destroy(this);
		}
	}

	#endregion

	#region Class Function
	public IEnumerator PlayFireworks(float _lastTiming)
	{
		for(int i=0;i<mList.Count;i++)
		{
			if(!mList[i].activeSelf && mList[i].name == "Fireworks")
			{
				mList[i].transform.position = new Vector3(	PlayerController.Instance.gameObject.transform.position.x,
				                                          	mList[i].transform.position.y,
				                                         	mList[i].transform.position.z);
				mList[i].SetActive(true);
				yield return new WaitForSeconds(_lastTiming);
				mList[i].SetActive(false);
				break;
			}
		}
	}

	public IEnumerator PlayExplosion(float _lastTiming,GameObject _object)
	{
		for(int i=0;i<mList.Count;i++)
		{
			if(!mList[i].activeSelf && mList[i].name == "Small Explosion")
			{
				SoundManager.Instance.Play("Explosion");
				mList[i].transform.position = new Vector3(	_object.transform.position.x,
				                                            _object.transform.position.y,
				                                            _object.transform.position.z);
				mList[i].SetActive(true);
				yield return new WaitForSeconds(_lastTiming);
				mList[i].SetActive(false);
				break;
			}
		}
	}

	public void ChangeAuroraWaveColor(Color _color)
	{
		mList[2].GetComponent<ParticleRenderer>().material.SetColor( "_EmisColor",_color );
		mList[2].GetComponentInChildren<Light>().color	= _color;
	}
	#endregion
}
