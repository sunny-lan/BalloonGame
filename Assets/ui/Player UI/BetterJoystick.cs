using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class BetterJoystick : OnScreenControl, IPointerMoveHandler, IPointerExitHandler, IPointerUpHandler
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

		UpdateJoystick(joystick);
	}

	private void UpdateJoystick(Vector2 joystick)
	{
		

		float fixedMagnitude = Mathf.Min(1, joystick.magnitude / radius);

		Vector2 value = joystick.normalized * fixedMagnitude;
		indicator.anchorMax = value/2 + Vector2.one/2;
		indicator.anchorMin = value/2 + Vector2.one/2;
		SendValueToControl(value);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		UpdateJoystick(Vector2.zero);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		UpdateJoystick(Vector2.zero);
	}

	[InputControl(layout = "Vector2")]
	[SerializeField]
	private string m_ControlPath;
	protected override string controlPathInternal
	{
		get => m_ControlPath; set => m_ControlPath = value;
	}
}
