using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameController : MonoBehaviour
{
    [SerializeField] private Field fieldPrefab;
    [SerializeField] private ARPlaneManager arPlaneManager; //определит поверхность
    [SerializeField] private ARRaycastManager arRaycastManager; //бросит рейкаст на поверхность
    [SerializeField] private ARAnchorManager arAnchorManager; //привяжен объект к точке рейкаста

    private Player _player;
    private Bot _bot;

    private Coroutine _playCoroutine;
    private bool _isEntityMakeStep;

    private Field _field;
    private FieldPositionController _fieldPositionController;
    private FiledSetup _fieldSetup; 

    private void Awake()
    {
        _player = GetComponent<Player>();
        _bot = GetComponent<Bot>();
    }

    private void Start()
    {
        _field = Instantiate(fieldPrefab);

        _fieldPositionController = _field.GetComponent<FieldPositionController>();
        _fieldSetup = _field.GetComponent<FiledSetup>();

        // передали ему всех
        _fieldPositionController.Initialize( arRaycastManager, arPlaneManager, arAnchorManager);
        _fieldPositionController.IsActive = true;
    }

    public void Play()
    {
        // уже поставили и настроили и можно заускать игру
        _fieldPositionController.IsActive = false;
        _playCoroutine = StartCoroutine(GameCoroutine());
    }

    // когда победа - кто победил
    // а зачем в скобюочках???
    private void OnCompleted(CellType winCellType)
    {
        StopCoroutine(_playCoroutine); 
    }

    private IEnumerator GameCoroutine()
    {
        Entity currenEntity;

        while (true)
        {
            currenEntity = _player;
            currenEntity.OnStep += OnEntityStep; // событие в боте
            _isEntityMakeStep = false; // флаг
            currenEntity.GetStep();

            // как только условие перестане быть тру то выходим из елда
            yield return new WaitWhile(() => _isEntityMakeStep == false);
            currenEntity.OnStep -= OnEntityStep;

            // переход хода к боту
            currenEntity = _bot;
            currenEntity.OnStep += OnEntityStep;
            _isEntityMakeStep = false;
            currenEntity.GetStep(); // передать поле TODO: get field 

            // ждем пока флаг не поменяется на тру!!!!! - прикольно
            yield return new WaitUntil(() => _isEntityMakeStep);
            currenEntity.OnStep -= OnEntityStep;
        }
    }

    private void OnEntityStep(int index, CellType type)
    {
        _isEntityMakeStep = true;
        //TODO: add field step
    }
}