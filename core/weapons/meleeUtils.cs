function doMeleeAttack(%pl, %proj, %eyerange, %radius, %hitAll, %slot)
{
	if (!isObject(%pl) || !isObject(%proj))
	{
		error("Cannot melee attack with invalid objects!");
		return;
	}

	%eye = %pl.getEyeTransform();
	// %eyeVec = %pl.getEyeVector();
	%eyeVec = %pl.getMuzzleVector(3); //eyeoffset fucks with this first person >:(
	%hackPos = %pl.getHackPosition();
	%searchPos = vectorAdd(%eye, vectorScale(%eyeVec, %eyerange));
	%masks = $Typemasks::PlayerObjectType | $Typemasks::VehicleObjectType;
	%objectMasks = $Typemasks::TerrainObjectType | $Typemasks::fxBrickObjectType | $Typemasks::StaticShapeObjectType;

	//do basic raycast search before radius
	//skip if %hitAll - we're hitting all in search anyways

	if ($debug)
	{
		%p = new Projectile()
		{
			dataBlock = pongProjectile;
			initialPosition = %searchPos;
			initialVelocity = "0 0 0";
		};
		%p.setscale("0.1 0.1 0.1");
		%p.schedule(1100, delete);

		%p = new Projectile()
		{
			dataBlock = pongProjectile;
			initialPosition = %searchPos;
			initialVelocity = "0 0 0.1";
		};
		%p.setscale(vectorScale("2 2 2", %radius));
		%p.schedule(500, delete);
	}

	%raycastPos = vectorAdd(%searchPos, vectorScale(%eyeVec, %radius));
	%simpleHit = containerRaycast(%eye, %raycastPos, %objectMasks | %masks, %pl);
	if (isObject(%simpleHit))
	{
		if (%simpleHit.getType() & %masks && !%hitAll)
		{
			//we hit a player/vehicle, hit them;
			if ($debug)
				talk("Melee: SimpleHit " @ getWord(%simpleHit, 0) @ " at " @ posFromRaycast(%simpleHit));
			
			%nextPos = %simpleHit.getPosition();
			%wb = %simpleHit.getWorldBox();
			%topPos = vectorScale(vectorAdd(getWords(%wb, 0, 2), getWords(%wb, 3, 5)), 0.5);
			%topPos = getWords(%topPos, 0, 1) SPC (getWord(%topPos, 2) / 2);

			%p = new Projectile()
			{
				dataBlock = %proj;
				initialPosition = vectorScale(vectorAdd(%nextPos, %topPos), 0.5);
				initialVelocity = vectorScale(%eyeVec, %proj.muzzleVelocity);
				sourceObject = %pl;
				sourceSlot = %slot;
				client = %pl.client;
			};
			return;
		}
	}

	initContainerRadiusSearch(%searchPos, %radius, %masks);

	while (isObject(%next = containerSearchNext()))
	{
		if (%next == %pl)
		{
			continue;
		}

		%nextPos = %next.getPosition();
		%wb = %next.getWorldBox();
		%topPos = vectorScale(vectorAdd(getWords(%wb, 0, 2), getWords(%wb, 3, 5)), 0.5);
		%topPos = getWords(%topPos, 0, 1) SPC (getWord(%topPos, 2) / 2);

		%hit = containerRaycast(%eye, %nextPos, %objectMasks | %masks, %pl);
		%vec = vectorNormalize(vectorSub(%eye, %nextPos));
		if (%hit == %next)
		{
			//we hit a player/vehicle, hit them IF they are in range;
			if (%hit.getType() & $Typemasks::PlayerObjectType)
			{
				//player worldbox is huge so make sure its actually in range
				%np1 = %nextPos;
				%np2 = %topPos;
				
				if ($debug)
				{
					talk("Distance check1: " @ vectorDist(%np1, %searchPos) @ " vs " @ %radius);
					talk("Distance check2: " @ vectorDist(%np2, %searchPos) @ " vs " @ %radius);
				}

				if ((vectorDist(%searchPos, %np1) > %radius && vectorDist(%searchPos, %np2) > %radius)
					|| (vectorDist(%np1, %eye) > %radius + %eyerange && vectorDist(%np2, %eye) > %radius + %eyerange))
				{
					if ($debug)
						talk("Distance check failed");

					continue;
				}
			}

			if ($debug)
			{
				talk("Melee: RadiusHit " @ getWord(%hit, 0) @ " at " @ posFromRaycast(%hit));
			}
			%p = new Projectile()
			{
				dataBlock = %proj;
				initialPosition = vectorScale(vectorAdd(%nextPos, %topPos), 0.5);
				initialVelocity = vectorScale(%eyeVec, %proj.muzzleVelocity);
				sourceObject = %pl;
				sourceSlot = %slot;
				client = %pl.client;
			};

			if (!%hitAll)
			{
				return;
			}
		}
	}

	if (isObject(%simpleHit)) //if we hit no players, but are looking at a brick/non-player-object
	{
		if ($debug)
			talk("Melee: FinalSimpleHit " @ getWord(%simpleHit, 0) @ " at " @ posFromRaycast(%simpleHit));
		
		%p = new Projectile()
		{
			dataBlock = %proj;
			initialPosition = posFromRaycast(%simpleHit);
			initialVelocity = vectorScale(%eyeVec, %proj.muzzleVelocity);
			sourceObject = %pl;
			sourceSlot = %slot;
			client = %pl.client;
		};
		return;
	}
}