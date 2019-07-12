echo("");
echo("");
echo("----Executing libraries----");
exec("./lib/Support_RPGDamage.cs");
exec("./lib/Support_Levels.cs");
exec("./lib/Support_AmmoGuns.cs");

echo("");
echo("");
echo("----Executing utils----");
exec("./util/reloadingGuns.cs");
exec("./util/getClosestObjectToPoint.cs");
exec("./util/getXFromObject.cs");
exec("./util/smallcat.cs");

echo("");
echo("");
echo("----Executing core files----");
exec("./core/exec.cs");