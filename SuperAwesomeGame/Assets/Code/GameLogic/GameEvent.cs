using System;

/// <summary>
/// GameLogic Events
/// </summary>
public enum GameEvent
{
	/*
	Event Type											Parameters											Description
	-----------------------------------------------				---------------------------------				----------------------------------------------------
	*/
	
	BeginStage,
	EndStage,

	AsteroidHittedByPlayer,								// 		-    										Asteroid has been hitted by player.
    AsteroidFallen,										// 		-    										Asteroid has touch the ground.    
}