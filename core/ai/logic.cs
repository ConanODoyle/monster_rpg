//// structure ////
// %bot.RPGData = scriptobject with fields defining properties
// - attackType (ScriptObject)
// 	- attackImage
// 	- triggerTime
// 	- minRange
// 	- maxRange
// 	- stopRange

// - botLogicFunc
// - damageCallbackFunc

// - botActionFunc


if (!isObject(MRPG_AISet))
{
	$MRPG_AISet = new SimSet(MRPG_AISet) {};
	schedule(1000, 0, eval, "MissionCleanup.add($MRPG_AISet);");
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
	
	if (isObject(%bot.target) && %bot.target.isDisabled())
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
	else if (isObject(%bot.target))
	{
		if (isFunction(%searchData.searchFunction))
		{
			%val = call(%searchData.searchFunction, %bot);
			%closest = getClosestObjectToPoint(%val, %bot.getPosition());
			if (%bot.target != %closest) //closest target (if existing) is not the current target
			{
				if (!%bot.isLosingCurrentTarget) //start timer for losing target, if not currently timing it
				{
					%bot.targetLossTime = $Sim::Time;
					%bot.isLosingCurrentTarget = 1;
				}

				if ($Sim::Time - %bot.targetLossTime > %data.resetTargetTime) //reset target
				{
					%bot.target = %closest;
					%bot.isLosingCurrentTarget = 0;
				}
			}
			else //closest target is current target, reset timer for losing target
			{
				%bot.isLosingCurrentTarget = 0;
				%bot.targetLossTime = "";
			}
		}
	}
	%bot.nextThink = ($Sim::Time + (%data.thinkTime >= 1 ? %data.thinkTime : 1) | 0) | 0;
}

function MRPGBot_simpleAction(%bot)
{
	if (%bot.nextAction > $Sim::Time)
	{
		return;
	}
	
	%data = %bot.RPGData;
	
	if (!isObject(%bot.target))
	{
		if (%bot.armAnim !$= "root")
		{
			%bot.playThread(1, root);
			%bot.armAnim = "root";
		}
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
		if (%bot.armAnim !$= "armReadyRight")
		{
			%bot.playThread(1, armReadyRight);
			%bot.armAnim = "armReadyRight";
		}
		%bot.setAimObject(%bot.target);

		if (getWord(%bot.target.position, 2) > getWord(%bot.position, 2) + 1.5 && getRandom() < 0.01)
		{
			%bot.setJumping(1);
		}
		else
		{
			%bot.setJumping(0);
		}

		%attackData = %data.attackType;
		%maxRange = %attackData.maxRange;
		%minRange = %attackData.minRange;

		%dist = vectorDist(%bot.getPosition(), %bot.target.getPosition());
		if (%dist < %minRange)
		{
			%bot.setMoveY(-1);
			%bot.setMoveX(0);
			if (%attackData.backingAttack)
			{
				%bot.setImageTrigger(0, 1);
			}
			else
			{
				%bot.setImageTrigger(0, 0);
			}
		}
		else if (%dist > %maxRange)
		{
			%bot.setMoveY(1);
			%bot.setMoveX(0);
			%bot.setImageTrigger(0, 0);
		}
		else
		{
			if (%dist < %attackData.stopRange)
			{
				%bot.setMoveY(0);
			}
			else
			{
				%bot.setMoveY(0.5);
			}
			%bot.setMoveX(0);
			%bot.setImageTrigger(0, 1);
		}
	}

	%bot.nextAction = 0; //always update action every tick
}

// function MRPGBot_simpleDamageCallback(%bot, %atkObj, %pos, %damage, %damageType)
// {
// 	if (%damage <= 0)
// 	{
// 		return;
// 	}

// 	%attacker = getPlayerFromObject(%atkObj);

// 	%data = %bot.RPGData;
// 	%searchData = %data.searchType;

// 	if (!isObject(%bot.target) && isObject(%attacker))
// 	{
// 		%radius = %searchData.searchRadius;
// 		if (vectorDist(%attacker))
// 	}
// }