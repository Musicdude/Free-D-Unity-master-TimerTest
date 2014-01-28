
/*<summary>
 * Utility class for Enum and other 
 * useful functions
 * </summary>
 */
public static class Utility
{
	// Random Enum Generator
	// Sorry but I put them this way to make it easier to read
	public static T GetRandomEnum<T>()
	{
		T value = (T) System.Enum.GetValues(typeof(T)).GetValue(
																UnityEngine.Random.Range(
																						 0,
		                                                                                 System.Enum.GetValues(typeof(T)).Length 
		                                                                                 )
		                                                        );	
		return value;
	}
	// Get Enum Value
	public static T GetEnum<T>(int _index)
	{
		T value = (T)System.Enum.GetValues(typeof(T)).GetValue(_index);
		return value;
	}
	// Get the Length of any EnumType
	public static int GetEnumLength<T>()	{	return System.Enum.GetValues(typeof(T)).Length;	}
}