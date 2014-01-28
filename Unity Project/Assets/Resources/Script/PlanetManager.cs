using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* <summary>
 * The Planet Manager of the game
 * Handles the spawning and movements
 * of the planets.
 * </summary>
 */
[RequireComponent (typeof(Spawner))]
public class PlanetManager : MonoBehaviour
{
	#region Variables
	[SerializeField] private int 				mSize;									// Size of our Manager
	[SerializeField] private int				mPlanetDamage;							// Damage for each Planet
	[SerializeField] private int				mPlanetSpeed;							// Speed for each Planet
	[SerializeField] private List<GameObject>	mPrefabList = new List<GameObject>();	// Planets Prefabs
	[SerializeField] private List<PlanetAI>		mList		= new List<PlanetAI>();		// Actual List
	private Spawner		mSpawner;														// Spawner Component
	#endregion

	#region Singleton
	private static PlanetManager mInstance;
	public static PlanetManager Instance
	{
		get
		{
			if(mInstance == null)
			{	
				GameObject temp = Resources.Load("Prefabs/PlanetManager") as GameObject;
				mInstance = temp.GetComponent<PlanetManager>();
			}
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		// Checking if there are any duplicates
		if (mInstance == null)			mInstance = this;
		else if(mInstance != this)		
		{
			if(mInstance.gameObject != this.gameObject)	Destroy(gameObject);
			else 										Destroy(this);
		}
		mSpawner 		= gameObject.GetComponent<Spawner>();							// Attach Spawner Component
	}
	private void Start()
	{
		mSize 	 		= GameManager.Instance.GetCurrentDifficulty().mNumberOfPlanets;	// Grab Difficulty
		mPlanetDamage 	= GameManager.Instance.GetCurrentDifficulty().mPlanetDamage;	// Grab Difficulty
		mPlanetSpeed	= GameManager.Instance.GetCurrentDifficulty().mPlanetsSpeed;	// Grab Difficulty

		// Adding Planets into List
		while(mList.Count < mSize)
		{
			GameObject temp = null;
			switch(Utility.GetRandomEnum<PlanetType>())
			{
			case PlanetType.typeA:	temp = Instantiate(mPrefabList[0]) as GameObject; break;
			case PlanetType.typeB:	temp = Instantiate(mPrefabList[1]) as GameObject; break;
			case PlanetType.typeC:	temp = Instantiate(mPrefabList[2]) as GameObject; break;
			case PlanetType.typeD:	temp = Instantiate(mPrefabList[3]) as GameObject; break;
			}
			temp.AddComponent<PlanetAI>();							// Attach Script
			temp.transform.position = mSpawner.SpawnLocation;		// Set the Spawning Location
			temp.transform.parent 	= this.gameObject.transform;	// Set the Game Object to the Manager
			mList.Add(temp.GetComponent<PlanetAI>());
		}
	}
	private void Update()
	{
		if(!Global.mPause)
		{
			// Spawn Planets
			// Update Planets
			if(mSpawner.SpawnTimer < 0)			SpawnPlanet();
			mSpawner.SpawnTimer -= Time.deltaTime;
			if(PlanetMovementHook != null) PlanetMovementHook();
		}
	}
	#endregion

	#region Class Function
	private void SpawnPlanet()
	{
		mSpawner.ResetSpawnTimer();
		for(int i=0;i<mList.Count;i++)
		{
			if(!mList[i].IsEnabled)
			{
				mList[i].transform.position	= mSpawner.SpawnLocation;		// Set the Spawning Location
				mList[i].IsEnabled 			= true;							// enable our planet
				return;														// stop the loop
			}
		}
	}
	public int	PlanetSpeed		{	get	{ return mPlanetSpeed;	}	}
	public int	PlanetDamage	{	get	{ return mPlanetDamage;	}	}
	#endregion

	#region Delegates
	public delegate void PlanetMovementDelegate();
	public event PlanetMovementDelegate PlanetMovementHook;
	#endregion
}