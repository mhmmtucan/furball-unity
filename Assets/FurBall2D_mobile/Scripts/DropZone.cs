using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
	public void OnPointEnter (PointerEventData eventData)
	{
		
	}

	public void OnPointExit (PointerEventData eventData)
	{

	}

	public void OnDrop (PointerEventData eventData)
	{
		Draggable d = eventData.pointerDrag.GetComponent<Draggable> ();

		if (gameObject.name.Equals ("DropZone")) {
			Destroy (eventData.pointerDrag);
			Destroy (d.placeholder);
		}
		/*
		if (d != null) {
			d.parentToReturnTo = this.transform;
		}
		*/
	}
}
