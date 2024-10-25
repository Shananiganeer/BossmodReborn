namespace BossMod.Endwalker.Trial.T07Zeromus;

// note: apparently there's a slight overlap between aoes in the center, which looks ugly, but at least that's the truth...
class VisceralWhirl(BossModule module) : Components.GenericAOEs(module)
{
    private readonly List<AOEInstance> _aoes = [];

    private static readonly AOEShapeRect _shapeNormal = new(29, 14);
    private static readonly AOEShapeRect _shapeOffset = new(60, 14);

    public bool Active => _aoes.Count > 0;

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _aoes;

    public override void OnCastStarted(Actor caster, ActorCastInfo spell)
    {
        switch ((AID)spell.Action.ID)
        {
            case AID.VisceralWhirlRAOE1:
            case AID.VisceralWhirlLAOE1:
                _aoes.Add(new(_shapeNormal, caster.Position, spell.Rotation, Module.CastFinishAt(spell)));
                break;
            case AID.VisceralWhirlRAOE2:
                _aoes.Add(new(_shapeOffset, caster.Position + _shapeOffset.HalfWidth * spell.Rotation.ToDirection().OrthoL(), spell.Rotation, Module.CastFinishAt(spell)));
                break;
            case AID.VisceralWhirlLAOE2:
                _aoes.Add(new(_shapeOffset, caster.Position + _shapeOffset.HalfWidth * spell.Rotation.ToDirection().OrthoR(), spell.Rotation, Module.CastFinishAt(spell)));
                break;
        }
    }

    public override void OnCastFinished(Actor caster, ActorCastInfo spell)
    {
        if ((AID)spell.Action.ID is AID.VisceralWhirlRAOE1 or AID.VisceralWhirlRAOE2 or AID.VisceralWhirlLAOE1 or AID.VisceralWhirlLAOE2)
            _aoes.RemoveAll(a => a.Rotation.AlmostEqual(spell.Rotation, 0.05f));
    }
}

class VoidBio(BossModule module) : Components.GenericAOEs(module)
{
    private readonly IReadOnlyList<Actor> _bubbles = module.Enemies(OID.ToxicBubble);

    private static readonly AOEShapeCircle _shape = new(3.5f);

    public override IEnumerable<AOEInstance> ActiveAOEs(int slot, Actor actor) => _bubbles.Where(actor => !actor.IsDead).Select(b => new AOEInstance(_shape, b.Position));

    public int GetBubbleCount() => _bubbles.Count(actor => !actor.IsDead);
}

class DarkBeckons(BossModule module) : Components.UniformStackSpread(module, 6, 0)
{
    public override void OnEventIcon(Actor actor, uint iconID)
    {
        if ((IconID)iconID is IconID.DarkBeckons)
            AddStack(actor, WorldState.FutureTime(5.1f));
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.DarkBeckons)
            Stacks.Clear();
    }
}

class DarkDivides(BossModule module) : Components.UniformStackSpread(module, 0, 5)
{
    public override void OnStatusGain(Actor actor, ActorStatus status)
    {
        if ((SID)status.ID == SID.DivisiveDark)
            AddSpread(actor, status.ExpireAt);
    }

    public override void OnEventCast(Actor caster, ActorCastEvent spell)
    {
        if ((AID)spell.Action.ID == AID.DarkDivides)
            Spreads.Clear();
    }
}
