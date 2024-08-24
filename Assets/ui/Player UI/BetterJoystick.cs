using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class BetterJoystick : OnScreenControl, IPointerMoveHandler
{
	[SerializeField] RectTransform indicator;

	public float radius = 100;

	public void OnPointerMove(PointerEventData evt)
	{

		Vector2 joystick = Vector2.zero;
		if (evt.pointerCurrentRaycast.gameObject == gameObject)
		{

			joystick = evt.position - transform.position.XY();
		}

		indicator.anchoredPosition = joystick;

		float fixedMagnitude = Mathf.Min(1, joystick.magnitude / radius);

		SendValueToControl(joystick.normalized * fixedMagnitude);
	}


	[InputControl(layout = "Vector2")]
	[SerializeField]
	private string m_ControlPath;
	protected override string controlPathInternal
	{
		get => m_ControlPath; set => m_ControlPath = value;
	}
}
