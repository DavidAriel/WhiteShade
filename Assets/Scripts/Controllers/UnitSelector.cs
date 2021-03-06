﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GameController : MonoBehaviour
{
    public List<Unit> Selected = new List<Unit>();
    private bool _isSelecting;
    private Vector3 _selectionStart;

    // Use this for initialization
    private Rect SelectionBox
    {
        get
        {
            var point1 = Camera.main.ScreenToWorldPoint(_selectionStart);
            var point2 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var topLeft = Vector3.Min(point1, point2);
            var bottomRight = Vector3.Max(point1, point2);
            // Create Rect
            return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
        }   
    }

       
    public void SelectDown()
    {
        UnselectAllUnits();
        _isSelecting = true;
        _selectionStart = Input.mousePosition;
    }

    public void SelectUp()
    {
            Selected = SelectUnits(SelectionBox);
            _isSelecting = false;
    }
    // Update is called once per frame
    void UpdateUnitSelection()
    {
        if (_isSelecting)
        {
            SelectBoxHover(SelectionBox);
        }
    }


    private List<Unit> SelectUnits(Rect selectionBox)
    {
        var selectables = FindObjectsOfType<Unit>();
        return selectables.Where(x => x.IsInPlayingGroup && selectionBox.Contains(x.transform.position)).ToList();
    }

    private void SelectBoxHover(Rect selectionBox)
    {
        foreach (var selectableObject in FindObjectsOfType<Unit>())
        {
            if (selectionBox.Contains(selectableObject.transform.position))
            {
                //in box
                if (selectableObject.SelectionCircle != null)
                {
                    selectableObject.SelectionCircle.SetActive(true);
                }
            }
            else
            {
                //out of box
                if (selectableObject.SelectionCircle != null)
                {
                    selectableObject.SelectionCircle.SetActive(false);
                }
            }
        }
    }

    private void UnselectAllUnits()
    {
        Selected.ForEach((x) => x.SelectionCircle.SetActive(false));
        //Debug.Log("Selected clear" + selected.Count);
        Selected.Clear();
    }
    private void OnGUI()
    {
        if (_isSelecting)
        {
            var rect = Utils.GetScreenRect(_selectionStart, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}