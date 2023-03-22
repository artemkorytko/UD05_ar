using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    None,
    Cross,
    Zero
}

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject cross;
    [SerializeField] private GameObject zero;

    public int Index { get; private set; }
    public bool IsActive { get; set; } = true;
    public CellType Type { get; private set; }

    public void Initialize(int index)
    {
        Index = index;
        
        Debug.Log(Index);
    }

    public void SelectCross()
    {
        if (!IsActive) return;

        cross.SetActive(true);
        zero.SetActive(false);
        IsActive = false;
        Type = CellType.Cross;
    }

    public void SelectZero()
    {
        if (!IsActive) return;

        cross.SetActive(false);
        zero.SetActive(true);
        IsActive = false;
        Type = CellType.Zero;
    }

    public void Refresh()
    {
        cross.SetActive(false);
        zero.SetActive(false);
        IsActive = true;
        Type = CellType.None;
    }
}