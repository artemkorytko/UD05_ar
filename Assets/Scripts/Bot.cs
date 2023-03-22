using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AR
{
    public class Bot : Entity
    {
        public override void GetStep(params CellType[] field)
        {
            StartCoroutine(ChooseStep(field));
        }

        private IEnumerator ChooseStep(CellType[] field)
        {
            yield return new WaitForSeconds(Random.Range(0.5f, 1f));
            
             List<int> freeCells = new List<int>();
             for (int i = 0; i < field.Length; i++)
             {
                 if(field[i] == CellType.None)
                     freeCells.Add(i);
             }
             
            OnStep?.Invoke(freeCells[Random.Range(0, freeCells.Count)], playWith);
        }
    }
}