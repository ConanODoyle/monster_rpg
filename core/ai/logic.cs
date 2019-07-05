//// structure ////
// %bot.RPGData = scriptobject with fields defining properties
// - attackType (ScriptObject)
// 	- attackImage
// 	- triggerTime
// 	- minRange
// 	- maxRange

// - botLogicFunc
// - damageCallbackFunc

// - botActionFunc


if (!isObject(MRPG_AISet))
{
	$MRPG_AISet = new SimSet(MRPG_AISet) {};
	MissionCleanup.add($MRPG_AISet);
}


function logicTick(%idx)
{
	cancel($masterLogicSchedule);
	if (!isObject(MRPG_AISet))
	{
		return;
	}

	%count = MRPG_AISet.getCount();

	if (%idx >= %count)
	{
		%idx = 0;
	}

	for (%i = 0; %i < 16; %i++)
	{
		if (%idx >= %count)
		{
			break;
		}
		%curr = MRPG_AISet.getObject(%idx);

		%data = %curr.RPGData;
		if (isFunction(%data.botLogicFunc))		call(%data.botLogicFunc, %curr);
		else									call(MRPGBot_simpleLogic, %curr);
		

		if (isFunction(%data.botActionFunc))	call(%data.botActionFunc, %curr);
		else									call(MRPGBot_simpleAction, %curr);
		

		%idx++;
	}

	$masterLogicSchedule = schedule(1, 0, logicTick, %idx);
}

schedule(1000, 0, logicTick, 0);


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
		if (isFunction(%searchData.searchFunction))
		{
			%val = call(%searchData.searchFunction, %bot);
			%closest = getClosestObjectToPoint(%val, %bot.getPosition());
			%bot.target = %closest;
		}

		if (!isObject(%bot.target) && %bot.nextRandomLook < $Sim::Time)
		{
			%rand = getRandom(1, 360);
			%vec = mSin(%rand) SPC mCos(%rand) SPC 0;
			%bot.setAimVector(%vec);
			%bot.nextRandomLook = ($Sim::Time + (getRandom(3, 8) | 0)) | 0;
		}
	}
	%bot.nextThink = ($Sim::Time + 1 | 0) | 0;
}

function MRPGBot_simpleAction(%bot)
{
	if (%bot.nextAction > $Sim::Time)
	{
		return;
	}
	
	%data = %bot.RPGData;
	%searchData = %data.searchType;
	
	if (!isObject(%bot.target))
	{
		%bot.target = "";
		%bot.setMoveX(0);
		%bot.setMoveY(0);
		if (isObject(%bot.getAimObject()))
		{
			%bot.setAimObject("");
		}
	}
	else
	{
		%bot.setAimObject(%bot.target);

		%attackType = %data.attackType;
		%maxRange = %attackType.maxRange;
		%minRange = %attackType.minRange;

		%dist = vectorDist(%bot.getPosition(), %bot.target.getPosition());
		if (%dist < %minRange)
		{
			%bot.setMoveY(-1);
			%bot.setMoveX(0);
			%bot.setImageTrigger(0, 0);
		}
		else if (%dist > %maxRange)
		{
			%bot.setMoveY(1);
			%bot.setMoveX(0);
			%bot.setImageTrigger(0, 0);
		}
		else
		{
			%bot.setMoveY(0);
			%bot.setMoveX(0);
			%bot.setImageTrigger(0, 1);
		}
	}

	%bot.nextAction = 0; //always update action every tick
}