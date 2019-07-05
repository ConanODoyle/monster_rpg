function getPlayerFromObject(%obj)
{
	if (%obj.getClassName() $= "Player")
	{
		return %obj;
	}
	else if (%obj.getClassName() $= "GameConnection")
	{
		return %obj.player;
	}
	else if (isObject(%obj.sourceObject))
	{
		if (%obj.sourceObject.getClassName() $= "Player")
		{
			return %obj.sourceObject;
		}
		else if (%obj.sourceObject.getClassName() $= "GameConnection")
		{
			return %obj.sourceObject.player;
		}
		else if (%obj.sourceObject.getClassName() $= "fxDTSBrick")
		{
			return %obj.sourceObject.getGroup().client.player;
		}
		else if (isFunction(%obj.sourceObject.getClassName(), "getControllingClient"))
		{
			return %obj.sourceObject.getControllingClient().player;
		}
		else
		{
			return 0;
		}
	}
	else if (%obj.getClassName() $= "fxDTSBrick")
	{
		return %obj.getGroup().client.player;
	}
	else if (isFunction(%obj.getClassName(), "getControllingClient"))
	{
		return %obj.getControllingClient().player;
	}

	return 0;
}