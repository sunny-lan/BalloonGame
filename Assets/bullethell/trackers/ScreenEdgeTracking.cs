
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

	public override IEnumerator Fire()
	{
		var screenSide = RandomlySelectSide();
		var axis = GetAxis(screenSide);
		var movementAxis = GetMovementAxis(screenSide);
		float rng = Random.value;

		child.transform.right = axis;

		for (float t = 0; t <= trackingDuration; t += Time.deltaTime)
		{
			var line = GetLine(screenSide);
			float start = Vector2.Dot(line.start, movementAxis),
				end = Vector2.Dot(line.end, movementAxis);

			var movement = mode switch
			{
				Mode.TrackPlayer => Mathf.Clamp(
					Vector2.Dot(GameManager.LevelManager.Player.transform.position, movementAxis),
					start, end
				),
				Mode.Random => Mathf.Lerp(start, end, rng),
				_ => throw new System.NotImplementedException(),
			};

			var pos = movement * movementAxis + axis * offset + Vector2.Scale(line.start, axis);
			child.transform.position = pos;

			yield return new WaitForEndOfFrame();
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

	private Vector2 GetAxis(ScreenSide s)
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
		float total = probability.Values.Sum();
		float rng = UnityEngine.Random.Range(0, total);
		foreach (var (side, prob) in probability)
		{
			rng -= prob;
			if (rng < 0)
			{
				return side;
			}
		}

		throw new System.Exception("This shouldn't happen");
	}
}
