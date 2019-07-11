function getLevelEXP(%level)
{
	%answer = 0;

	for(%i = 1; %i < %level; %i++)
	{
		%answer += mFloor(%i + 300 * mpow(2, (%i / 35)));
	}

	return mFloor(%answer / 4);
}

function GameConnection::addExperience(%cl, %amt)
{
	%amt = mFloor(%amt);
	%cl.exp = ((%cl.exp | 0) + %amt | 0) | 0;
	%levelExp = getLevelEXP(%cl.level + 1) | 0;
	while (%cl.exp >= %levelExp)
	{
		%cl.exp = ((%cl.exp | 0) - %levelExp | 0);
		%cl.levelUp(1);
		%levelExp = getLevelEXP(%cl.level + 1) | 0;
	}
}

function GameConnection::levelUp(%cl, %amt)
{
	%cl.level += %amt;
	if (isObject(%cl.player))
	{
		%pl = %cl.player;
		%pl.spawnExplosion("spawnProjectile", "1 1 1");
		
		if (%amt > 0)
		{
			%pl.setDamageLevel(%pl.getDamageLevel() - %pl.getDatablock().maxDamage * 0.1); //heal 10% on level up
		}
	}
	%cl.playSound(rewardSound);
	%cl.maxDamage = 100 + (%cl.level - 1) * 10;
}

// Level 1: 0
// Level 2: 83
// Level 3: 174
// Level 4: 276
// Level 5: 388
// Level 6: 512
// Level 7: 650
// Level 8: 801
// Level 9: 969
// Level 10: 1154
// Level 11: 1358
// Level 12: 1584
// Level 13: 1833
// Level 14: 2107
// Level 15: 2411
// Level 16: 2746
// Level 17: 3115
// Level 18: 3523
// Level 19: 3973
// Level 20: 4470
// Level 21: 5018
// Level 22: 5624
// Level 23: 6291
// Level 24: 7028
// Level 25: 7842
// Level 26: 8740
// Level 27: 9730
// Level 28: 10824
// Level 29: 12031
// Level 30: 13363
// Level 31: 14833
// Level 32: 16456
// Level 33: 18247
// Level 34: 20224
// Level 35: 22406
// Level 36: 24815
// Level 37: 27473
// Level 38: 30408
// Level 39: 33648
// Level 40: 37224
// Level 41: 41171
// Level 42: 45529
// Level 43: 50339
// Level 44: 55649
// Level 45: 61512
// Level 46: 67983
// Level 47: 75127
// Level 48: 83014
// Level 49: 91721
// Level 50: 101334
// Level 51: 111946
// Level 52: 123661
// Level 53: 136594
// Level 54: 150873
// Level 55: 166637
// Level 56: 184040
// Level 57: 203254
// Level 58: 224467
// Level 59: 247887
// Level 60: 273743
// Level 61: 302289
// Level 62: 333805
// Level 63: 368600
// Level 64: 407016
// Level 65: 449429
// Level 66: 496255
// Level 67: 547954
// Level 68: 605033
// Level 69: 668052
// Level 70: 737629
// Level 71: 814447
// Level 72: 899261
// Level 73: 992898
// Level 74: 1096280
// Level 75: 1210420
// Level 76: 1336450
// Level 77: 1475580
// Level 78: 1629200
// Level 79: 1798810
// Level 80: 1986070
// Level 81: 2192830
// Level 82: 2421090
// Level 83: 2673120
// Level 84: 2951370
// Level 85: 3258590
// Level 86: 3597800
// Level 87: 3972300
// Level 88: 4385790
// Level 89: 4842300
// Level 90: 5346340
// Level 91: 5902820
// Level 92: 6517240
// Level 93: 7195640
// Level 94: 7944620
// Level 95: 8771580
// Level 96: 9684580
// Level 97: 10692600
// Level 98: 11805600
// Level 99: 13034400
// Level 100: 14391200