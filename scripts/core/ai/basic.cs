//// structure ////
// %bot.RPGData = scriptobject with fields defining properties
// - maxDamage
// - armor
// - resist
// - level

// - searchType (ScriptObject)
// 	- searchFunction
// 	- searchRadius
// 	- etc...

// - attackType (ScriptObject)
// 	- attackImage
// 	- triggerTime
// 	- etc...

// - botLogicFunc
// - botActionFunc
// - damageCallbackFunc

// - expReward
















function MRPGBot_simpleLogic(%bot)
{
	if (%bot.nextThink > $Sim::Time)
	{
		return;
	}
	
	%data = %bot.RPGData;
	%searchData = %data.searchType;
	
	if (%bot.target.isDisabled())
	{
		%bot.target = "";
	}
	
	if (!isObject(%bot.target))
	{
		%val = call(%searchData.searchFunction, %bot);
		%closest = getClosestObjectToPoint(%val, %bot.getPosition());
		%bot.target = %closest;
	}
	%bot.nextThink = ($Sim::Time + 1 | 0) | 0;
}