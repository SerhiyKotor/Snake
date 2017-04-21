using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnObjects : MonoBehaviour {

    
    public Text AppleCounter;
    int _findApple = 0;

    bool _buttonIsPressed;
    bool _gameStarted;

    float _startTime;
    float _timeSinceStartTime;

    public GameObject SpawnPrefab;
    public GameObject Myparent;

    public int TylesX;
    public int TylesY;
    public int SnakeSize = 5;

    int _axisX;
    int _axisY=-1;
    
    GameObject[,] _allTilesObjects;

    List<GameObject> _snakeList = new List<GameObject>();
    List<Vector2> _snakeCoordinats = new List<Vector2>();
    List<Vector2> _snakeCoordinatsOld = new List<Vector2>();

    List<GameObject> _objectsSpawned = new List<GameObject>();
    List<Vector2> _objectsSpawnedCoordinats = new List<Vector2>();

    float _updateDelay;
    public float DefaultDelay = 0.25f;

    bool _updateDelayIsNotDefault;
    public float TimeOFSpeadChanging =10f;
       
    public GameObject Panel;
           
    void UpdateInterval()
    {
        _buttonIsPressed = false;

        for (int i = 0; i < _snakeList.Count; i++)
        {
            _snakeList[i].GetComponent<EnumScript>().SetState(EnumScript.ObjectState.Frame);         
        }

        _snakeList.Clear();

        _snakeCoordinatsOld = new List<Vector2>(_snakeCoordinats);

        for (int i = 0; i < _snakeCoordinats.Count; i++)
        {          
            if (i == 0)
            {
                _snakeCoordinats[i] = new Vector2(_snakeCoordinatsOld[i].x + _axisX, _snakeCoordinatsOld[i].y + _axisY);

                // spawn on new side
                if (_snakeCoordinats[i].x == TylesX)
                {                
                    _snakeCoordinats[i] = new Vector2(0, _snakeCoordinats[i].y);
                }
                if (_snakeCoordinats[i].x == -1)
                {                   
                    _snakeCoordinats[i] = new Vector2(TylesX-1, _snakeCoordinats[i].y);
                }
                if (_snakeCoordinats[i].y == TylesY)
                {
                    _snakeCoordinats[i] = new Vector2(_snakeCoordinats[i].x, 0);
                }
                if (_snakeCoordinats[i].y == -1)
                {
                    _snakeCoordinats[i] = new Vector2(_snakeCoordinats[i].x, TylesY - 1);
                }
            }
            else
            {
                _snakeCoordinats[i] = new Vector2(_snakeCoordinatsOld[i-1].x, _snakeCoordinatsOld[i-1].y);
            }                  
        }

        for (int i = 0; i < _snakeCoordinats.Count-1; i++)
        {         
            _snakeList.Add(_allTilesObjects[(int)_snakeCoordinats[i].x, (int)_snakeCoordinats[i].y]);
        }


        // find Apple
        if (_allTilesObjects[(int)_snakeCoordinats[0].x, (int)_snakeCoordinats[0].y].GetComponent<EnumScript>().ReturnState() == EnumScript.ObjectState.Apple)
        {        
            _snakeList.Add(_allTilesObjects[(int)_snakeCoordinats[_snakeCoordinats.Count-1].x, (int)_snakeCoordinats[_snakeCoordinats.Count-1].y]);
            _snakeCoordinats.Add(new Vector2(_snakeCoordinats[_snakeCoordinats.Count - 1].x+1, _snakeCoordinats[_snakeCoordinats.Count - 1].y+1));

            _findApple++;
            AppleCounter.text = "Find apples: " + _findApple.ToString();

            SpawnObjectState(EnumScript.ObjectState.Apple);
            
        }

        // find Fast
        if (_allTilesObjects[(int)_snakeCoordinats[0].x, (int)_snakeCoordinats[0].y].GetComponent<EnumScript>().ReturnState() == EnumScript.ObjectState.Fast)
        {            
            _updateDelay = 0.125f;
            _updateDelayIsNotDefault = true;
            
        }

        // find Slow
        if (_allTilesObjects[(int)_snakeCoordinats[0].x, (int)_snakeCoordinats[0].y].GetComponent<EnumScript>().ReturnState() == EnumScript.ObjectState.Slow)
        {
            _updateDelay = 0.5f;
            _updateDelayIsNotDefault = true;
            
        }

        // find Body
        for (int i = 2; i < _snakeCoordinats.Count - 1; i++)
        {
            if ((_snakeCoordinats[i].x == _snakeCoordinats[0].x) && (_snakeCoordinats[i].y == _snakeCoordinats[0].y))
            {
                StartNewGame();
                break;
            }
        }

        // find Swap
        if (_allTilesObjects[(int)_snakeCoordinats[0].x, (int)_snakeCoordinats[0].y].GetComponent<EnumScript>().ReturnState() == EnumScript.ObjectState.Swap)
        {
            _allTilesObjects[(int)_snakeCoordinats[0].x, (int)_snakeCoordinats[0].y].GetComponent<EnumScript>().SetState(EnumScript.ObjectState.Frame);
                 
            _snakeCoordinats.Reverse();

            _axisY = ((int)_snakeCoordinats[0].y - (int)_snakeCoordinats[1].y);
            _axisX = ((int)_snakeCoordinats[0].x - (int)_snakeCoordinats[1].x);                               
        }

        // draw snake
        for (int i = 0; i < _snakeList.Count; i++)
        {
            if (i == 0)
            {
                _snakeList[i].GetComponent<EnumScript>().SetState(EnumScript.ObjectState.Head);            
            }
            else
            {
                _snakeList[i].GetComponent<EnumScript>().SetState(EnumScript.ObjectState.Body);           
            }
        }       
    }

    void SpawnObjectState(EnumScript.ObjectState state)
    {
        int _x = Random.Range(0, TylesX);
        int _y = Random.Range(0, TylesY);
        bool doAgain= false;
       
        for (int i = 0; i < _snakeCoordinats.Count; i++)
        {
            if((_snakeCoordinats[i].x== _x)&&(_snakeCoordinats[i].y == _y))
            {
                doAgain = true;
                Debug.Log("Spawn Again");
                break;
            }           
        }
        for (int i = 0; i < _objectsSpawnedCoordinats.Count; i++)
        {
            if ((_objectsSpawnedCoordinats[i].x == _x) && (_objectsSpawnedCoordinats[i].y == _y))
            {
                doAgain = true;
                Debug.Log("Spawn Again not in block");
                break;
            }
        }

        if (doAgain == true)
        {
            SpawnObjectState(state);
        }
        else
        {
            _allTilesObjects[_x, _y].GetComponent<EnumScript>().SetState(state);

            _objectsSpawned.Add(_allTilesObjects[_x, _y]);
            _objectsSpawnedCoordinats.Add(new Vector2(_x, _y));
            
        }
              
    }
   
    IEnumerator waitFunction()
    {
        while (_gameStarted == true)
        {
            yield return new WaitForSeconds(_updateDelay);
          
            UpdateInterval();
        }              
    }

    void Start()
    {            
        _gameStarted = true;
        StartCoroutine(waitFunction());
                                        
        _allTilesObjects = new GameObject[TylesX, TylesY];
       
        RectTransform parent = gameObject.GetComponent<RectTransform>();
        GridLayoutGroup grid = gameObject.GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(parent.rect.width / TylesX, parent.rect.height / TylesY);

        for (int j = 0; j < TylesY; j++)
        {
            for (int i = 0; i < TylesX; i++)
            {           
                _allTilesObjects[i, j] = (GameObject)Instantiate(SpawnPrefab);
                _allTilesObjects[i, j].transform.parent = Myparent.transform;               
            }
        }
        
        StartNewGame();                         
    }


    void StartNewGame()
    {
        _buttonIsPressed = false;

        _findApple = 0;
        AppleCounter.text = "Find apples: " + _findApple.ToString();

        _axisY = -1;
        _axisX = 0;
       
        foreach(GameObject obj in _objectsSpawned)
        {
            obj.GetComponent<EnumScript>().SetState(EnumScript.ObjectState.Frame);
        }
        _objectsSpawned.Clear();
        _objectsSpawnedCoordinats.Clear();

        _snakeList.Clear();
        _snakeCoordinats.Clear();


        // Snake size on start
        if(SnakeSize> TylesY/2-1)
        {
            SnakeSize = TylesY/2-1;
        }
        for (int i =0; i<= SnakeSize; i++)
        {
            _snakeList.Add(_allTilesObjects[TylesX / 2, TylesY / 2 + i]);
            _snakeCoordinats.Add(new Vector2(TylesX / 2, TylesY / 2 + i));
        }
        
        _updateDelay = DefaultDelay;      
        SpawnObjectState(EnumScript.ObjectState.Apple);
    }

    
    void FixedUpdate()
    {
                
        int _superRandom = Random.Range(0, 5000);
        if (_superRandom < 5)
        {       
            SpawnObjectState(EnumScript.ObjectState.Fast);
        }
        if ((_superRandom < 10) && (_superRandom >= 5))
        {    
            SpawnObjectState(EnumScript.ObjectState.Slow);
        }
        if ((_superRandom < 15) && (_superRandom >= 10))
        {
            SpawnObjectState(EnumScript.ObjectState.Swap);
        }

        // timer to return in normal speed
        if (_updateDelayIsNotDefault == true)
        {
            _startTime = Time.time;
            _updateDelayIsNotDefault = false;
        }
        _timeSinceStartTime = Time.time - _startTime;
        if (_timeSinceStartTime > TimeOFSpeadChanging)
        {
            _updateDelay = DefaultDelay;
        }
        
        

    }
   
    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.W)) && (_axisY != 1) && (_buttonIsPressed == false))
        {
            _axisY = -1;
            _axisX = 0;
            _buttonIsPressed = true;
        }
        if ((Input.GetKeyDown(KeyCode.S)) && (_axisY != -1) && (_buttonIsPressed == false))
        {
            _axisY = 1;
            _axisX = 0;
            _buttonIsPressed = true;

        }
        if ((Input.GetKeyDown(KeyCode.D)) && (_axisX != -1) && (_buttonIsPressed == false))
        {
            _axisY = 0;
            _axisX = 1;
            _buttonIsPressed = true;

        }
        if ((Input.GetKeyDown(KeyCode.A)) && (_axisX != 1) && (_buttonIsPressed == false))
        {
            _axisY = 0;
            _axisX = -1;
            _buttonIsPressed = true;
        }
        
    }

}



