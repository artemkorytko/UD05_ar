using System.Collections;
using UnityEngine;

namespace AR
{
    public class Player : Entity    
    {
        [SerializeField] private Camera arCamera;
        
        public override void GetStep(params CellType[] field)
        {
            StartCoroutine(ChooseStep());
        }

        private IEnumerator ChooseStep()
        {
            while (true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var ray = arCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit))
                    {
                        var cell = hit.collider.GetComponent<Cell>();
                        if (cell && cell.IsActive)
                        {
                            OnStep?.Invoke(cell.Index, playWith);
                            break;
                        }
                    }
                }
            }

            yield return null;
        }
    }
}