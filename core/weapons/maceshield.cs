datablock ShapeBaseImageData(MaceAndShieldImage)
{
	shapeFile = "./assets/maceshield/maceshield.dts";
	emap = true;

	mountPoint = 0;
	offset = "0 0 0";
	eyeOffset = 0; //"0.7 1.2 -0.5";
	rotation = eulerToMatrix( "0 0 0" );

	className = "WeaponImage";

	// Projectile && Ammo.
	item = MaceAndShieldItem;
	ammo = " ";
	projectile = MaceAndShieldProjectile;
	projectileType = Projectile;

	casing = GunShellDebris;
	shellExitDir        = "1.0 0.1 1.0";
	shellExitOffset     = "0 0 0";
	shellExitVariance   = 10.0;	
	shellVelocity       = 5.0;

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
	stateTimeoutValue[0]					= 0.2;
	stateSequence[0]						= "Equip";
	stateTransitionOnTimeout[0]		= "Ready";
	stateSound[0]							= weaponSwitchSound;

	stateName[1]							= "Ready";
	stateTransitionOnTriggerDown[1]	= "Attack";
	stateScript[1]							= "onReady";
	stateSequence[1]						= "Ready";
	// stateTransitionOnTimeout[1]		= "Ready";
	// stateTimeoutValue[1]					= 0.1;

	stateName[2]							= "Attack";
	stateTransitionOnTimeout[2]		= "Recovery";
	stateScript[2]							= "onAttack";
	stateTimeoutValue[2]					= 0.2;
	stateSequence[2]						= "Attack";

	stateName[3]							= "Recovery";
	stateScript[3]							= "onRecovery";
	stateTimeoutValue[3]					= 0.2;
	stateTransitionOnTimeout[3]		= "Ready";
};

function MaceAndShieldImage::onMount(%this, %obj, %slot)
{
	%obj.canShieldBlock = 1;
	%obj.playThread(1, armReadyRight);
	%obj.hideNode("rhand");
	%obj.hideNode("rhook");
	%obj.hideNode("lhand");
	%obj.hideNode("lhook");
}

function MaceAndShieldImage::onUnmount(%this, %obj, %slot)
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

function MaceAndShieldImage::onReady(%this, %obj, %slot)
{
	%obj.canShieldBlock = 1;
}

function MaceAndShieldImage::onAttack(%this, %obj, %slot)
{
	%obj.canShieldBlock = 0;
}

function MaceAndShieldImage::onRecovery(%this, %obj, %slot)
{
	%obj.canShieldBlock = 1;
}

datablock ShapeBaseImageData(ShieldBlockImage : MaceAndShieldImage)
{
	stateName[0]							= "Parry";
	stateTimeoutValue[0]					= 0.2;
	stateSequence[0]						= "Block";
	stateTransitionOnTimeout[0]		= "Block";
	stateAllowImageChange[0]			= false;
	stateSound[0]							= weaponSwitchSound;

	stateName[1]							= "Block1";
	stateTransitionOnTriggerDown[1]	= "";
	stateScript[1]							= "onBlock1";
	stateSequence[1]						= "";
	stateAllowImageChange[1]			= false;			//timeout swapping weapons/back to normal mace for 0.4s total
	stateTransitionOnTimeout[1]		= "Block2";
	stateTimeoutValue[1]					= 0.2;

	stateName[2]							= "Block2";
	stateTransitionOnTimeout[2]		= "";
	stateScript[2]							= "onBlock2";
	stateTimeoutValue[2]					= 0;
	stateSequence[2]						= "";
	stateAllowImageChange[2]			= true;

	stateName[3]							= "";
	stateScript[3]							= "";
	stateTimeoutValue[3]					= 0;
	stateTransitionOnTimeout[3]		= "";
};

function ShieldBlockImage::onMount(%this, %obj, %slot)
{
	%obj.isShieldBlocking = 1;
	%obj.shieldBlockPercent = 1;

	%obj.playThread(1, armReadyRight);
	%obj.hideNode("rhand");
	%obj.hideNode("rhook");
	%obj.hideNode("lhand");
	%obj.hideNode("lhook");
}

function ShieldBlockImage::onUnmount(%this, %obj, %slot)
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

function ShieldBlockImage::onBlock1(%this, %obj, %slot)
{
	%obj.isShieldBlocking = 1;
	%obj.shieldBlockPercent = 0.5;
}

function ShieldBlockImage::onBlock2(%this, %obj, %slot)
{
	if (!%obj.rmbDown)
	{
		%obj.shieldBlockPercent = 0;
		%obj.isShieldBlocking = 0;

		%obj.mountImage(MaceAndShieldImage, %slot);

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

			if (%obj.canShieldBlock)
			{
				%obj.canShieldBlock = 0;
				%obj.mountImage(ShieldBlockImage, 0);
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