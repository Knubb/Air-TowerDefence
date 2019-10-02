﻿using System.Collections;
using UnityEngine;

public class WavesManager : MonoBehaviour
{
    CreepFactory _CreepFactory;
    GameObject _Spawnpoint;

    public WavesContainer WavesContainer;

    public int WaveIndex { get; private set; } = 0;

    private void Start()
    {
        _CreepFactory = new CreepFactory();
        _Spawnpoint = GameObject.FindGameObjectWithTag("Spawnpoint");

        StartNextWave();
    }

    public void StartNextWave()
    {
        StartCoroutine(StartWave(WavesContainer.Waves[WaveIndex]));
        WaveIndex++;
    }

    IEnumerator StartWave(Wave wave)
    {
        yield return new WaitForSecondsRealtime(1f);

        foreach (var spawn in wave.Spawns)
        {
            SpawnCreeps(spawn);

            yield return new WaitForSecondsRealtime(spawn.TimeToNext);
        }
    }


    private void SpawnCreeps(Spawn spawn)
    {
        foreach (var datapoint in spawn.SpawnData)
        {
            var creep = Instantiate(_CreepFactory.GetCreepPrefab(datapoint.CreepIdentifier));
            creep.transform.position = _Spawnpoint.transform.TransformPoint(datapoint.RelativePosition);
        }
    }
}