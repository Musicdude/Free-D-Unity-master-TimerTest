using UnityEngine;
using System.Collections;

[System.Serializable]
public class Difficulty 
{
	public DifficultyType	mDifficulty;				// Difficulty Type
	public int 				mNumberOfPlanets;			// Size for Planets
	public int 				mPlanetsSpeed;				// Speed for Planets
	public int				mPlanetDamage;				// Damage for Planets
	public int 				mNumberOfSequenceSets;		// Sequence for Notes
	public int 				mNotesSpeed;				// Speed for Notes
	public int				mNotesPoint;				// Points for Notes
	public int 				mHighScore;					// Highscore for this difficulty
}