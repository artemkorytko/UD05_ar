using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    private Cell[] _cells;

    public event System.Action<CellType> OnCompleted;

    public void Initialize()
    {
        _cells = GetComponentsInChildren<Cell>();
        for (int i = 0; i < _cells.Length; i++)
        {
            _cells[i].Initialize(i);
        }
    }

    public void SetState(bool isOn)
    {
        foreach (var cell in _cells)
        {
            cell.IsActive = isOn;
        }
    }

    public void Refresh()
    {
        foreach (var cell in _cells)
        {
            cell.Refresh();
        }
    }

    public void SetCell(int index, CellType type)
    {
        if (index < 0 || index >= _cells.Length)
            return;

        switch (type)
        {
            case CellType.Cross:
                _cells[index].SelectCross();
                break;
            case CellType.Zero:
                _cells[index].SelectZero();
                break;
        }

        Check();
    }

    private void Check()
    {
        if (CheckLines(out CellType winType))
        {
            OnCompleted?.Invoke(winType);
            return;
        }

        if (CheckCount())
        {
            OnCompleted?.Invoke(CellType.None);
        }
    }

    private bool CheckCount()
    {
        var unusedCount = 0;

        foreach (var cell in _cells)
        {
            if (cell.Type == CellType.None)
            {
                unusedCount++;
            }
        }

        if (unusedCount == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //TODO: need refactoring
    private bool CheckLines(out CellType winType)
    {
        winType = default;

        var cellType = CellType.None;

        //check rows
        for (int i = 0; i < _cells.Length; i += 3)
        {
            cellType = _cells[i].Type;
            var result = true;

            for (int j = i + 1; j < i + 3; j++)
            {
                if (cellType != _cells[j].Type)
                    result = false;
                break;
            }

            if (result && cellType != CellType.None)
            {
                winType = cellType;
                return true;
            }
        }

        //check colums
        for (int i = 0; i < 3; i++)
        {
            cellType = _cells[i].Type;
            var result = true;

            for (int j = i + 3; j < _cells.Length; j += 3)
            {
                if (cellType != _cells[j].Type)
                    result = false;
                break;
            }

            if (result && cellType != CellType.None)
            {
                winType = cellType;
                return true;
            }
        }

        //check diagonal 1
        {
            cellType = _cells[0].Type;
            var result = true;

            for (int j = 4; j < _cells.Length; j += 4)
            {
                if (cellType != _cells[j].Type)
                    result = false;
                break;
            }

            if (result && cellType != CellType.None)
            {
                winType = cellType;
                return true;
            }
        }
        
        //check diagonal 2
        {
            cellType = _cells[2].Type; 
            var result = true;

            //HACK
            for (int j = 4; j < _cells.Length - 1; j += 2)
            {
                if (cellType != _cells[j].Type)
                    result = false;
                break;
            }

            if (result && cellType != CellType.None)
            {
                winType = cellType;
                return true;
            }
        }

        return false;
    }

    public CellType[] FieldToArray()
    {
        var field = new CellType[9];
        for (int i = 0; i < _cells.Length; i++)
        {
            field[i] = _cells[i].Type;
        }

        return field;
    }
}