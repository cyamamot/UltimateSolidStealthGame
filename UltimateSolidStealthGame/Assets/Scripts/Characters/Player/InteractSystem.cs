using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSystem : MonoBehaviour {

    [SerializeField]
    float interactDistance;

    PlayerManager manager;

	void Start () {
        manager = GetComponent<PlayerManager>();
        enabled = false;
	}
	
	public void TryInteract() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance)) {
            Interactable interactable = hit.transform.GetComponent<Interactable>();
            if (interactable) {
                interactable.Interact();
            }
        }
    }
}
