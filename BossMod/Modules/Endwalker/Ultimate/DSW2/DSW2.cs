﻿using System.Linq;

namespace BossMod.Endwalker.Ultimate.DSW2
{
    class P2AscalonMercy : CommonComponents.SelfTargetedAOE
    {
        public P2AscalonMercy() : base(ActionID.MakeSpell(AID.AscalonsMercyConcealedAOE), new AOEShapeCone(50, 15.Degrees())) { }
    }

    class P2UltimateEnd : CommonComponents.CastCounter
    {
        public P2UltimateEnd() : base(ActionID.MakeSpell(AID.UltimateEndAOE)) { }
    }

    [PrimaryActorOID((uint)OID.BossP2)]
    public class DSW2 : BossModule
    {
        private Actor? _bossP3;
        public Actor? BossP2() => PrimaryActor;
        public Actor? BossP3() => _bossP3;

        public DSW2(BossModuleManager manager, Actor primary)
            : base(manager, primary, new ArenaBoundsCircle(new(100, 100), 22))
        {
            StateMachine = new DSW2States(this).Build();
        }

        protected override void UpdateModule()
        {
            // TODO: this is an ugly hack, think how multi-actor fights can be implemented without it...
            // the problem is that on wipe, any actor can be deleted and recreated in the same frame
            if (_bossP3 == null && StateMachine?.ActivePhaseIndex == 1)
                _bossP3 = Enemies(OID.BossP3).FirstOrDefault();
        }

        protected override void DrawArenaForegroundPre(int pcSlot, Actor pc)
        {
            //Arena.Actor(Enemies(OID.SerJanlenoux).FirstOrDefault(), 0xffffffff);
            //Arena.Actor(Enemies(OID.SerVellguine).FirstOrDefault(), 0xff0000ff);
            //Arena.Actor(Enemies(OID.SerPaulecrain).FirstOrDefault(), 0xff00ff00);
            //Arena.Actor(Enemies(OID.SerIgnasse).FirstOrDefault(), 0xffff0000);
            //Arena.Actor(Enemies(OID.SerHermenost).FirstOrDefault(), 0xff00ffff);
            //Arena.Actor(Enemies(OID.SerGuerrique).FirstOrDefault(), 0xffff00ff);
            //Arena.Actor(Enemies(OID.SerHaumeric).FirstOrDefault(), 0xffffff00);
            //Arena.Actor(Enemies(OID.SerNoudenet).FirstOrDefault(), 0xffffff80);
            //Arena.Actor(Enemies(OID.SerZephirin).FirstOrDefault(), 0xff8080ff);
            //Arena.Actor(Enemies(OID.SerAdelphel).FirstOrDefault(), 0xff80ff80);
            //Arena.Actor(Enemies(OID.SerGrinnaux).FirstOrDefault(), 0xffff8080);
            //Arena.Actor(Enemies(OID.SerCharibert).FirstOrDefault(), 0xff80ffff);
        }

        protected override void DrawArenaForegroundPost(int pcSlot, Actor pc)
        {
            if (!PrimaryActor.IsDestroyed)
                Arena.Actor(PrimaryActor, ArenaColor.Enemy);
            Arena.Actor(_bossP3, ArenaColor.Enemy);
            //Arena.AddLine(PrimaryActor.Position, PrimaryActor.Position + GeometryUtils.DirectionToVec3(PrimaryActor.Rotation) * 5, ArenaColor.Enemy);
        }
    }
}