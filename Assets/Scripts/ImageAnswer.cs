﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ImageAnswer : MonoBehaviour {
    public LineRenderer leftLine, topLine, rightLine, bottomLine;
    public Rect rect;

    public void OnMouseDown() {
        Destroy(leftLine.gameObject);
        Destroy(topLine.gameObject);
        Destroy(rightLine.gameObject);
        Destroy(bottomLine.gameObject);

        CropSprite.RemoveAnswer(this);
        Destroy(gameObject);
    }
}
