using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Collider _collider;
    [SerializeField] private UnityEvent _allSpawnDeadEvent;
 
    private List<SpawnPosition> _spawnPositions;
    private List<Locomotion> _spawnList;
    private bool _hasSpawn;



    private void Awake()
    {
        var spawnPositionList = transform.parent.GetComponentsInChildren<SpawnPosition>();  
        _spawnPositions = new List<SpawnPosition>(spawnPositionList);
        _spawnList = new List<Locomotion>();
    }

    /*private void Update()
    {
        if(!_hasSpawn || _spawnList.Count == 0)
        {
            return;
        }

        // check: either all dead 
        bool isAllSpawnDead = true;
        foreach (var character in _spawnList)
        {
            if (character.currentState != Locomotion.State.Dead)
            {
                isAllSpawnDead = false;
                break;
            }
        }

        if (isAllSpawnDead)
        {
            _allSpawnDeadEvent?.Invoke();
        }
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnCharacters();
        }
    }

    public void SpawnCharacters()
    {
        if(_hasSpawn)
        {
            return;
        }

        _hasSpawn = true;

        foreach (var point in _spawnPositions)
        {
            if(point.enemyToSpawn != null)
            {
                 //Locomotion locomotion = /
                    Instantiate(point.enemyToSpawn, point.transform.position, point.transform.rotation);
               // _spawnList.Add(locomotion);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _collider.bounds.size);
    }
}
