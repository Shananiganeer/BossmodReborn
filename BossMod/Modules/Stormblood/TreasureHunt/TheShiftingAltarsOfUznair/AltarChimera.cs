namespace BossMod.Stormblood.TreasureHunt.ShiftingAltarsOfUznair.AltarChimera;

public enum OID : uint
{
    Boss = 0x2539, //R=5.92
    BossAdd = 0x256A, //R=2.07
    IceVoidzone = 0x1E8D9C,
    BonusAddAltarMatanga = 0x2545, // R3.42
    Helper = 0x233C
}

public enum AID : uint
{
    AutoAttack = 870, // Boss->player, no cast, single-target
    AutoAttack2 = 872, // BonusAddAltarMatanga->player, no cast, single-target

    AutoAttack3 = 6499, // BossAdd->player, no cast, single-target
    TheScorpionsSting = 13393, // Boss->self, 3.5s cast, range 6+R 90-degree cone
    TheRamsVoice = 13394, // Boss->self, 5.0s cast, range 4+R circle, interruptible, deep freeze + frostbite
    TheLionsBreath = 13392, // Boss->self, 3.5s cast, range 6+R 120-degree cone, burn
    LanguorousGaze = 13742, // BossAdd->self, 3.0s cast, range 6+R 90-degree cone
    TheRamsKeeper = 13396, // Boss->location, 3.5s cast, range 6 circle, voidzone
    TheDragonsVoice = 13395, // Boss->self, 5.0s cast, range 8-30 donut, interruptible, paralaysis
    unknown = 9636, // BonusAddAltarMatanga->self, no cast, single-target
    Spin = 8599, // BonusAddAltarMatanga->self, no cast, range 6+R 120-degree cone
    RaucousScritch = 8598, // BonusAddAltarMatanga->self, 2.5s cast, range 5+R 120-degree cone
    Hurl = 5352, // BonusAddAltarMatanga->location, 3.0s cast, range 6 circle
    Telega = 9630 // BonusAdds->self, no cast, single-target, bonus add disappear
}

public enum IconID : uint
{
    Baitaway = 23 // player
}

class TheScorpionsSting(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TheScorpionsSting), new AOEShapeCone(11.92f, 45.Degrees()));
class TheRamsVoice(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TheRamsVoice), new AOEShapeCircle(9.92f));
class TheRamsVoiceHint(BossModule module) : Components.CastInterruptHint(module, ActionID.MakeSpell(AID.TheRamsVoice));
class TheLionsBreath(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TheLionsBreath), new AOEShapeCone(11.92f, 60.Degrees()));
class LanguorousGaze(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.LanguorousGaze), new AOEShapeCone(8.07f, 45.Degrees()));
class TheDragonsVoice(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.TheDragonsVoice), new AOEShapeDonut(8, 30));
class TheDragonsVoiceHint(BossModule module) : Components.CastInterruptHint(module, ActionID.MakeSpell(AID.TheDragonsVoice));
class TheRamsKeeper(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.TheRamsKeeper), 6);

class TheRamsKeeperBait(BossModule module) : Components.GenericBaitAway(module)
{
    private static readonly AOEShapeCircle circle = new(6);

    public override void OnEventIcon(Actor actor, uint iconID)
    {
        if (iconID == (uint)IconID.Baitaway)
            CurrentBaits.Add(new(actor, actor, circle));
    }

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID == AID.TheRamsKeeper)
            CurrentBaits.Clear();
    }

    public override void AddAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        base.AddAIHints(slot, actor, assignment, hints);
        if (CurrentBaits.Any(x => x.Target == actor))
            hints.AddForbiddenZone(ShapeDistance.Circle(Module.Center, 17.5f));
    }

    public override void AddHints(int slot, Actor actor, TextHints hints)
    {
        base.AddHints(slot, actor, hints);
        if (CurrentBaits.Any(x => x.Target == actor))
            hints.Add("Bait away!");
    }
}

class IceVoidzone(BossModule module) : Components.PersistentVoidzone(module, 6, m => m.Enemies(OID.IceVoidzone).Where(z => z.EventState != 7));
class RaucousScritch(BossModule module) : Components.SelfTargetedAOEs(module, ActionID.MakeSpell(AID.RaucousScritch), new AOEShapeCone(8.42f, 30.Degrees()));
class Hurl(BossModule module) : Components.LocationTargetedAOEs(module, ActionID.MakeSpell(AID.Hurl), 6);
class Spin(BossModule module) : Components.Cleave(module, ActionID.MakeSpell(AID.Spin), new AOEShapeCone(9.42f, 60.Degrees()), (uint)OID.BonusAddAltarMatanga);

class ChimeraStates : StateMachineBuilder
{
    public ChimeraStates(BossModule module) : base(module)
    {
        TrivialPhase()
            .ActivateOnEnter<TheScorpionsSting>()
            .ActivateOnEnter<TheRamsVoice>()
            .ActivateOnEnter<TheRamsVoiceHint>()
            .ActivateOnEnter<TheDragonsVoice>()
            .ActivateOnEnter<TheDragonsVoiceHint>()
            .ActivateOnEnter<TheLionsBreath>()
            .ActivateOnEnter<LanguorousGaze>()
            .ActivateOnEnter<TheRamsKeeper>()
            .ActivateOnEnter<TheRamsKeeperBait>()
            .ActivateOnEnter<IceVoidzone>()
            .ActivateOnEnter<Hurl>()
            .ActivateOnEnter<RaucousScritch>()
            .ActivateOnEnter<Spin>()
            .Raw.Update = () => module.Enemies(OID.Boss).All(e => e.IsDead) && module.Enemies(OID.BossAdd).All(e => e.IsDead) && module.Enemies(OID.BonusAddAltarMatanga).All(e => e.IsDead);
    }
}

[ModuleInfo(BossModuleInfo.Maturity.Verified, Contributors = "Malediktus", GroupType = BossModuleInfo.GroupType.CFC, GroupID = 586, NameID = 7591)]
public class Chimera(WorldState ws, Actor primary) : BossModule(ws, primary, new(100, 100), new ArenaBoundsCircle(19))
{
    protected override void DrawEnemies(int pcSlot, Actor pc)
    {
        Arena.Actor(PrimaryActor);
        Arena.Actors(Enemies(OID.BossAdd), Colors.Object);
        Arena.Actors(Enemies(OID.BonusAddAltarMatanga), Colors.Vulnerable);
    }

    protected override void CalculateModuleAIHints(int slot, Actor actor, PartyRolesConfig.Assignment assignment, AIHints hints)
    {
        foreach (var e in hints.PotentialTargets)
        {
            e.Priority = (OID)e.Actor.OID switch
            {
                OID.BonusAddAltarMatanga => 3,
                OID.BossAdd => 2,
                OID.Boss => 1,
                _ => 0
            };
        }
    }
}
