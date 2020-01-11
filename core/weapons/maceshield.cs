datablock ProjectileData(MaceAndShieldProjectile : swordProjectile)
{
	uiName = "";
	directDamage = 15;
	penetration = 50;
	type = "Physical";
};

datablock ItemData(MaceAndShieldItem : hammerItem)
{
	shapeFile = "./assets/maceshield/maceshielditemvert.dts";
	uiName = "Mace & Shield";
	iconName = "";

	image = MaceAndShieldImageA;
	colorShiftColor = "1 1 1 1";
};

datablock ShapeBaseImageData(MaceAndShieldImageA)
{
	shapeFile = "./assets/maceshield/maceshield.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = "0.75 1.2 -0.5"; //"0.7 1.2 -0.5";
	rotation = eulerToMatrix( "0 0 0" );

	className = "WeaponImage";

	// Projectile && Ammo.*
	// item = MaceAndShieldItem;
	projectile = MaceAndShieldProjectile;
	// projectile = swordProjectile;
	projectileType = Projectile;

	//melee particles shoot from eye node for consistancy
	melee = true;

	doColorShift = false;
	colorShiftColor = MaceAndShieldItem.colorShiftColor;

	// Images have a state system which controls how the animations
	// are run, which sounds are played, script callbacks, etc. This
	// state system is downloaded to the client so that clients can
	// predict state changes and animate accordingly.  The following
	// system supports basic ready->fire->reload transitions as
	// well as a no-ammo->dryfire idle state.

	// Initial start up state
	stateName[0]							= "Activate";
	stateTimeoutValue[0]					= 0.8;
	stateSequence[0]						= "Equip";
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]							= weaponSwitchSound;

	stateName[1]							= "Ready";
	stateTransitionOnTriggerDown[1]	= "Attack1";
	stateScript[1]							= "onReady";
	stateSequence[1]						= "Ready";

	stateName[2]							= "Attack1";
	stateTransitionOnTimeout[2]		= "Recovery1";
	stateScript[2]							= "onAttack";
	stateTimeoutValue[2]					= 0.2;
	stateSequence[2]						= "Attack1";

	stateName[3]							= "Recovery1";
	stateScript[3]							= "onRecovery";
	stateSequence[3]						= "Recovery1";
	stateTimeoutValue[3]					= 0.6;
	stateTransitionOnTimeout[3]		= "PreReady2";
	// stateTransitionOnTriggerUp[3]		= "Ready2";
	// stateWaitForTimeout[3]				= 1;

	stateName[4]							= "Ready2";
	stateTransitionOnTriggerDown[4]	= "Attack2";
	stateScript[4]							= "onReady";
	// stateSequence[4]						= "Ready";
	stateTransitionOnTimeout[4]		= "Ready";
	stateTimeoutValue[4]					= 0.8;

	stateName[5]							= "Attack2";
	stateTransitionOnTimeout[5]		= "Recovery2";
	stateScript[5]							= "onAttack";
	stateTimeoutValue[5]					= 0.2;
	stateSequence[5]						= "Attack2";

	stateName[6]							= "Recovery2";
	stateScript[6]							= "onRecovery";
	stateSequence[6]						= "Recovery2";
	stateTimeoutValue[6]					= 0.6;
	stateTransitionOnTimeout[6]		= "PreReady";
	// stateTransitionOnTriggerUp[6]		= "Ready";
	// stateWaitForTimeout[6]				= 1;

	stateName[7]							= "PreReady2";
	stateTransitionOnTriggerUp[7]		= "Ready2";

	stateName[8]							= "PreReady";
	stateTransitionOnTriggerUp[8]		= "Ready";
};

function MaceAndShieldImageA::onMount(%this, %obj, %slot)
{
	%obj.canShieldBlock = 1;
	%obj.playThread(1, armReadyBoth);
	%obj.hideNode("rhand");
	%obj.hideNode("rhook");
	%obj.hideNode("lhand");
	%obj.hideNode("lhook");
}

function MaceAndShieldImageA::onUnmount(%this, %obj, %slot)
{
	%obj.canShieldBlock = 0;
	if (isObject(%cl = %obj.client))
	{
		%obj.unHideNode($LHand[%cl.lhand]);
		%obj.unHideNode($RHand[%cl.rhand]);
	}
	else
	{
		%obj.unHideNode("rhand");
		%obj.unHideNode("lhand");
	}
}

function MaceAndShieldImageA::onReady(%this, %obj, %slot)
{
	%obj.canShieldBlock = 1;

	if (%obj.canShieldBlock && %obj.rmbDown)
	{
		%obj.canShieldBlock = 0;
		%obj.mountImage(ShieldBlockImageA, 0);
		return;
	}
}

function MaceAndShieldImageA::onAttack(%this, %obj, %slot)
{
	%obj.canShieldBlock = 0;

	// %p = new Projectile()
	// {
	// 	dataBlock = %this.projectile;
	// 	initialPosition = vectorAdd(%obj.getEyeTransform(), %obj.getEyeVector());
	// 	initialVelocity = vectorAdd(vectorScale(%obj.getEyeVector(), %this.projectile.muzzleVelocity),
	// 		vectorScale(%obj.getVelocity(), %this.projectile.velInheritFactor));
	// 	client = %obj.client;
	// 	sourceClient = %obj.client;
	// 	sourceObj = %obj;
	// };
	doMeleeAttack(%obj, %this.projectile, 1.5, 1.5, 0);
}

function MaceAndShieldImageA::onRecovery(%this, %obj, %slot)
{
	%obj.canShieldBlock = 0;
}

datablock ShapeBaseImageData(ShieldBlockImageA : MaceAndShieldImageA)
{
	stateName[0]							= "Parry";
	stateTimeoutValue[0]					= 0.3;
	stateSequence[0]						= "Block";
	stateTransitionOnTimeout[0]		= "Block1";
	stateAllowImageChange[0]			= false;
	stateSound[0]							= weaponSwitchSound;

	stateName[1]							= "Block1";
	stateTransitionOnTriggerDown[1]	= "";
	stateScript[1]							= "onBlock1";
	stateSequence[1]						= "";
	stateAllowImageChange[1]			= false;			//timeout swapping weapons/back to normal mace for 0.4s total
	stateTransitionOnTimeout[1]		= "Block2";
	stateTimeoutValue[1]					= 0.4;

	stateName[2]							= "Block2";
	stateTransitionOnTimeout[2]		= "Block2b";
	stateScript[2]							= "onBlock2";
	stateTimeoutValue[2]					= 0.1;
	stateSequence[2]						= "";
	stateAllowImageChange[2]			= true;

	stateName[3]							= "Block2b";
	stateScript[3]							= "onBlock2";
	stateSequence[3]						= "";
	stateTimeoutValue[3]					= 0.1;
	stateTransitionOnTimeout[3]		= "Block2";
	stateTransitionOnTriggerUp[3]		= "";
	stateWaitForTimeout[3]				= 0;

	stateName[4]							= "";
	stateTransitionOnTriggerDown[4]	= "";
	stateScript[4]							= "";
	// stateSequence[4]						= "";
	stateTransitionOnTimeout[4]		= "";
	stateTimeoutValue[4]					= 0;

	stateName[5]							= "";
	stateTransitionOnTimeout[5]		= "";
	stateScript[5]							= "";
	stateTimeoutValue[5]					= 0;
	stateSequence[5]						= "";

	stateName[6]							= "";
	stateScript[6]							= "";
	stateSequence[6]						= "";
	stateTimeoutValue[6]					= 0;
	stateTransitionOnTimeout[6]		= "";
	stateTransitionOnTriggerUp[6]		= "";
	stateWaitForTimeout[6]				= 0;

	stateName[7]							= "";
	stateTransitionOnTriggerUp[7]		= "";

	stateName[8]							= "";
	stateTransitionOnTriggerUp[8]		= "";
};

function ShieldBlockImageA::onMount(%this, %obj, %slot)
{
	%obj.isShieldBlocking = 1;
	%obj.shieldBlockPercent = 1;

	%obj.playThread(1, armReadyBoth);
	%obj.hideNode("rhand");
	%obj.hideNode("rhook");
	%obj.hideNode("lhand");
	%obj.hideNode("lhook");
}

function ShieldBlockImageA::onUnmount(%this, %obj, %slot)
{
	%obj.isShieldBlocking = 0;
	%obj.shieldBlockPercent = 0;
	if (isObject(%cl = %obj.client))
	{
		%obj.unHideNode($LHand[%cl.lhand]);
		%obj.unHideNode($RHand[%cl.rhand]);
	}
	else
	{
		%obj.unHideNode("rhand");
		%obj.unHideNode("lhand");
	}
}

function ShieldBlockImageA::onBlock1(%this, %obj, %slot)
{
	%obj.isShieldBlocking = 1;
	%obj.shieldBlockPercent = 0.5;
}

function ShieldBlockImageA::onBlock2(%this, %obj, %slot)
{
	if (!%obj.rmbDown)
	{
		%obj.shieldBlockPercent = 0;
		%obj.isShieldBlocking = 0;

		%obj.mountImage(MaceAndShieldImageA, %slot);

		return;
	}
	%obj.isShieldBlocking = 1;
	%obj.shieldBlockPercent = 0.5;
}

package MaceAndShieldPackage
{
	function Armor::onTrigger(%this, %obj, %trig, %val)
	{
		if (%trig == 4)
		{
			%obj.rmbDown = %val;

			if (%obj.canShieldBlock && %val)
			{
				%obj.canShieldBlock = 0;
				%obj.mountImage(ShieldBlockImageA, 0);
				return;
			}
		}

		parent::onTrigger(%this, %obj, %trig, %val);
	}

	function Armor::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType)
	{
		if (%obj.isShieldBlocking && %obj.shieldBlockPercent > 0)
		{
			%damage = %damage * (1 - %obj.shieldBlockPercent);
		}
		return parent::damage(%db, %obj, %sourceObj, %pos, %damage, %damageType);
	}
};
activatePackage(MaceAndShieldPackage);