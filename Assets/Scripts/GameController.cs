using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private Field fieldPrefab;
    [SerializeField] private ARPlaneManager arPlaneManager; 
    [SerializeField] private ARRaycastManager arRaycastManager;
    [SerializeField]  private ARAnchorManager arAnchorManager;
    
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
        
        _fieldPositionController.Initialize(arPlaneManager, arRaycastManager, arAnchorManager);
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
        Entity currEntity;

        while (true)
        {
            currEntity = _player;
            currEntity.OnStep += OnEntityStep;
            _isEntityMakeStep = false;
            currEntity.GetStep();

            yield return new WaitWhile(() => _isEntityMakeStep == false);
            currEntity.OnStep -= OnEntityStep;

            currEntity = _bot;
            currEntity.OnStep += OnEntityStep;
            _isEntityMakeStep = false;
            currEntity.GetStep();  //TODO: get field

            yield return new WaitUntil(() => !_isEntityMakeStep);
            currEntity.OnStep -= OnEntityStep;
        }
    }

    private void OnEntityStep(int index, CellType type)
    {
        _isEntityMakeStep = true;
        //TODO : add field step
    }
}
