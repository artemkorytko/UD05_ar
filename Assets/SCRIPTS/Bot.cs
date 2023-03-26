using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Bot : Entity
{
    public override void GetStep(params CellType[] field)
    {
        StartCoroutine(ChooseStep(field));
    }
    
    
    // все поле на входе ожидается массивом
    private IEnumerator ChooseStep(CellType[] field)
    {
        // рандоно ждет - делает вид что думает
        yield return new WaitForSeconds(Random.Range(0.5f, 1f));

        // отфильтровать и взять массив свободных ячеек
        List<int> freeCells = new List<int>();
        for (int i = 0; i < field.Length; i++)
        {
            // если нет значения то там пусто
            if (field[i] == CellType.None)
            {
                freeCells.Add(i);
            }
        }
        
        // ходит просто рандомно
        // инвокает в GameController
        OnStep?.Invoke(freeCells[Random.Range(0, freeCells.Count)], playWith);
    }
    
    // передать массив свободных ячеек
    
    
    
}
