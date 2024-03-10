﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace BossMod.Endwalker.FATE.Chi
{
    public enum OID : uint
    {
        Boss = 0x34CB, // R=16.0
        Helper1 = 0x34CC, //R=0.5
        Helper2 = 0x34CD, //R=0.5
        Helper3 = 0x361E, //R=0.5
        Helper4 = 0x364C, //R=0.5
        Helper5 = 0x364D, //R=0.5
        Helper6 = 0x3505, //R=0.5
    };

    public enum AID : uint
    {
        AutoAttack = 25952, // Boss->player, no cast, single-target
        AssaultCarapace = 25954, // Boss->self, 5,0s cast, range 120 width 32 rect
        AssaultCarapace2 = 25173, // Boss->self, 8,0s cast, range 120 width 32 rect
        Carapace_RearGuns2dot0A = 25958, // Boss->self, 8,0s cast, range 120 width 32 rect
        Carapace_ForeArms2dot0A = 25957, // Boss->self, 8,0s cast, range 120 width 32 rect
        AssaultCarapace3 = 25953, // Boss->self, 5,0s cast, range 16-60 donut
        Carapace_ForeArms2dot0B = 25955, // Boss->self, 8,0s cast, range 16-60 donut
        Carapace_RearGuns2dot0B = 25956, // Boss->self, 8,0s cast, range 16-60 donut
        ForeArms = 25959, // Boss->self, 6,0s cast, range 45 180-degree cone
        ForeArms2 = 26523, // Boss->location, 6,0s cast, range 45 180-degree cone
        ForeArms2dot0 = 25961, // Boss->self, no cast, range 45 180-degree cone
        RearGuns2dot0 = 25964, // Boss->self, no cast, range 45 180-degree cone
        RearGuns = 25962, // Boss->self, 6,0s cast, range 45 180-degree cone
        RearGuns2 = 26524, // Boss->location, 6,0s cast, range 45 180-degree cone
        RearGuns_ForeArms2dot0 = 25963, // Boss->self, 6,0s cast, range 45 180-degree cone
        ForeArms_RearGuns2dot0 = 25960, // Boss->self, 6,0s cast, range 45 180-degree cone
        Hellburner = 25971, // Boss->self, no cast, single-target, circle tankbuster
        Hellburner2 = 25972, // Helper1->players, 5,0s cast, range 5 circle
        FreeFallBombs = 25967, // Boss->self, no cast, single-target
        FreeFallBombs2 = 25968, // Helper1->location, 3,0s cast, range 6 circle
        MissileShower = 25969, // Boss->self, 4,0s cast, single-target
        MissileShower2 = 25970, // Helper2->self, no cast, range 30 circle
        Teleport = 25155, // Boss->location, no cast, single-target, boss teleports mid
        BunkerBuster = 25975, // Boss->self, 3,0s cast, single-target
        BunkerBuster2 = 25101, // Helper3->self, 10,0s cast, range 20 width 20 rect
        BunkerBuster3 = 25976, // Helper6->self, 12,0s cast, range 20 width 20 rect
        BouncingBomb = 27484, // Boss->self, 3,0s cast, single-target
        BouncingBomb2 = 27485, // Helper4->self, 5,0s cast, range 20 width 20 rect
        BouncingBomb3 = 27486, // Helper5->self, 1,0s cast, range 20 width 20 rect
        ThermobaricExplosive = 25965, // Boss->self, 3,0s cast, single-target
        ThermobaricExplosive2 = 25966, // Helper1->location, 10,0s cast, range 55 circle, damage fall off AOE
    };

    public enum IconID : uint
    {
        Tankbuster = 243, // player
        BunkerbusterTelegraph = 292, // Helper3/Helper6
    };

   class Bunkerbuster : Components.GenericAOEs
    {
        private List<Actor> _casters = new();
        private DateTime _activation1;
        private DateTime _activation2;

        private static readonly AOEShapeRect rect = new(10, 10, 10);

        public override IEnumerable<AOEInstance> ActiveAOEs(BossModule module, int slot, Actor actor)
        {
            if (_casters.Count >= 3)
            {
                yield return new(rect, _casters[0].Position, _casters[0].Rotation, _activation1, ArenaColor.Danger);
                yield return new(rect, _casters[1].Position, _casters[1].Rotation, _activation1, ArenaColor.Danger);
                yield return new(rect, _casters[2].Position, _casters[2].Rotation, _activation1, ArenaColor.Danger);
            }
            if (_casters.Count >= 6)
            {
                yield return new(rect, _casters[3].Position, _casters[3].Rotation, _activation2);
                yield return new(rect, _casters[4].Position, _casters[4].Rotation, _activation2);
                yield return new(rect, _casters[5].Position, _casters[5].Rotation, _activation2);
            }
        }

        public override void OnCastStarted(BossModule module, Actor caster, ActorCastInfo spell)
        {
            if ((AID)spell.Action.ID is AID.BunkerBuster2)
            {
                if (_activation1 == default)
                {
                    _activation1 = spell.NPCFinishAt;
                    _activation2 = _activation1.AddSeconds(1.9f);
                }
            }
            if ((AID)spell.Action.ID is AID.BunkerBuster3)
            {
                if (_activation1 == default)
                {
                    _activation1 = spell.NPCFinishAt;
                    _activation2 = _activation1.AddSeconds(1.9f);
                }
            }
        }

        public override void OnCastFinished(BossModule module, Actor caster, ActorCastInfo spell)
        {
            if ((AID)spell.Action.ID is AID.BunkerBuster2 or AID.BunkerBuster3)
            {
                ++NumCasts;
                if (_casters.Count > 0)
                    _casters.Remove(caster);
                if (NumCasts == 3)
                {
                    _activation1 = _activation1.AddSeconds(1.9f);
                    _activation2 = _activation1.AddSeconds(1.9f);
                    NumCasts = 0;
                }
            }
        }

        public override void OnEventIcon(BossModule module, Actor actor, uint iconID)
        {
            if (iconID == (uint)IconID.BunkerbusterTelegraph)
                _casters.Add(actor);
        }

        public override void Update(BossModule module)
        {
            if (_casters.Count == 0)
                _activation1 = default;
        }
    }

   class BouncingBomb : Components.GenericAOEs
    {
        private List<Actor> _casters = new();
        private DateTime _activation1;
        private DateTime _activation2;
        private int bombcount;

        private static readonly AOEShapeRect rect = new(10, 10, 10);

        public override IEnumerable<AOEInstance> ActiveAOEs(BossModule module, int slot, Actor actor)
        {
            if (bombcount == 1)
            {
                if (_casters.Count >= 1 && NumCasts == 0)
                    yield return new(rect, _casters[0].Position, _casters[0].Rotation, _activation1, ArenaColor.Danger);
                if (_casters.Count >= 4 && NumCasts == 0)
                {
                    yield return new(rect, _casters[1].Position, _casters[1].Rotation, _activation2);
                    yield return new(rect, _casters[2].Position, _casters[2].Rotation, _activation2);
                    yield return new(rect, _casters[3].Position, _casters[3].Rotation, _activation2);
                }
                if (_casters.Count >= 3 && NumCasts == 1)
                {
                    yield return new(rect, _casters[0].Position, _casters[0].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[1].Position, _casters[1].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[2].Position, _casters[2].Rotation, _activation1, ArenaColor.Danger);
                }
                if (_casters.Count >= 8 && NumCasts == 1)
                {
                    yield return new(rect, _casters[3].Position, _casters[3].Rotation, _activation2);
                    yield return new(rect, _casters[4].Position, _casters[4].Rotation, _activation2);
                    yield return new(rect, _casters[5].Position, _casters[5].Rotation, _activation2);
                    yield return new(rect, _casters[6].Position, _casters[6].Rotation, _activation2);
                    yield return new(rect, _casters[7].Position, _casters[7].Rotation, _activation2);
                }
                if (_casters.Count >= 5 && NumCasts == 4)
                {
                    yield return new(rect, _casters[0].Position, _casters[0].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[1].Position, _casters[1].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[2].Position, _casters[2].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[3].Position, _casters[3].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[4].Position, _casters[4].Rotation, _activation1, ArenaColor.Danger);
                }
            }
            if (bombcount == 2)
            {
                if (_casters.Count >= 2 && NumCasts == 0)
                {
                    yield return new(rect, _casters[0].Position, _casters[0].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[1].Position, _casters[1].Rotation, _activation1, ArenaColor.Danger);
                }
                if (_casters.Count >= 7 && NumCasts == 0)
                {
                    yield return new(rect, _casters[2].Position, _casters[2].Rotation, _activation2);
                    yield return new(rect, _casters[3].Position, _casters[3].Rotation, _activation2);
                    yield return new(rect, _casters[4].Position, _casters[4].Rotation, _activation2);
                    yield return new(rect, _casters[5].Position, _casters[5].Rotation, _activation2);
                    yield return new(rect, _casters[6].Position, _casters[6].Rotation, _activation2);
                }
                if (_casters.Count >= 5 && NumCasts == 2)
                {
                    yield return new(rect, _casters[0].Position, _casters[0].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[1].Position, _casters[1].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[2].Position, _casters[2].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[3].Position, _casters[3].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[4].Position, _casters[4].Rotation, _activation1, ArenaColor.Danger);
                }
                if (_casters.Count >= 13 && NumCasts == 2)
                {
                    yield return new(rect, _casters[5].Position, _casters[5].Rotation, _activation2);
                    yield return new(rect, _casters[6].Position, _casters[6].Rotation, _activation2);
                    yield return new(rect, _casters[7].Position, _casters[7].Rotation, _activation2);
                    yield return new(rect, _casters[8].Position, _casters[8].Rotation, _activation2);
                    yield return new(rect, _casters[9].Position, _casters[9].Rotation, _activation2);
                    yield return new(rect, _casters[10].Position, _casters[10].Rotation, _activation2);
                    yield return new(rect, _casters[11].Position, _casters[11].Rotation, _activation2);
                    yield return new(rect, _casters[12].Position, _casters[12].Rotation, _activation2);
                }
                if (_casters.Count >= 8 && NumCasts == 7)
                {
                    yield return new(rect, _casters[0].Position, _casters[0].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[1].Position, _casters[1].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[2].Position, _casters[2].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[3].Position, _casters[3].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[4].Position, _casters[4].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[5].Position, _casters[5].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[6].Position, _casters[6].Rotation, _activation1, ArenaColor.Danger);
                    yield return new(rect, _casters[7].Position, _casters[7].Rotation, _activation1, ArenaColor.Danger);
                }
            }
        }

        public override void OnActorCreated(BossModule module, Actor actor)
        {
            if ((OID)actor.OID == OID.Helper4)
            {
                _activation1 = module.WorldState.CurrentTime.AddSeconds(8);
                ++bombcount;
            }
            if ((OID)actor.OID is OID.Helper4 or OID.Helper5)
                _casters.Add(actor);
        }

        public override void OnCastStarted(BossModule module, Actor caster, ActorCastInfo spell)
        {
            if ((AID)spell.Action.ID is AID.BouncingBomb2)
            {
                _activation1 = spell.NPCFinishAt;
                _activation2 = _activation1.AddSeconds(2.8f);
            }
        }

        public override void OnCastFinished(BossModule module, Actor caster, ActorCastInfo spell)
        {
            if ((AID)spell.Action.ID is AID.BouncingBomb2 or AID.BouncingBomb3)
            {
                ++NumCasts;
                if (_casters.Count > 0)
                    _casters.Remove(caster);
                if ((bombcount == 1 && NumCasts is 1 or 4) || (bombcount == 2 && NumCasts is 2 or 7))
                {
                    _activation1 = _activation1.AddSeconds(2.8f);
                    _activation2 = _activation1.AddSeconds(2.8f);
                }
            }
        }

        public override void Update(BossModule module)
        {
            if (_casters.Count == 0)
            {
                _activation1 = default;
                bombcount = 0;
                NumCasts = 0;
            }
        }
    }

    class Combos : Components.GenericAOEs
    {
        private static readonly AOEShapeCone cone = new (45, 90.Degrees());
        private static readonly AOEShapeDonut donut = new (16, 60);
        private static readonly AOEShapeRect rect = new (120, 16, 120);
        private DateTime _activation1;
        private DateTime _activation2;
        private AOEShape? _shape1;
        private AOEShape? _shape2;
        private bool offset;
        private Angle _rotation;

        public override IEnumerable<AOEInstance> ActiveAOEs(BossModule module, int slot, Actor actor)
        {
            if (_shape1 != null && _shape2 != null)
            {
                if (NumCasts == 0)
                {
                    yield return new(_shape1, module.PrimaryActor.Position, _rotation, activation: _activation1, ArenaColor.Danger);
                    if (!offset)
                        yield return new(_shape2, module.PrimaryActor.Position, _rotation, activation: _activation2);
                    else
                        yield return new(_shape2, module.PrimaryActor.Position, _rotation + 180.Degrees(), activation: _activation2);
                }
                if (NumCasts == 1)
                {
                    if (!offset)
                        yield return new(_shape2, module.PrimaryActor.Position, _rotation, activation: _activation2, ArenaColor.Danger);
                    else
                        yield return new(_shape2, module.PrimaryActor.Position, _rotation + 180.Degrees(), activation: _activation2, ArenaColor.Danger);
                }
            }
        }

        public override void OnCastStarted(BossModule module, Actor caster, ActorCastInfo spell)
        {
            switch ((AID)spell.Action.ID)
            {
                case AID.Carapace_ForeArms2dot0A:
                    _shape1 = rect;
                    _shape2 = cone;
                    break;
                case AID.Carapace_ForeArms2dot0B:
                    _shape1 = donut;
                    _shape2 = cone;
                    break;
                case AID.Carapace_RearGuns2dot0A:
                    _shape1 = rect;
                    _shape2 = cone;
                    offset = true;
                    break;
                case AID.Carapace_RearGuns2dot0B:
                    _shape1 = donut;
                    _shape2 = cone;
                    offset = true;
                    break;
                case AID.RearGuns_ForeArms2dot0:
                case AID.ForeArms_RearGuns2dot0:
                    _shape1 = _shape2 = cone;
                    offset = true;
                    break;
            }            
            if ((AID)spell.Action.ID is AID.Carapace_ForeArms2dot0A or AID.Carapace_ForeArms2dot0B or AID.Carapace_RearGuns2dot0A or AID.Carapace_RearGuns2dot0B or AID.RearGuns_ForeArms2dot0 or AID.ForeArms_RearGuns2dot0)
            {
                _activation1 = spell.NPCFinishAt;
                _activation2 = spell.NPCFinishAt.AddSeconds(3.1f);
                _rotation = spell.Rotation;
            }
        }

        public override void OnCastFinished(BossModule module, Actor caster, ActorCastInfo spell)
        {     
            if ((AID)spell.Action.ID is AID.Carapace_ForeArms2dot0A or AID.Carapace_ForeArms2dot0B or AID.Carapace_RearGuns2dot0A or AID.Carapace_RearGuns2dot0B or AID.RearGuns_ForeArms2dot0 or AID.ForeArms_RearGuns2dot0)
                ++NumCasts;
        }

        public override void OnEventCast(BossModule module, Actor caster, ActorCastEvent spell)
        {     
            if ((AID)spell.Action.ID is AID.RearGuns2dot0 or AID.ForeArms2dot0)
            {
                NumCasts = 0;
                offset = false;
                _shape1 = null;
                _shape2 = null;
            }
        }
    }

    class Hellburner : Components.BaitAwayCast
    {
        public Hellburner() : base(ActionID.MakeSpell(AID.Hellburner2), new AOEShapeCircle(5), true) { }
    }

    class HellburnerHint : Components.SingleTargetCast
    {
        public HellburnerHint() : base(ActionID.MakeSpell(AID.Hellburner2)) { }
    }

    class MissileShower : Components.SingleTargetCast
    {
        public MissileShower() : base(ActionID.MakeSpell(AID.MissileShower), "Raidwide x2") { }
    }

    class ThermobaricExplosive : Components.LocationTargetedAOEs
    {
        public ThermobaricExplosive() : base(ActionID.MakeSpell(AID.ThermobaricExplosive2), 25) { }
    }

    class AssaultCarapace : Components.SelfTargetedAOEs
    {
        public AssaultCarapace() : base(ActionID.MakeSpell(AID.AssaultCarapace), new AOEShapeRect(120, 16, 120)) { }
    }

    class AssaultCarapace2 : Components.SelfTargetedAOEs
    {
        public AssaultCarapace2() : base(ActionID.MakeSpell(AID.AssaultCarapace2), new AOEShapeRect(120, 16, 120)) { }
    }

    class AssaultCarapace3 : Components.SelfTargetedAOEs
    {
        public AssaultCarapace3() : base(ActionID.MakeSpell(AID.AssaultCarapace3), new AOEShapeDonut(16, 60)) { }
    }

    class ForeArms : Components.SelfTargetedAOEs
    {
        public ForeArms() : base(ActionID.MakeSpell(AID.ForeArms), new AOEShapeCone(45, 90.Degrees())) { }
    }

    class ForeArms2 : Components.SelfTargetedAOEs
    {
        public ForeArms2() : base(ActionID.MakeSpell(AID.ForeArms2), new AOEShapeCone(45, 90.Degrees())) { }
    }

    class RearGuns : Components.SelfTargetedAOEs
    {
        public RearGuns() : base(ActionID.MakeSpell(AID.RearGuns), new AOEShapeCone(45, 90.Degrees())) { }
    }

    class RearGuns2 : Components.SelfTargetedAOEs
    {
        public RearGuns2() : base(ActionID.MakeSpell(AID.RearGuns2), new AOEShapeCone(45, 90.Degrees())) { }
    }

    class FreeFallBombs : Components.LocationTargetedAOEs
    {
        public FreeFallBombs() : base(ActionID.MakeSpell(AID.FreeFallBombs2), 6) { }
    }

    class ChiStates : StateMachineBuilder
    {
        public ChiStates(BossModule module) : base(module)
        {
            TrivialPhase()
                .ActivateOnEnter<AssaultCarapace>()
                .ActivateOnEnter<AssaultCarapace2>()
                .ActivateOnEnter<AssaultCarapace3>()
                .ActivateOnEnter<Combos>()
                .ActivateOnEnter<ForeArms>()
                .ActivateOnEnter<ForeArms2>()
                .ActivateOnEnter<RearGuns>()
                .ActivateOnEnter<RearGuns2>()
                .ActivateOnEnter<Hellburner>()
                .ActivateOnEnter<HellburnerHint>()
                .ActivateOnEnter<FreeFallBombs>()
                .ActivateOnEnter<ThermobaricExplosive>()
                .ActivateOnEnter<Bunkerbuster>()
                .ActivateOnEnter<BouncingBomb>()
                .ActivateOnEnter<MissileShower>();
        }
    }

    [ModuleInfo(FateID = 1855)]
    public class Chi : BossModule
    {
        public Chi(WorldState ws, Actor primary) : base(ws, primary, new ArenaBoundsSquare(new(650, 0), 30)) { }
    }
}
