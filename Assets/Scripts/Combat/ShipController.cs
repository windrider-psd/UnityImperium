﻿using Imperium;
using Imperium.Enum;
using Imperium.Navigation;
using UnityEngine;

public class ShipController : ObjectController
{
    public FleetCommandQueue fleetCommandQueue = new FleetCommandQueue();
    public ShipType shipType;
    public StationConstructor stationConstructor;
    //private ShipMovement shipMovement;
    public Ship ship;

    public void AttackTarget(GameObject target, bool resetCommands, bool loopCommands)
    {
        if (!target.Equals(gameObject))
        {
            FleetCommand fleetCommand = new AttackCommand(gameObject, target);

            AddCommand(resetCommands, fleetCommand);

            fleetCommandQueue.loopFleetCommands = loopCommands;

            TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
            foreach (TurretController turret in turrets)
            {
                turret.SetFirePriority(target);
            }
        }
    }

    public void BuildStation(GameObject station, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new BuildCommand(gameObject, station);
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public void FireTurrets(GameObject target)
    {
        TurretController[] turrets = gameObject.GetComponentsInChildren<TurretController>(false);
        foreach (TurretController turret in turrets)
        {
            turret.Fire(target);
        }
    }

    public void MineAsteroid(GameObject asteroid, bool resetCommands)
    {
        FleetCommand fleetCommand = new MineCommand(gameObject, asteroid);
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = false;
    }

    public void MoveToPosition(Vector3 destination, float destinationOffset, bool resetCommands, bool loopCommands)
    {
        FleetCommand fleetCommand = new MoveCommand(gameObject, destination, destinationOffset);
        AddCommand(resetCommands, fleetCommand);
        fleetCommandQueue.loopFleetCommands = loopCommands;
    }

    public void SetIdle()
    {
        fleetCommandQueue.ResetCommands();
        fleetCommandQueue.loopFleetCommands = false;
    }

    private void AddCommand(bool resetCommands, FleetCommand fleetCommand)
    {
        Debug.Log("Added: " + fleetCommand);
        if (resetCommands)
        {
            fleetCommandQueue.ResetCommands();
            fleetCommandQueue.fleetCommands.Add(fleetCommand);
        }
        else
        {
            fleetCommandQueue.fleetCommands.Add(fleetCommand);
        }

        if (fleetCommandQueue.CurrentFleetCommand == null)
        {
            fleetCommandQueue.CurrentFleetCommand = fleetCommand;
        }
    }

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(gameObject.transform.position, ship.shipStats.FieldOfViewDistance);
        }
        catch
        {
        }
    }

    private void Start()
    {
        ship = ShipFactory.getInstance().CreateShip(shipType);
        stats = ship.shipStats;

       // shipMovement = new ShipMovement(gameObject.transform, 2f, 50f);

        lowestTurretRange = base.GetLowestTurretRange();

        stationConstructor = GetComponent<StationConstructor>();

        StartCoroutine(ShieldRegeneration());
    }

    private void Update()
    {
        if (fleetCommandQueue.fleetCommands.Count > 0)
        {
            FleetCommand fleetCommand = fleetCommandQueue.CurrentFleetCommand;

            if (!fleetCommand.IsFinished())
            {
                fleetCommand.ExecuteCommand();
            }
            else
            {
                FleetCommand next = fleetCommandQueue.SetNextFleetCommand();
                if (next == null)
                {
                    SetIdle();
                }
            }
        }

        FireAtClosestTarget();
    }


    public void MoveControl(Vector3 destination)
    {
        Quaternion desRotation = Quaternion.LookRotation(destination - transform.position, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desRotation, ship.angularSpeed * Time.deltaTime);
        transform.position += transform.forward * ship.speed * Time.deltaTime;
    }
}