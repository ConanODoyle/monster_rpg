function fireProjectiles(%this, %obj, %slot, %projectile, %spread, %shellcount)
{
	for(%shell = 0; %shell < %shellcount; %shell++)
	{
		%vector = %obj.getMuzzleVector(%slot);
		%objectVelocity = %obj.getVelocity();
		%vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
		%vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
		%velocity = VectorAdd(%vector1, %vector2);
		
		%x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		%z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
		
		%mat = MatrixCreateFromEuler(%x SPC %y SPC %z);
		%velocity = MatrixMulVector(%mat, %velocity);

		%p = new (%this.projectileType)()
		{
			dataBlock = %projectile;
			initialVelocity = %velocity;
			initialPosition = %obj.getMuzzlePoint(%slot);
			sourceObject = %obj;
			sourceSlot = %slot;
			client = %obj.client;
		};
		MissionCleanup.add(%p);
		%result = %result SPC %p;
	}

	return trim(%result);
}

function centerprintToolAmmoString(%this, %obj, %slot)
{
	%cl = %obj.client;
	%currAmmo = %obj.toolAmmo[%obj.currTool];
	if (isObject(%cl))
	{
		%cl.centerprint("<just:right><font:Consolas:24>\c3" @ %currAmmo @ " / " @ %this.item.maxAmmo @ " ", 4);
	}
}