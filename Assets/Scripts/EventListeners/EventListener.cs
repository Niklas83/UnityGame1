using UnityEngine;
using System.Collections;

public enum EventMessage {
	Activate,
	Deactivate,
	ToggleActivate,
}

public class EventListener : MonoBehaviour {

	public void ReceiveEvent(EventMessage iEvent) {
		switch (iEvent) {
		case EventMessage.Activate : 
			SendMessage("SetActive", true);
			break;
		case EventMessage.Deactivate :
			SendMessage("SetActive", false);
			break;
		case EventMessage.ToggleActivate :
			MonoBehaviour[] mb = GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour b in mb) {
				if (b is IActivatable) {
					IActivatable a = (b as IActivatable);
					a.SetActive(!a.IsActive());
				}
			}
			break;
		}
	}
}
