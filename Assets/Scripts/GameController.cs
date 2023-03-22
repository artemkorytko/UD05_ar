using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace AR
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Field fieldPrefab;
        [SerializeField] private ARRaycastManager arRaycastManager; // бросит луч
        [SerializeField] private ARPlaneManager arPlaneManager; // определит поверхность
        [SerializeField] private ARAnchorManager arAnchorManager; // привяжет объект к точке рейкаст
        
        private Player _player;
        private Bot _bot;
        
        private Coroutine _playCoroutine;
        private bool _isEntityMakeStep;

        private Field _field;
        private FieldPositionController _fieldPositionController;
        private FieldSetup _fieldSetup;
        
        private void Awake()
        {
            _player = GetComponent<Player>();
            _bot = GetComponent<Bot>();
        }

        private void Start()
        {
            _field = Instantiate(fieldPrefab);

            _fieldPositionController = _field.GetComponent<FieldPositionController>();
            _fieldSetup = _field.GetComponent<FieldSetup>();
            
            _fieldPositionController.Initialize(arAnchorManager, arRaycastManager, arPlaneManager);
            _fieldPositionController.IsActive = true;
        }


        public void Play()
        {
            _fieldPositionController.IsActive = false;
            _playCoroutine = StartCoroutine(GameCoroutine());
        }

        private void OnCompleted(CellType winCellType)
        {
            StopCoroutine(_playCoroutine);
        }

        private IEnumerator GameCoroutine()
        {
            Entity currentEntity;
            while (true)
            {
                currentEntity = _player;
                currentEntity.OnStep += OnEntityStep;
                _isEntityMakeStep = false;
                currentEntity.GetStep();
                
                yield return new WaitWhile(()=> _isEntityMakeStep == false);
                currentEntity.OnStep -= OnEntityStep;
                
                currentEntity = _bot;
                currentEntity.OnStep += OnEntityStep;
                _isEntityMakeStep = false;
                currentEntity.GetStep(); // todo get field
                
                yield return new WaitUntil(()=> _isEntityMakeStep);
                currentEntity.OnStep -= OnEntityStep;
            }
        }

        private void OnEntityStep(int index, CellType type)
        {
            _isEntityMakeStep = true;
            // TODO: add field step
            
        }
    }
}