using UnityEngine;
// базовый тип
namespace DefaultNamespace
{
    public abstract class Entity: MonoBehaviour
    {
        [SerializeField] protected CellType playWith;

        // метод скажет что у нас был сделан шаг, передает каким индексом и типом мы играли
        public System.Action<int, CellType> OnStep;

        // шаг который у них будет по-разному реализовываться
        public abstract void GetStep(params CellType[] field); // params передает или массив или ничего или 1 элемент оного
    }
}