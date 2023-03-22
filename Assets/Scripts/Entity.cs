using UnityEngine;

namespace AR
{
    public abstract class Entity : MonoBehaviour
    {
        [SerializeField] protected CellType playWith;

        public System.Action<int, CellType> OnStep;

        public abstract void GetStep(params CellType[] field); 

    }
}