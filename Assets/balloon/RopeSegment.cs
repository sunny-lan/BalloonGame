using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{
    public GameObject connectedAbove, connectedBelow;
    public Rope oldParent;

    public float Height { get;private set; }

    void Start()
    {
        var hingejoint = GetComponent<HingeJoint2D>();

		connectedAbove = hingejoint.connectedBody?.gameObject;
        RopeSegment aboveSegment = connectedAbove?.GetComponent<RopeSegment>();
        Height = connectedAbove.GetComponent<SpriteRenderer>().bounds.size.y;

		if (aboveSegment != null)
        {
            aboveSegment.connectedBelow = gameObject;
            float spriteBottom = Height;
            hingejoint.connectedAnchor = new(0, spriteBottom * -1);
        }
        else
        {
            hingejoint.connectedAnchor = Vector2.zero;
        }
    }

}
