$MRPGClass_Swordsman = new ScriptObject(MRPGClasses) {
	class = "Swordsman";
	armor = 3;
	armorPerLevel = "1 + 0.25 * x";
	resist = 3;
	resistPerLevel = "0.5 + 0.25 * x";
	maxDamage = 120;
	healthPerLevel = "x * 5 + 5";
};

$MRPGClass_Bowman = new ScriptObject(MRPGClasses) {
	class = "Bowman";
	armor = 1;
	armorPerLevel = "0.5";
	resist = 1;
	resistPerLevel = "0.5";
	maxDamage = 100;
	healthPerLevel = "5 + x";
};

$MRPGClass_Gunman = new ScriptObject(MRPGClasses) {
	class = "Gunman";
	armor = 1;
	armorPerLevel = "0.75";
	resist = 1;
	resistPerLevel = "0.75";
	maxDamage = 80;
	healthPerLevel = "5";
};

$MRPGClass_Mage = new ScriptObject(MRPGClasses) {
	class = "Mage";
	armor = 0;
	armorPerLevel = "0.5";
	resist = 2;
	resistPerLevel = "1";
	maxDamage = 80;
	healthPerLevel = "5";
};

$MRPGClass_Healer = new ScriptObject(MRPGClasses) {
	class = "Healer";
	armor = 1;
	armorPerLevel = "0.5";
	resist = 1;
	resistPerLevel = "0.5";
	maxDamage = 80;
	healthPerLevel = "10";
};