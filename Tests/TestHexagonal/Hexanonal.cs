using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hexanonal : MonoBehaviour {

    [SerializeField]
    Text position;

    [SerializeField]
    Image image;

	// Use this for initialization
	void Start () {

        Button b = GetComponent<Button>();
        if (b != null)
        {
            b.onClick.AddListener(onClick);
        }
	}

    private HexagonalGrid _controller = null;
    public void setController(HexagonalGrid controller)
    {
        _controller = controller;
    }

    private void onClick()
    {
        _controller.onHexagonalClick(_position);
    }

    Vector2 _position = new Vector2();
	public void setPosition(Vector2 v)
    {
        _position.x = v.x;
        _position.y = v.y;

        position.text = v.x + ", " + v.y;
    }

    public void resetPosition()
    {
        position.text = _position.x + ", " + _position.y;
    }

    public Vector2 getPosition()
    {
        return _position;
    }

    public void setSelected(bool selected)
    {
        if (selected)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            image.color = new Color(1f, 1f, 1f);
        }
    }

    public void setText(string s)
    {
        position.text = s;
    }

    public void reset()
    {
        setSelected(false);
        resetPosition();
    }
}
