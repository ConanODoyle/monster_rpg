if (!isObject($RPGData_DemonicEye))
{
	$RPGData_DemonicEye = new ScriptObject(RPGDataObject) {};
	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_DemonicEye);");
}

if (!isObject($RPGData_DemonicEyeAttack))
{
	$RPGData_DemonicEyeRangedAttack = new ScriptObject(DemonicEyeRangedAttackType) {
		attackImage = "GoblinBowImage";
		triggerTime = "1 1";
		minRange = 0;
		maxRange = 100;
		stopRange = 1000;
		backingAttack = 1;
	};

	schedule(1000, 0, eval, "MissionCleanup.add($RPGData_DemonicEyeRangedAttack);");
}

datablock PlayerData(EyePlayer : PlayerNoJet)
{
	drag = 8;
};

$RPGData_DemonicEye.name = "DemonicEye";
$RPGData_DemonicEye.maxDamage = 80;
$RPGData_DemonicEye.armor = 0;
$RPGData_DemonicEye.resist = 0;
$RPGData_DemonicEye.level = 1;
$RPGData_DemonicEye.thinkTime = 1;
$RPGData_DemonicEye.resetTargetTime = 10;

$RPGData_DemonicEye.searchType = "NearRadiusLOSSearchType";

$RPGData_DemonicEye.attackType = "DemonicEyeRangedAttackType";

// $RPGData_DemonicEye.wanderSpeed = 10;
// $RPGData_DemonicEye.leashDistance = 10;
$RPGData_DemonicEye.autoCutoffRange = 40;

$RPGData_DemonicEye.botLogicFunc = "MRPGBot_simpleLogic";
$RPGData_DemonicEye.botActionFunc = "MRPGBot_simpleAction";
$RPGData_DemonicEye.damageCallbackFunc = "MRPGBot_simpleDamageCallback";

$RPGData_DemonicEye.expReward = "4 6";
$RPGData_DemonicEye.goldReward = "1 2";

$RPGData_DemonicEye.maxForwardSpeed = 4.5;
$RPGData_DemonicEye.maxBackwardSpeed = 2.8;
$RPGData_DemonicEye.maxSideSpeed = 2.8;
$RPGData_DemonicEye.maxYawSpeed = 10;
$RPGData_DemonicEye.maxPitchSpeed = 10;
$RPGData_DemonicEye.scale = "0.8 0.8 0.8";
$RPGData_DemonicEye.botDB = "PlayerStandardArmor";
$RPGData_DemonicEye.canJump = 1;


function MRPGBot_DemonicEyeLogic(%bot)
{
	if (!isObject(%bot.target) && %bot.nextRandomLook < $Sim::Time)
	{
		%doRandomLook = 1;
	}
	MRPGBot_simpleLogic(%bot);
	if (%doRandomLook)
	{
		%rand = getRandom(1, 360);
		%vec = vectorNormalize(mSin(%rand) SPC mCos(%rand) SPC 0);
		%rand = getRandom() * 2 - 1;
		%vec = vectorNormalize(getWords(vectorScale(%vec, 1 - mAbs(%rand)), 0, 1) SPC %rand);
		%bot.setAimVector(%vec);
		%bot.nextRandomLook = ($Sim::Time + (getRandom(3, 8) | 0)) | 0;
	}
}

function MRPGBot_DemonicEyeAction(%bot)
{
	if (%bot.nextAction > $Sim::Time)
	{
		return;
	}
	else if (%bot.isDisabled())
	{
		clearTriggers(%bot);
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
		%bot.setJumping(0);
		if (isObject(%bot.getAimObject()))
		{
			%bot.setAimObject("");
		}
		clearTriggers(%bot);
	}
	else
	{
		if (%bot.armAnim !$= "armReadyRight")
		{
			%bot.playThread(1, armReadyRight);
			%bot.armAnim = "armReadyRight";
		}
		%bot.setAimObject(%bot.target);

		%zDiff = getWord(%bot.target.position, 2) - getWord(%bot.position, 2);
		if (%bot.canJump && %zDiff > 1.2 && getRandom() < 0.08 * %zDiff)
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
				if (%bot.nextBump < $Sim::Time) //force aiming visual update for immobile bots
				{
					%bot.addVelocity("0 0 0.1");
					%bot.nextBump = $Sim::Time + 0.2;
				}
				%bot.setImageTrigger(0, 1);
			}
			else
			{
				if (%bot.nextBump < $Sim::Time) //force aiming visual update for immobile bots
				{
					%bot.addVelocity("0 0 0.1");
					%bot.nextBump = $Sim::Time + 0.2;
				}
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
				
			if (%bot.nextBump < $Sim::Time) //force aiming visual update for immobile bots
			{
				%bot.addVelocity("0 0 0.1");
				%bot.nextBump = $Sim::Time + 0.2;
			}
			%bot.setImageTrigger(0, 1);
		}
	}
	MRPGBot_DemonicEyeMovement(%bot); //use a separate forced schedule to handle movement

	%bot.nextAction = 0; //always update action every tick
}

function MRPGBot_DemonicEyeMovement(%bot)
{
	cancel(%bot.movementSchedule);

	if (%bot.isDisabled())
	{
		return;
	}

	%targetLocation = %bot.targetLocation;
	%position = %bot.getPosition();
	%moveSpeed = %bot.moveSpeed ? %bot.moveSpeed : 3;

	%vec = vectorSub(%targetLocation, %position);
	if (vectorLen(%vec < 1))
	{
		%moveSpeed = 3;
	}
	%vec = vectorScale(vectorNormalize(%vec), %moveSpeed);
	
	%bot.setVelocity(%vec);

	%bot.movementSchedule = schedule(1, %bot, MRPGBot_DemonicEyeMovement, %bot);
}