/// <summary>
/// GameLogic Events
/// </summary>
public enum GameEvent
{
	/*
	Event Type											Parameters											Description
	-----------------------------------------------				---------------------------------				----------------------------------------------------
	*/
	
	StartStage,																								//Stage begin (or restart)
	EndStage,																								//The stage finish (player lose or win)

	AsteroidHittedByPlayer,								// 		-    										Asteroid has been hitted by player.
    AsteroidFallen,										// 		-    										Asteroid has touch the ground.    
}