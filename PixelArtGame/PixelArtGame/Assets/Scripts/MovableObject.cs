﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovableObject : MonoBehaviour {

	public SpriteRenderer objectSpriteRenderer;
	
	virtual public void Update () {
		objectSpriteRenderer.sortingOrder = (int)((transform.position.y) * GameManager.instance.sortingOrderPrecision);
	}
}
