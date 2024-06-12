
using System.Collections;
using System.Linq;
using UnityEngine;

public class ScreenEdgeTracking : BulletHellObj
{
	public enum ScreenSide
	{
		Top, Bottom, Left, Right
	}

	public enum Mode
	{
		TrackPlayer,
		Random
	}

	// Probabilities
	[SerializeField]
	public GenericDictionary<ScreenSide, float> probability = new()
	{
		[ScreenSide.Left] = 0.5f,
		[ScreenSide.Right] = 0.5f,
	};

	public float offset = 0.1f;
	public float trackingDuration = 0.1f;
	public Mode mode = Mode.TrackPlayer;
	public bool setRotation = true;

	public float offsetRandomization = 0;

	public override IEnumerator Fire()
	{
		var screenSide = RandomlySelectSide();

		var yAxis = GetOffsetAxis(screenSide);
		var xAxis = GetMovementAxis(screenSide);
		var yAxisAbs = Vector2.one - xAxis;

		float rng = Random.value;

		if (setRotation)
			child.transform.right = yAxis;

		for (float t = 0; t <= trackingDuration; t += Time.deltaTime)
		{
			var line = GetLine(screenSide);
			float startX = Vector2.Dot(line.start, xAxis),
				endX = Vector2.Dot(line.end, xAxis);

			var x = mode switch
			{
				Mode.TrackPlayer => Mathf.Clamp(
					Vector2.Dot(GameManager.LevelManager.Player.transform.position, xAxis),
					startX, endX
				),
				Mode.Random => Mathf.Lerp(startX, endX, rng),
				_ => throw new System.NotImplementedException(),
			};

			var y = offset * yAxis
				+ offsetRandomization * yAxis
				+ Vector2.Scale(line.start, yAxisAbs);

			var pos = x * xAxis
				+ y;
			child.transform.position = pos;

			Debug.Log($"side={screenSide} line={line} x={x} y={y} pos={pos}");

			yield return null;
		}
	}

	private (Vector2 start, Vector2 end) GetLine(ScreenSide s)
	{
		var cam = GameManager.LevelManager.Camera;
		return s switch
		{
			ScreenSide.Top => (cam.ViewportToWorldPoint(new(0, 1, 0)), cam.ViewportToWorldPoint(new(1, 1, 0))),
			ScreenSide.Bottom => (cam.ViewportToWorldPoint(new(0, 0, 0)), cam.ViewportToWorldPoint(new(1, 0, 0))),
			ScreenSide.Left => (cam.ViewportToWorldPoint(new(0, 0, 0)), cam.ViewportToWorldPoint(new(0, 1, 0))),
			ScreenSide.Right => (cam.ViewportToWorldPoint(new(1, 0, 0)), cam.ViewportToWorldPoint(new(1, 1, 0))),
			_ => throw new System.NotImplementedException(),
		};
	}

	private Vector2 GetOffsetAxis(ScreenSide s)
	{
		return s switch
		{
			ScreenSide.Top => Vector2.down,
			ScreenSide.Bottom => Vector2.up,
			ScreenSide.Left => Vector2.right,
			ScreenSide.Right => Vector2.left,
			_ => throw new System.NotImplementedException(),
		};
	}

	private Vector2 GetMovementAxis(ScreenSide s)
	{
		return s switch
		{
			ScreenSide.Top => Vector2.right,
			ScreenSide.Bottom => Vector2.right,
			ScreenSide.Left => Vector2.up,
			ScreenSide.Right => Vector2.up,
			_ => throw new System.NotImplementedException(),
		};
	}

	private ScreenSide RandomlySelectSide()
	{
		return probability.SelectByProbability();
	}
}
