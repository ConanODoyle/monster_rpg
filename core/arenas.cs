// //Arenas:
// //Desert
// //Castle

// $Arena_Loc[0] = "Desert";
// $Arena_Shop[0] = "_shopSpawn";
// $Arena_MobSpawners["Desert"] = "_small _medium _large _archer";
// $Arena_Levels[0] = 1 SPC 20;
// $Arena_ShopLevels[0] = 10 SPC 19;
// exec("./arenaData/desert1.cs");

// $Arena_Loc[1] = "Castle";
// $Arena_Shop[1] = "_shopSpawn";
// $Arena_Levels[1] = 21 SPC 40;
// $Arena_ShopLevels[1] = 30 SPC 39;
// // exec("./arenaData/castle1.cs");

// $Arena_LocCount = 2;


// function getArenaID(%roundNum)
// {
// 	if ($lastRetrievedArenaID !$= "" && $lastRetrievedArenaRoundNum == %roundNum)
// 	{
// 		return $lastRetrievedArenaID;
// 	}

// 	for (%i = 0; %i < $Arena_LocCount; %i++)
// 	{
// 		%range = $Arena_Levels[%i];
// 		if (%roundNum >= getWord(%range, 0) && %roundNum <= getWord(%range, 1))
// 		{
// 			$lastRetrievedArenaID = %i;
// 			$lastRetrievedArenaRoundNum = %roundNum;
// 			return %i;
// 		}
// 	}
// }

// function getArenaMobData(%roundNum)
// {
// 	%final = getArenaID(%roundNum);

// 	%mobs = $Arena_LocLevel[%final, %roundNum];

// 	return %mobs;
// }

// function getArenaMobSpawners(%arenaName)
// {
// 	return $Arena_MobSpawners[%arenaName];
// }

// function getArenaShopSpawn(%roundNum)
// {
// 	%brick = $Arena_Shop[getArenaID()];
// 	%bounds = %brick.getWorldBox();
// 	%min = getWords(%bounds, 0, 2);
// 	%max = getWords(%bounds, 3, 5);

// 	%x = getRandom(getWord(%min, 0) * 2, getWord(%max, 0) * 2) / 2;
// 	%y = getRandom(getWord(%min, 1) * 2, getWord(%max, 1) * 2) / 2;
// 	%z = getRandom(getWord(%min, 2) * 2, getWord(%max, 2) * 2) / 2;
// 	return %x SPC %y SPC %z SPC getWords(%brick.getTransform(), 3, 6);
// }

// function getArenaSpawn(%roundNum)
// {
// 	%brick = $Arena_Loc[getArenaID()];
// 	%bounds = %brick.getWorldBox();
// 	%min = getWords(%bounds, 0, 2);
// 	%max = getWords(%bounds, 3, 5);

// 	%x = getRandom(getWord(%min, 0) * 2, getWord(%max, 0) * 2) / 2;
// 	%y = getRandom(getWord(%min, 1) * 2, getWord(%max, 1) * 2) / 2;
// 	%z = getRandom(getWord(%min, 2) * 2, getWord(%max, 2) * 2) / 2;
// 	return %x SPC %y SPC %z SPC getWords(%brick.getTransform(), 3, 6);
// }