$MRPGClass_Swordsman = new ScriptObject(MRPGClasses) {
	class = "Swordsman";
	armor = 3;
	armorPerLevel = "x";
	resist = 3;
	resistPerLevel = "0.5 * x";
	maxDamage = 120;
	healthPerLevel = "x * 10";
};

$MRPGClass_Bowman = new ScriptObject(MRPGClasses) {
	class = "Bowman";
	armor = 1;
	armorPerLevel = "0.5 * x";
	resist = 1;
	resistPerLevel = "0.5 * x";
	maxDamage = 100;
	healthPerLevel = "x * 10";
};

$MRPGClass_Gunman = new ScriptObject(MRPGClasses) {
	class = "Gunman";
	armor = 1;
	armorPerLevel = "x";
	resist = 1;
	resistPerLevel = "x";
	maxDamage = 80;
	healthPerLevel = "x * 5";
};