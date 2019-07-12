//functions
function Rifle_onFire(%this, %obj, %slot)
{
	if(vectorLen(%obj.getVelocity()) < 2 && (getSimTime() - %obj.lastShotTime) > 1000)
	{
		%spread = 0.0001;
	}
	else
	{
		%spread = 0.0005;
	}
	%projectile = %this.projectile;
	%shellcount = 1;

	%obj.playThread(2, jump);
	%obj.schedule(60, playThread, 2, plant);
	%obj.toolAmmo[%obj.currTool]--;

	%p = fireProjectiles(%this, %obj, %slot, %projectile, %spread, %shellcount);

	centerprintToolAmmoString(%this, %obj, %slot);

	return %p;
}

function Rifle_onEject(%this, %obj, %slot)
{
	%obj.playThread(2, shiftright);
	%obj.schedule(100, playThread, 2, shiftleft);
	%obj.schedule(200, playThread, 2, plant);
	
	centerprintToolAmmoString(%this, %obj, %slot);
}

function Rifle_onReloadStart(%this, %obj, %slot)
{
	%obj.playThread(2, shiftright);
	%obj.schedule(100, playThread, 2, plant);

	centerprintToolAmmoString(%this, %obj, %slot);
}

function Rifle_onMount(%this, %obj, %slot)
{
	centerprintToolAmmoString(%this, %obj, %slot);
}

function Rifle_onReloaded(%this, %obj, %slot)
{
	%obj.toolAmmo[%obj.currTool] = %this.item.maxAmmo;
	centerprintToolAmmoString(%this, %obj, %slot);
}

function RifleProjectile_damage(%this, %obj, %col, %fade, %pos, %normal)
{
	if(%this.directDamage <= 0)
	{
		return;
	}

	%damageType = $DamageType::Direct;
	if(%this.directDamageType)
	{
		%damageType = %this.directDamageType;
	}

	%scale = getWord(%obj.getScale(), 2);
	%directDamage = %this.directDamage;
	%damage = %directDamage;

	%sobj = %obj.sourceObject;

	if(%col.getType() & $TypeMasks::PlayerObjectType)
	{
		%colscale = getWord(%col.getScale(),2);
		if (getword(%pos, 2) > getword(%col.getWorldBoxCenter(), 2) - 3.3 * %colscale)
		{
			%directDamage = %directDamage * 2;
			%damageType = $DamageType::Rifle;
		}

		%col.damage(%obj, %pos, %directDamage, %damageType);
	}
	else
	{
		%col.damage(%obj, %pos, %directDamage, %damageType);
	}
}


//audio
datablock AudioProfile(RifleReloadSound)
{
   filename    = "./assets/sounds/HK_Sniper_Rifle_Reload.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(RifleFireSound)
{
   filename    = "./assets/sounds/HK_Sniper_Rifle_FireB.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(HeavyRifleFireSound)
{
   filename    = "./assets/sounds/HK_Sniper_Rifle_Fire.wav";
   description = AudioClose3d;
   preload = true;
};

datablock AudioProfile(RifleRicochetSound)
{
   filename    = "./assets/sounds/HK_Ricochet.wav";
   description = AudioClose3d;
   preload = true;
};


//flash
datablock ParticleData(RifleFlashParticle)
{
    dragCoefficient      = 0;
    gravityCoefficient   = 0;
    inheritedVelFactor   = 0;
    constantAcceleration = 0.0;
    lifetimeMS           = 50;
    lifetimeVarianceMS   = 0;
    textureName          = "base/data/particles/star1";
    spinSpeed        = 9000.0;
    spinRandomMin        = -5000.0;
    spinRandomMax        = 5000.0;

    colors[0]     = "1.0 0.5 0 0.9";
    colors[1]     = "0.9 0.4 0 0.8";
    colors[2]     = "1 0.5 0.2 0.6";
    colors[3]     = "1 0.5 0.2 0.4";

    sizes[0]      = 2.95;
   sizes[1]      = 0.3;
    sizes[2]      = 0.10;
    sizes[3]      = 0.0;

   times[0] = 0.0;
   times[1] = 0.1;
   times[2] = 0.5;
   times[3] = 1.0;

    useInvAlpha = false;
};
datablock ParticleEmitterData(RifleFlashEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 54.0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 0;
   thetaMax         = 1;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;

   particles = "RifleFlashParticle";
};


//explosion
datablock ParticleData(RifleExplosionParticle)
{
	dragCoefficient      = 8;
	gravityCoefficient   = -0.5;
	inheritedVelFactor   = 0.2;
	constantAcceleration = 0.0;
	lifetimeMS           = 50;
	lifetimeVarianceMS   = 35;
	textureName          = "base/data/particles/star1";
	spinSpeed		= 500.0;
	spinRandomMin		= -500.0;
	spinRandomMax		= 500.0;
	colors[0]     = "1 1 0.0 0.9";
	colors[1]     = "0.9 0.0 0.0 0.0";
	sizes[0]      = 2.5;
	sizes[1]      = 1;

	useInvAlpha = false;
};

datablock ParticleEmitterData(RifleExplosionEmitter)
{
	lifeTimeMS = 50;

   ejectionPeriodMS = 3;
   periodVarianceMS = 0;
   ejectionVelocity = 0;
   velocityVariance = 0.0;
   ejectionOffset   = 0.0;
   thetaMin         = 89;
   thetaMax         = 90;
   phiReferenceVel  = 0;
   phiVariance      = 360;
   overrideAdvance = false;
   particles = "RifleExplosionParticle";

   useEmitterColors = true;
};

datablock ExplosionData(RifleExplosion)
{
	//explosionShape = "";
	// soundProfile = RifleRicochetSound;

	lifeTimeMS = 150;

	particleEmitter = gunExplosionRingEmitter;
	particleDensity = 5;
	particleRadius = 0.2;

	emitter[0] = RifleExplosionEmitter;

	faceViewer     = true;
	explosionScale = "1 1 1";

	shakeCamera = true;
	camShakeFreq = "10.0 11.0 10.0";
	camShakeAmp = "1.0 1.0 1.0";
	camShakeDuration = 0.5;
	camShakeRadius = 10.0;

	// Dynamic light
	lightStartRadius = 2;
	lightEndRadius = 2;
	lightStartColor = "0.5 0.8 0.9";
	lightEndColor = "0 0 0";
};








datablock ParticleData(RifleTrailBParticle)
{
	dragCoefficient		= 3.0;
	windCoefficient		= 0.0;
	gravityCoefficient	= 0.0;
	inheritedVelFactor	= 0.0;
	constantAcceleration	= 0.0;
	lifetimeMS		= 500;
	lifetimeVarianceMS	= 0;
	spinSpeed		= 10.0;
	spinRandomMin		= -50.0;
	spinRandomMax		= 50.0;
	useInvAlpha		= false;
	animateTexture		= false;
	//framesPerSec		= 1;

	textureName		= "base/data/particles/cloud";
	//animTexName		= "~/data/particles/dot";

	// Interpolation variables
	colors[0]	= "1 1 1 0.1";
	colors[1]	= "0.7 0.7 0.7 0.08";
	colors[2]	= "0.4 0.4 0.4 0";
	sizes[0]	= 0.5;
	sizes[1]	= 0.25;
	sizes[2]	= 0.0;
	times[0]	= 0.0;
	times[1]	= 0.1;
	times[2]	= 1.0;
};

datablock ParticleEmitterData(RifleTrailBEmitter)
{
   ejectionPeriodMS = 1;
   periodVarianceMS = 0;
   ejectionVelocity = 0; //0.25;
   velocityVariance = 0; //0.10;
   ejectionOffset = 0;
   thetaMin         = 0.0;
   thetaMax         = 90.0;  

   particles = RifleTrailBParticle;
};

AddDamageType("Rifle",   '<bitmap:add-ons/Weapon_Gun/CI_Gun> %1',    '%2 <bitmap:add-ons/Weapon_Gun/CI_Gun> %1',0.75,1);
datablock ProjectileData(RustyRifleProjectile)
{
   projectileShapeName = "add-ons/Weapon_Gun/bullet.dts";
   directDamage        = 20;
   directDamageType    = $DamageType::Rifle;
   radiusDamageType    = $DamageType::Rifle;

   brickExplosionRadius = 0;
   brickExplosionImpact = false;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 0;
   brickExplosionMaxVolume = 0;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 0;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse	     = 800;
   verticalImpulse     = 500;
   explosion           = RifleExplosion;
   particleEmitter     = RifleTrailBEmitter;

   muzzleVelocity      = 100;
   velInheritFactor    = 1;

   armingDelay         = 0;
   lifetime            = 9000;
   fadeDelay           = 9000;
   bounceElasticity    = 0.5;
   bounceFriction      = 0.20;
   isBallistic         = true;
   gravityMod = 0.2;

   hasLight    = false;
   lightRadius = 3.0;
   lightColor  = "0 0 0.5";
};

//////////
// item //
//////////
datablock ItemData(RustyRifleItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./assets/rifles/snubRifle.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Rusty Rifle";
	iconName = "";
	doColorShift = true;
	colorShiftColor = "0.47 0.33 0.33 1";

	 // Dynamic properties defined by the scripts
	image = RustyRifleImage;
	canDrop = true;
	
	maxAmmo = 5;
	canReload = 1;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(RustyRifleImage)
{
   // Basic Item properties
   shapeFile = "./assets/rifles/snubRifle.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = RustyRifleItem;
   ammo = " ";
   projectile = RustyRifleProjectile;
   projectileType = Projectile;

   casing = GunShellDebris;
   shellExitDir        = "1.0 0.1 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;	
   shellVelocity       = 5.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   minShotTime = 1000;

   doColorShift = true;
   colorShiftColor = RustyRifleItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]						= "Activate";
	stateTimeoutValue[0]				= 0.15;
	stateSequence[0]					= "Activate";
	stateTransitionOnTimeout[0]			= "Bolt";
	stateSound[0]						= weaponSwitchSound;

	stateName[1]                    	= "Ready";
	stateTransitionOnTimeout[1]     	= "Ready";
	stateTimeoutValue[1]            	= 0.05;
	stateScript[1]                  	= "onDisplay";
	stateTransitionOnTriggerDown[1] 	= "Fire";
	stateTransitionOnNoAmmo[1]			= "ReloadedA";
	stateAllowImageChange[1]			= true;
	stateSequence[1]					= "ready";

	stateName[2]						= "Fire";
	stateTransitionOnTimeout[2]			= "Smoke";
	stateTimeoutValue[2]				= 0.3;
	stateFire[2]						= true;
	stateAllowImageChange[2]			= false;
	stateScript[2]						= "onFire";
	stateWaitForTimeout[2]				= true;
	stateEmitter[2]						= RifleFlashEmitter;
	stateEmitterTime[2]					= 0.05;
	stateEmitterNode[2]					= "muzzleNode";
	stateSound[2]						= RifleFireSound;

	stateName[3]						= "Smoke";
	stateEmitter[3]						= GunSmokeEmitter;
	stateEmitterTime[3]					= 0.3;
	stateEmitterNode[3]					= "muzzleNode";
	stateTimeoutValue[3]				= 0.15;
	stateTransitionOnTimeout[3]			= "Bolt";

	stateName[4]						= "Bolt";
	stateTimeoutValue[4]				= 0.8;
	stateTransitionOnTimeout[4]			= "LoadCheckA";
	stateWaitForTimeout[4]				= true;
	stateEjectShell[4]					= true;
	stateSequence[4]					= "Bolt";
	stateSound[4]						= RifleReloadSound;
	stateScript[4]						= "onEject";
	
	stateName[5]						= "LoadCheckA";
	stateScript[5]						= "onLoadCheck";
	stateTimeoutValue[5]				= 0.01;
	stateTransitionOnTimeout[5]			= "LoadCheckB";
						
	stateName[6]						= "LoadCheckB";
	stateTransitionOnAmmo[6]			= "Ready";
	stateTransitionOnNoAmmo[6]			= "ForceReload";
	
	stateName[7]						= "ReloadedA";
	stateScript[7]						= "onReloadComplete";
	stateTimeoutValue[7]				= 0.01;
	stateTransitionOnTimeout[7]			= "ReloadedB";
						
	stateName[8]						= "ReloadedB";
	stateTransitionOnAmmo[8]			= "CompleteReload";
	stateTransitionOnNoAmmo[8]			= "Reload";
						
	stateName[9]						= "ForceReload";
	stateTransitionOnTimeout[9]			= "ForceReloaded";
	stateTimeoutValue[9]				= 1.4;
	stateSequence[9]					= "Fire";
	stateSound[9]						= BrickMoveSound;
	stateScript[9]						= "onReloadStart";
	
	stateName[10]						= "ForceReloaded";
	stateTransitionOnTimeout[10]		= "ReloadedA";
	stateTimeoutValue[10]				= 0.2;
	stateScript[10]						= "onReloaded";
	
	stateName[11]						= "Reload";
	stateTransitionOnTimeout[11]		= "Reloaded";
	stateTransitionOnTriggerDown[11]	= "Fire";
	stateWaitForTimeout[11]				= false;
	stateTimeoutValue[11]				= 1.4;
	stateSequence[11]					= "Fire";
	stateSound[11]						= BrickMoveSound;
	stateScript[11]						= "onReloadStart";
	
	stateName[12]						= "Reloaded";
	stateTransitionOnTimeout[12]     	= "ReloadedA";
	stateTransitionOnTriggerDown[12]	= "Fire";
	stateWaitForTimeout[12]				= false;
	stateTimeoutValue[12]				= 0.2;
	stateScript[12]						= "onReloaded";

	stateName[13]						= "CompleteReload";
	stateTimeoutValue[13]				= 1.0;
	stateTransitionOnTimeout[13]		= "Ready";
	stateWaitForTimeout[13]				= true;
	stateSequence[13]					= "bolt";
	stateSound[13]						= RifleReloadSound;
	stateScript[13]						= "onEject";
};

function RustyRifleImage::onFire(%this, %obj, %slot)
{
	return Rifle_onFire(%this, %obj, %slot);
}

function RustyRifleImage::onEject(%this, %obj, %slot)
{
	Rifle_onEject(%this, %obj, %slot);
}

function RustyRifleImage::onReloadStart(%this, %obj, %slot)
{
	Rifle_onReloadStart(%this, %obj, %slot);
}

function RustyRifleImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
	Rifle_onMount(%this, %obj, %slot);
}

function RustyRifleImage::onLoadCheck(%this, %obj, %slot)
{
	Parent::onLoadCheck(%this, %obj, %slot);
}

function RustyRifleImage::onReloaded(%this, %obj, %slot)
{
	Parent::onReloaded(%this, %obj, %slot);
	Rifle_onReloaded(%this, %obj, %slot);
}

function RustyRifleProjectile::damage(%this, %obj, %col, %fade, %pos, %normal)
{
	RifleProjectile_damage(%this, %obj, %col, %fade, %pos, %normal);
}






datablock ProjectileData(CleanRifleProjectile : RustyRifleProjectile)
{
	directDamage = 40;
	muzzleVelocity = 125;
};

datablock ItemData(CleanRifleItem : RustyRifleItem)
{
	shapeFile = "./assets/rifles/rifle.dts";
	uiName = "Clean Rifle";
	image = CleanRifleImage;

	colorShiftColor = "0.23 0.26 0.32 1";
	maxAmmo = 6;
};

datablock ShapeBaseImageData(CleanRifleImage : RustyRifleImage)
{
	shapeFile = "./assets/rifles/rifle.dts";
	colorShiftColor = CleanRifleItem.colorShiftColor;
	projectile = CleanRifleProjectile;

	item = CleanRifleItem;

	stateTimeoutValue[2] = 0.2; //reduce fire state time (0.3 >> 0.15)
	stateTimeoutValue[3] = 0.15; //reduce smoke time (0.3 >> 0.15)
	stateTimeoutValue[4] = 0.7; //reduce bolt time (0.8 >> 0.7)

	stateTimeoutValue[9] = 1; //reduce reload time (1.4 >> 1)
	stateTimeoutValue[11] = 1; //reduce reload time (1.4 >> 1)
};

function CleanRifleImage::onFire(%this, %obj, %slot)
{
	return Rifle_onFire(%this, %obj, %slot);
}

function CleanRifleImage::onEject(%this, %obj, %slot)
{
	Rifle_onEject(%this, %obj, %slot);
}

function CleanRifleImage::onReloadStart(%this, %obj, %slot)
{
	Rifle_onReloadStart(%this, %obj, %slot);
}

function CleanRifleImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
	Rifle_onMount(%this, %obj, %slot);
}

function CleanRifleImage::onLoadCheck(%this, %obj, %slot)
{
	Parent::onLoadCheck(%this, %obj, %slot);
}

function CleanRifleImage::onReloaded(%this, %obj, %slot)
{
	Parent::onReloaded(%this, %obj, %slot);
	Rifle_onReloaded(%this, %obj, %slot);
}

function CleanRifleProjectile::damage(%this, %obj, %col, %fade, %pos, %normal)
{
	RifleProjectile_damage(%this, %obj, %col, %fade, %pos, %normal);
}






datablock ProjectileData(PolishedRifleProjectile : RustyRifleProjectile)
{
	directDamage = 60;
	muzzleVelocity = 170;
};

datablock ItemData(PolishedRifleItem : RustyRifleItem)
{
	shapeFile = "./assets/rifles/longRifle.dts";
	uiName = "Polished Rifle";
	image = PolishedRifleImage;

	colorShiftColor = "0.22 0.40 0.38 1";
	maxAmmo = 8;
};

datablock ShapeBaseImageData(PolishedRifleImage : RustyRifleImage)
{
	shapeFile = "./assets/rifles/longRifle.dts";
	colorShiftColor = PolishedRifleItem.colorShiftColor;
	projectile = PolishedRifleProjectile;

	item = PolishedRifleItem;

	stateTimeoutValue[2] = 0.08; //reduce fire state time (0.15 >> 0.08)
	stateTimeoutValue[3] = 0.05; //reduce smoke time (0.15 >> 0.05)
	stateTimeoutValue[4] = 0.7; //reduce bolt time (0.7 >> 0.7)

	stateTimeoutValue[9] = 0.8; //reduce reload time (1 >> 0.8)
	stateTimeoutValue[11] = 0.8; //reduce reload time (1 >> 0.8)
};

function PolishedRifleImage::onFire(%this, %obj, %slot)
{
	return Rifle_onFire(%this, %obj, %slot);
}

function PolishedRifleImage::onEject(%this, %obj, %slot)
{
	Rifle_onEject(%this, %obj, %slot);
}

function PolishedRifleImage::onReloadStart(%this, %obj, %slot)
{
	Rifle_onReloadStart(%this, %obj, %slot);
}

function PolishedRifleImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
	Rifle_onMount(%this, %obj, %slot);
}

function PolishedRifleImage::onLoadCheck(%this, %obj, %slot)
{
	Parent::onLoadCheck(%this, %obj, %slot);
}

function PolishedRifleImage::onReloaded(%this, %obj, %slot)
{
	Parent::onReloaded(%this, %obj, %slot);
	Rifle_onReloaded(%this, %obj, %slot);
}

function PolishedRifleProjectile::damage(%this, %obj, %col, %fade, %pos, %normal)
{
	RifleProjectile_damage(%this, %obj, %col, %fade, %pos, %normal);
}






datablock ProjectileData(MSniperRifleProjectile : RustyRifleProjectile)
{
	directDamage = 200;
	muzzleVelocity = 200;
};

datablock ItemData(MSniperRifleItem : RustyRifleItem)
{
	shapeFile = "./assets/rifles/sniperRifle.dts";
	uiName = "Sniper Rifle";
	image = MSniperRifleImage;

	colorShiftColor = "0.34 0.34 0.34 1";
	maxAmmo = 8;
};

datablock ShapeBaseImageData(MSniperRifleImage : RustyRifleImage)
{
	shapeFile = "./assets/rifles/sniperRifle.dts";
	colorShiftColor = MSniperRifleItem.colorShiftColor;
	projectile = MSniperRifleProjectile;

	item = MSniperRifleItem;

	stateTransitionOnTimeout[0]			= "Smoke"; //make q spam slower than actually firing normally
	stateSound[2] = HeavyRifleFireSound;

	stateTimeoutValue[2] = 0.5; //increase fire state time (0.3 >> 0.5)
	stateTimeoutValue[3] = 0.6; //increase smoke time (0.2 >> 0.6)
	stateTimeoutValue[4] = 0.8; //increase bolt time (0.8 >> 0.8)

	stateTimeoutValue[9] = 1.8; //increase reload time (1.4 >> 1.8)
	stateTimeoutValue[11] = 1.8; //increase reload time (1.4 >> 1.8)
};

function MSniperRifleImage::onFire(%this, %obj, %slot)
{
	return Rifle_onFire(%this, %obj, %slot);
}

function MSniperRifleImage::onEject(%this, %obj, %slot)
{
	Rifle_onEject(%this, %obj, %slot);
}

function MSniperRifleImage::onReloadStart(%this, %obj, %slot)
{
	Rifle_onReloadStart(%this, %obj, %slot);
}

function MSniperRifleImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);
	Rifle_onMount(%this, %obj, %slot);
}

function MSniperRifleImage::onLoadCheck(%this, %obj, %slot)
{
	Parent::onLoadCheck(%this, %obj, %slot);
}

function MSniperRifleImage::onReloaded(%this, %obj, %slot)
{
	Parent::onReloaded(%this, %obj, %slot);
	Rifle_onReloaded(%this, %obj, %slot);
}

function MSniperRifleProjectile::damage(%this, %obj, %col, %fade, %pos, %normal)
{
	RifleProjectile_damage(%this, %obj, %col, %fade, %pos, %normal);
}