// - searchType (ScriptObject)
// 	- searchFunction
// 		- options: _radiusSearch, _radiusFOVSearch
// 		- searchRadius (RadiusSearch options)
// 	- requireLOS (RadiusSearch options)
// 	- searchFOV (FOVSearch options)

if (!isObject(SearchTypes))
{
	$SearchTypes = new SimSet(SearchTypes) {};
	
	SearchTypes.add(new ScriptObject(NearRadiusSearchType)      { searchFunction = "MRPGBot_radiusSearch"; searchRadius = 30; } );
	SearchTypes.add(new ScriptObject(RadiusSearchType)          { searchFunction = "MRPGBot_radiusSearch"; searchRadius = 60; } );
	SearchTypes.add(new ScriptObject(FarRadiusSearchType)       { searchFunction = "MRPGBot_radiusSearch"; searchRadius = 90; } );
	SearchTypes.add(new ScriptObject(NearRadiusLOSSearchType)   { searchFunction = "MRPGBot_radiusSearch"; searchRadius = 20; requireLOS = 1; } );
	SearchTypes.add(new ScriptObject(RadiusLOSSearchType)       { searchFunction = "MRPGBot_radiusSearch"; searchRadius = 60; requireLOS = 1; } );
	SearchTypes.add(new ScriptObject(FarRadiusLOSSearchType)    { searchFunction = "MRPGBot_radiusSearch"; searchRadius = 90; requireLOS = 1; } );
	SearchTypes.add(new ScriptObject(LOSSearchType)             { searchFunction = "MRPGBot_radiusSearch"; searchRadius = 1000; requireLOS = 1; } );
	SearchTypes.add(new ScriptObject(NearRadiusFOVSearchType)   { searchFunction = "MRPGBot_radiusFOVSearch"; searchRadius = 20; searchFOV = 160; } );
	SearchTypes.add(new ScriptObject(RadiusFOVSearchType)       { searchFunction = "MRPGBot_radiusFOVSearch"; searchRadius = 60; searchFOV = 160; } );
	SearchTypes.add(new ScriptObject(FarRadiusFOVSearchType)    { searchFunction = "MRPGBot_radiusFOVSearch"; searchRadius = 90; searchFOV = 160; } );
	SearchTypes.add(new ScriptObject(FOVSearchType)             { searchFunction = "MRPGBot_radiusFOVSearch"; searchRadius = 1000; searchFOV = 160; } );
	
	schedule(1000, 0, eval, "MissionCleanup.add(SearchTypes);");
}

function MRPGBot_radiusSearch(%bot)
{
	%data = %bot.RPGData.searchType;
	%pos = %bot.getPosition();
	
	for (%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%cl = ClientGroup.getObject(%i);
		%pl = %cl.player;

		if (!isObject(%cl.minigame) && !%cl.enableMRPG)
		{
			continue;
		}
		
		if (vectorDist(%pos, %pl.getPosition()) <= %data.searchRadius) //within range
		{
			if (%data.requireLOS) //check for line of sight (bot eye to player)
			{
				%masks = $Typemasks::fxBrickObjectType;
				%start = %bot.getEyeTransform();
				%ends = %pl.getEyeTransform() TAB %pl.getHackPosition() TAB %pl.getPosition();
				%pass = 0;
				for (%j = 0; %j < getFieldCount(%ends); %j++)
				{
					%ray = containerRaycast(%start, getField(%ends, %j), %masks);
					if (!isObject(%ray))
					{
						%pass = 1;
						break;
					}
				}
				if (!%pass)
				{
					continue;
				}
			}
			%search = %search SPC %pl;
		}
	}
	
	return getWords(%search, 1, 100);
}

function MRPGBot_radiusFOVSearch(%bot)
{
	%data = %bot.RPGData.searchType;
	%eyePos = %bot.getEyeTransform();
	
	%radiusSearch = MRPGBot_radiusSearch(%bot);
	for (%i = 0; %i < getWordCount(%radiusSearch); %i++)
	{
		%pl = getWord(%radiusSearch, %i);
		%vec1 = vectorNormalize(vectorSub(%pl.getHackPosition(), %eyePos));
		%vec2 = vectorNormalize(%bot.getEyeVector());
		if (mACos(vectorDot(%vec1, %vec2)) <= %data.searchFOV / 180 * 3.14) //convert FOV (degrees) to radians
		{
			%search = %search SPC %pl;
		}
	}
	return getWords(%search, 1, 100);
}
