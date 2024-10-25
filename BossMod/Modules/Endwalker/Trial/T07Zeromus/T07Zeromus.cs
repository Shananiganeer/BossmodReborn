namespace BossMod.Endwalker.Trial.T07Zeromus;

class AbyssalNox(BossModule module) : Components.RaidwideInstant(module, ActionID.MakeSpell(AID.AbyssalNox), 0, "Heal to full");
class AbyssalEchoes(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.AbyssalEchoes), new AOEShapeCircle(12), 5);
class BigBangPuddle(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.BigBangAOE), 5);
/* class BigBangSpread(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.BigBangSpread), 5);
class BigCrunchPuddle(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.BigCrunchAOE), 5);
class BigCrunchSpread(BossModule module) : Components.SpreadFromCastTargets(module, ActionID.MakeSpell(AID.BigCrunchSpread), 5); */

[ModuleInfo(BossModuleInfo.Maturity.WIP, Contributors = "SwagMode", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 964, NameID = 12586)]
public class T07Zeromus(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsSquare(20));