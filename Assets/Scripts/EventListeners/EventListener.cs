using UnityEngine;
using System.Collections;

public enum EventMessage {
	Activate,
	Deactivate,
	ToggleActivate,
	Register,
	Unregister,
}

public class EventListener : MonoBehaviour {

	public void ReceiveEvent(EventMessage iEvent) {
		switch (iEvent) {
		case EventMessage.Activate : 
			SendMessage("SetActive", true);         //Metodnamnet
			break;
		case EventMessage.Deactivate :
			SendMessage("SetActive", false);
			break;
		case EventMessage.ToggleActivate :                          //används ej just nu
			MonoBehaviour[] mb = GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour b in mb) {
				if (b is IActivatable) {
					IActivatable a = (b as IActivatable);
					a.SetActive(!a.IsActive());
				}
			}
			break;
		case EventMessage.Register :
			SendMessage("Register");
			break;
		case EventMessage.Unregister :
			SendMessage("Unregister");
			break;
		}
	}
}
