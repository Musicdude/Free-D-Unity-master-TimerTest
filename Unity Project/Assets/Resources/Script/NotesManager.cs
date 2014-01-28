using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(Spawner))]
[RequireComponent (typeof(NoteSequence))]

public class NotesManager : MonoBehaviour 
{
	[SerializeField] private GameObject[]	mPrefabList = new GameObject[4];	// Prefab List
	[SerializeField] private GameObject		mBubblePrefab;						// Bubble Prefab
	[SerializeField] private List<NoteAI> 	mList		= new List<NoteAI>();	// Actual List
	[SerializeField] private int			mNoteSetSize;						// Number of Sets
	[SerializeField] private int			mNoteMovementSpeed;
	private	Spawner		mSpawner;	// spawner component
	#region Singleton
	private static NotesManager mInstance;
	public static NotesManager Instance
	{
		get
		{
			if(mInstance == null)
			{	mInstance = Resources.Load("NotesManager") as NotesManager;	}
			return mInstance;
		}
	}
	#endregion

	#region Unity Function
	private void Awake()
	{
		// Checking if there are any duplicates
		if (mInstance == null)		
		{	
			mInstance = this;
		}
		else if(mInstance != this)		
		{
			if(mInstance.gameObject != this.gameObject)	Destroy(gameObject);
			else 										Destroy(this);
		}
		mSpawner = gameObject.GetComponent<Spawner>();

	}
	private void Start()
	{
		mNoteMovementSpeed = GameManager.Instance.GetCurrentDifficulty().mNotesSpeed;

		for(int i=0;i<mNoteSetSize*mPrefabList.Length;i++)
		{
			GameObject temp 	= Instantiate(mPrefabList[i%mPrefabList.Length]) as GameObject;
			GameObject bubble 	= Instantiate(mBubblePrefab) as GameObject;
			bubble.transform.parent = temp.transform;
			temp.AddComponent<NoteAI>();
			temp.GetComponent<NoteAI>().Type = Utility.GetEnum<NoteType>(i%mPrefabList.Length);
			temp.GetComponent<NoteAI>().MovementSpeed = mNoteMovementSpeed;
			temp.transform.position	= mSpawner.SpawnLocation;
			temp.transform.parent	= this.transform;
			mList.Add(temp.GetComponent<NoteAI>());
		}
	}

	private void Update()
	{
		if(!Global.mPause)
		{
			if(mSpawner.SpawnTimer < 0)	SpawnNotes();
			mSpawner.SpawnTimer -= Time.deltaTime;
			if(UpdateMovementHook != null)	UpdateMovementHook();
		}
	}
	#endregion

	#region Class Function
	public int GetPrefabListLength()	{	return mPrefabList.Length;	}
	private void SpawnNotes()
	{
		NoteType requiredNote = NoteSequence.Instance.GetCurrentNote();
		mSpawner.ResetSpawnTimer();

		//Checks for note that is spawned
		foreach(NoteAI note in mList)
		{
			if(note.IsEnabled)
			{
				if(requiredNote == note.Type )	// If the Note we want is on the field
				{
					NoteType randomNote = Utility.GetRandomEnum<NoteType>();
					foreach(NoteAI temp in mList)
					{
						// should be part of player sequence
						// note enabled spawn him (for now)
						if(temp.Type == randomNote)
						{
							if(!temp.IsEnabled)
							{
								temp.transform.position = mSpawner.SpawnLocation;		// Set the Spawning Location
								temp.IsEnabled = true;
								return;
							}
						}
					}
					return;	// Do Not Spawn
				}
			}
		}
		foreach(NoteAI note in mList)
		{
			// should be part of player sequence
			// note enabled spawn him (for now)
			if(note.Type == requiredNote)
			{
				note.transform.position = mSpawner.SpawnLocation;		// Set the Spawning Location
				note.IsEnabled = true;
				return;
			}
		}

	}
	#endregion

	#region Delegate
	public delegate void UpdateMovementDelegate();
	public event UpdateMovementDelegate UpdateMovementHook;
	#endregion
}
