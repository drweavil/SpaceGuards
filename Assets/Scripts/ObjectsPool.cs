using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObjectsPool : MonoBehaviour {
	static Dictionary<string, Stack<object>> objects = new Dictionary<string, Stack<object>>();


	public static void PushObject(string path, GameObject pushedObject){
		pushedObject.SetActive (false);

		Stack<object> objectsStack = new Stack<object> ();
		if (objects.TryGetValue (path, out objectsStack)) {
			objectsStack.Push(pushedObject);
		} else {
			objectsStack = new Stack<object> ();
			objectsStack.Push(pushedObject);
			objects.Add (path, objectsStack);
		}
	}

	public static GameObject PullObject(string path){
		Stack<object> foundObjectsStack = new Stack<object> ();
		GameObject foundObject;
		if (objects.TryGetValue (path, out foundObjectsStack)) {
			if (foundObjectsStack.Count == 0) {
				foundObject = Instantiate((GameObject)Resources.Load(path));
				foundObject.SetActive (true);
			} else {
				foundObject = (GameObject)foundObjectsStack.Pop();
				foundObject.SetActive (true);
				//foundObjectsStack.RemoveAt (0);
			}
		} else {
			foundObject = Instantiate((GameObject)Resources.Load(path));
			foundObject.SetActive (true);
		}
		return foundObject;
	}
}
