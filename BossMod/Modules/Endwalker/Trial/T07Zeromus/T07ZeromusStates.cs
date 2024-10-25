namespace BossMod.Endwalker.Trial.T07Zeromus;

class T07ZeromusStates : StateMachineBuilder
{
    public T07ZeromusStates(BossModule module) : base(module)
    {
        SimplePhase(0, Phase1, "P1")
            .Raw.Update = () => Module.PrimaryActor.IsDestroyed || Module.PrimaryActor.IsDead || Module.PrimaryActor.HPMP.CurHP <= 1 || (Module.PrimaryActor.CastInfo?.IsSpell(AID.RendTheRift) ?? false);
        DeathPhase(1, Phase2) // starts at around 25%, after current mechanic is resolved
            .Raw.Update = () => Module.PrimaryActor.IsDestroyed || Module.PrimaryActor.IsDead || Module.PrimaryActor.HPMP.CurHP <= 1 || (Module.PrimaryActor.CastInfo?.IsSpell(AID.Enrage) ?? false);
        DeathPhase(2, EnrageP2); // starts at around 660s after current mechanic is resolved
    }

    private void Phase1(uint id)
    {
        AbyssalNoxEchoesSableThread(id, 6.2f, false);
        VisceralWhirl(id + 0x10000, 5.3f);
        DarkMatter(id + 0x20000, 7.4f);
        FlareNox(id + 0x30000, 7.1f);
        VoidBioVisceralWhirl(id + 0x40000, 5.1f);
        BigBang(id + 0x50000, 5.1f);
        VisceralWhirlDarkBeckonsDivides(id + 0x60000, 5.3f);
        MeteorImpact(id + 0x70000, 0.9f);
        DarkMatter(id + 0x80000, 7.4f);
        BlackHole(id + 0x90000, 5.3f);
        VoidMeteor(id + 0xA0000, 10.2f);
        BlackHole(id + 0xB0000, 5.3f);
        BigCrunch(id + 0xC0000, 4.5f);
        AbyssalNoxEchoesSableThread(id + 0xD0000, 10.3f, true);
        VoidBioVisceralWhirl(id + 0xE0000, 3.9f);
        DarkMatter(id + 0xF0000, 7.1f);
        AbyssalNoxEchoesSableThread(id + 0x100000, 13.9f, true);
        VoidBioVisceralWhirl(id + 0x110000, 3.9f);
        DarkMatter(id + 0x120000, 7.1f);
        AbyssalNoxEchoesEnrage(id + 0x130000, 13.9f);
    }

    private void Phase2(uint id)
    {
        RendTheRift(id, 0);
        DimensionalSurgeNostalgia(id + 0x10000, 8.2f);
        FlowOfTheAbyss(id + 0x20000, 7.4f, false);
        FlowOfTheAbyss(id + 0x30000, 10.7f, true);
        DimensionalSurgeNostalgia(id + 0x40000, 7.7f);
        FlowOfTheAbyss(id + 0x50000, 7.4f, true);
        FlowOfTheAbyss(id + 0x60000, 10.7f, true);
        DimensionalSurgeNostalgia(id + 0x70000, 7.7f);
        // TODO: never seen stuff below...
        FlowOfTheAbyss(id + 0x80000, 7.4f, true);
        FlowOfTheAbyss(id + 0x90000, 10.7f, true);
        DimensionalSurgeNostalgia(id + 0xA0000, 7.7f);
        FlowOfTheAbyss(id + 0xB0000, 7.4f, true);
        FlowOfTheAbyss(id + 0xC0000, 10.7f, true);
        DimensionalSurgeNostalgia(id + 0xD0000, 7.7f);
        FlowOfTheAbyss(id + 0xE0000, 7.4f, true);
        FlowOfTheAbyss(id + 0xF0000, 10.7f, true);

        SimpleState(id + 0xFF0000, 10, "???");
    }

    private void EnrageP2(uint id)
    {
        Cast(id, AID.Enrage, 5, 10, "Enrage")
            .ActivateOnEnter<BigBangPuddle>(); // first puddle/spread starts at the same time, but the rest are slightly staggered
    }

    private void AbyssalNoxEchoesSableThread(uint id, float delay, bool second)
    {
        Cast(id, AID.AbyssalNox, delay, 5);
        ComponentCondition<AbyssalEchoes>(id + 0x1000, 0.1f, comp => comp.Casters.Count > 0)
            .ActivateOnEnter<AbyssalEchoes>()
            .ExecOnEnter<AbyssalEchoes>(comp => comp.MaxCasts = 0); // don't show aoes before doom, this is misleading
        ComponentCondition<AbyssalEchoes>(id + 0x1010, 5, comp => comp.Casters.Count > 5, "1 hp"); // dooms are slightly staggered apply around here
        ComponentCondition<AbyssalEchoes>(id + 0x1020, 11, comp => comp.NumCasts > 0, "Circles 1")
            .ExecOnEnter<AbyssalEchoes>(comp => comp.MaxCasts = 5);
        ComponentCondition<AbyssalEchoes>(id + 0x1030, 5, comp => comp.Casters.Count == 0, "Circles 2")
            .ActivateOnEnter<SableThread>() // second can very slightly overlap
            .DeactivateOnExit<AbyssalEchoes>();

        Cast(id + 0x2000, AID.SableThread, second ? 0 : 5.1f, 5);
        ComponentCondition<SableThread>(id + 0x2010, 0.7f, comp => comp.NumCasts > 0, "Wild charge start");
        ComponentCondition<SableThread>(id + 0x2020, second ? 8.9f : 7.5f, comp => comp.NumCasts >= (second ? 7 : 6), "Wild charge resolve")
            .DeactivateOnExit<SableThread>();
    }

    private void AbyssalNoxEchoesEnrage(uint id, float delay)
    {
        Cast(id, AID.AbyssalNox, delay, 5);
        ComponentCondition<AbyssalEchoes>(id + 0x1000, 0.1f, comp => comp.Casters.Count > 0)
            .ActivateOnEnter<AbyssalEchoes>()
            .ExecOnEnter<AbyssalEchoes>(comp => comp.MaxCasts = 0); // don't show aoes before doom, this is misleading
        ComponentCondition<AbyssalEchoes>(id + 0x1010, 5, comp => comp.Casters.Count > 5, "1 hp"); // dooms are slightly staggered apply around here
        ComponentCondition<AbyssalEchoes>(id + 0x1020, 11, comp => comp.NumCasts > 0, "Circles 1")
            .ExecOnEnter<AbyssalEchoes>(comp => comp.MaxCasts = 5);
        ComponentCondition<AbyssalEchoes>(id + 0x1030, 5, comp => comp.Casters.Count == 0, "Circles 2")
            .DeactivateOnExit<AbyssalEchoes>();
        Cast(id + 0x2000, AID.Enrage, 5, 10, "Enrage")
            .ActivateOnEnter<BigBangPuddle>() // first puddle/spread starts at the same time, but the rest are slightly staggered
            /* .ActivateOnEnter<BigBangSpread>() */;
    }

    private void DarkMatter(uint id, float delay)
    {
        CastStart(id, AID.DarkMatter, delay)
            .ActivateOnEnter<DarkMatter>();
        CastEnd(id + 1, 4);
        ComponentCondition<DarkMatter>(id + 2, delay, comp => comp.RemainingCasts <= 2, "Tankbuster 1")
            .SetHint(StateMachine.StateHint.Tankbuster);
        ComponentCondition<DarkMatter>(id + 3, 1.6f, comp => comp.RemainingCasts <= 1, "Tankbuster 2")
            .SetHint(StateMachine.StateHint.Tankbuster);
        ComponentCondition<DarkMatter>(id + 4, 1.6f, comp => comp.RemainingCasts <= 0, "Tankbuster 3")
            .DeactivateOnExit<DarkMatter>()
            .SetHint(StateMachine.StateHint.Tankbuster);
    }

    private State VisceralWhirl(uint id, float delay)
    {
        CastMulti(id, [AID.VisceralWhirlR, AID.VisceralWhirlL], delay, 8)
            .ActivateOnEnter<VisceralWhirl>();
        return ComponentCondition<VisceralWhirl>(id + 2, 0.8f, comp => !comp.Active, "Lines")
            .DeactivateOnExit<VisceralWhirl>();
    }

    private void VoidBioVisceralWhirl(uint id, float delay)
    {
        Cast(id, AID.VoidBio, delay, 5, "Bubbles spawn")
            .ActivateOnEnter<VoidBio>();
        VisceralWhirl(id + 0x10, 6.1f);
        ComponentCondition<VoidBio>(id + 0x20, 0.8f, comp => comp.GetBubbleCount() > 0)
            .DeactivateOnExit<VoidBio>();
    }

    private void VisceralWhirlDarkBeckonsDivides(uint id, float delay)
    {
        VisceralWhirl(id, 6.1f)
            .ActivateOnEnter<DarkBeckons>();

        ComponentCondition<DarkBeckons>(id + 0x10, 0.6f, comp => !comp.Active, "Stack")
            .DeactivateOnExit<DarkBeckons>();

        ComponentCondition<DarkDivides>(id + 0x20, 3.5f, comp => !comp.Active, "Spread")
            .ActivateOnEnter<DarkDivides>()
            .DeactivateOnExit<DarkDivides>();
    }

    /* private void VisceralWhirlChainsBombs(uint id, float delay)
    {
        CastStartMulti(id, [AID.VisceralWhirlR, AID.VisceralWhirlL], delay);
        ComponentCondition<BondsOfDarkness>(id + 1, 1.9f, comp => comp.NumTethers > 0, "Chains appear")
            .ActivateOnEnter<VisceralWhirl>()
            .ActivateOnEnter<BondsOfDarkness>(); // tethers have ~5s to be broken
        // +3.1s: acceleration bomb icons
        CastEnd(id + 3, 6, "Stay still") // TODO: check when exactly does the stillness check happen, add hint?
            .DeactivateOnExit<BondsOfDarkness>();
        // +0.5s: acceleration bomb debuffs expire
        ComponentCondition<VisceralWhirl>(id + 5, 0.8f, comp => !comp.Active, "Lines")
            .DeactivateOnExit<VisceralWhirl>();

        ComponentCondition<MiasmicBlast>(id + 0x10, 0.3f, comp => comp.Casters.Count > 0)
            .ActivateOnEnter<MiasmicBlast>();
        // +6.3s: spread icons
        ComponentCondition<MiasmicBlast>(id + 0x12, 8, comp => comp.NumCasts > 0, "Crosses")
            .DeactivateOnExit<MiasmicBlast>();
        ComponentCondition<DarkDivides>(id + 0x20, 3.5f, comp => !comp.Active, "Spread")
            .ActivateOnEnter<DarkDivides>()
            .DeactivateOnExit<DarkDivides>();
    } */

    /* private void Flare(uint id, float delay)
    {
        Cast(id, AID.Flare, delay, 7)
            .ActivateOnEnter<FlareTowers>();
        ComponentCondition<FlareTowers>(id + 2, 1, comp => comp.NumCasts > 0, "Towers")
            .ActivateOnEnter<FlareScald>()
            .DeactivateOnExit<FlareTowers>();

        ComponentCondition<ProminenceSpine>(id + 0x10, 2.1f, comp => comp.Casters.Count > 0, "AOEs at towers") // first scald happens at the same time
            .ActivateOnEnter<ProminenceSpine>();
        ComponentCondition<ProminenceSpine>(id + 0x20, 5, comp => comp.NumCasts > 0, "Lines")
            .DeactivateOnExit<FlareScald>()
            .DeactivateOnExit<ProminenceSpine>();
    } */

    private void FlareNox(uint id, float delay)
    {
        Cast(id, AID.Flare, delay, 7)
            .ActivateOnEnter<FlareTowers>();
        ComponentCondition<FlareTowers>(id + 2, 1, comp => comp.NumCasts > 0, "Towers")
            .ActivateOnEnter<FlareScald>()
            .DeactivateOnExit<FlareTowers>();

        CastStart(id + 0x10, AID.Nox, 1.1f)
            .ActivateOnEnter<Nox>();

        ComponentCondition<ProminenceSpine>(id + 0x11, 1, comp => comp.Casters.Count > 0, "AOEs at towers") // first scald happens at the same time
            .ActivateOnEnter<ProminenceSpine>();
        ComponentCondition<ProminenceSpine>(id + 0x20, 5, comp => comp.NumCasts > 0, "Lines")
            .DeactivateOnExit<FlareScald>()
            .DeactivateOnExit<ProminenceSpine>();

        ComponentCondition<Nox>(id + 0x30, 4, comp => comp.NumCasts > 0, "Chaser start");
        ComponentCondition<Nox>(id + 0x40, 6.3f, comp => comp.Chasers.Count == 0, "Chaser resolve")
            .DeactivateOnExit<Nox>();
    }

    private void BigBang(uint id, float delay)
    {
        Cast(id, AID.BigBang, delay, 10, "Raidwide")
            .ActivateOnEnter<BigBangPuddle>()
            .DeactivateOnExit<BigBangPuddle>()
            .SetHint(StateMachine.StateHint.Raidwide);
    }

    private void BigCrunch(uint id, float delay)
    {
        Cast(id, AID.BigCrunch, delay, 10, "Raidwide")
            /* .ActivateOnEnter<BigCrunchPuddle>() // first puddle/spread starts at the same time, but the rest are slightly staggered
            .ActivateOnEnter<BigCrunchSpread>()
            .DeactivateOnExit<BigCrunchPuddle>()
            .DeactivateOnExit<BigCrunchSpread>() */
            .SetHint(StateMachine.StateHint.Raidwide);
    }

    private void MeteorImpact(uint id, float delay)
    {
        Cast(id, AID.MeteorImpact, delay, 11);
        // +0.9s: first set bound
        ComponentCondition<MeteorImpactCharge>(id + 0x10, 2.7f, comp => comp.NumCasts >= 4, "Charges 1");
        // +5.4s: second set bound
        ComponentCondition<MeteorImpactCharge>(id + 0x20, 7.0f, comp => comp.NumCasts >= 8, "Charges 2")
            .DeactivateOnExit<MeteorImpactCharge>();
        ComponentCondition<MeteorImpactExplosion>(id + 0x30, 2.1f, comp => comp.Casters.Count > 0)
            .ActivateOnEnter<MeteorImpactExplosion>();
        ComponentCondition<MeteorImpactExplosion>(id + 0x31, 5, comp => comp.NumCasts > 0, "Explosions")
            .DeactivateOnExit<MeteorImpactExplosion>();
    }

    private void VoidMeteor(uint id, float delay)
    {
        Cast(id, AID.VoidMeteor, delay, 4.8f)
            .ActivateOnEnter<MeteorImpactProximity>();
        ComponentCondition<MeteorImpactProximity>(id + 2, 1.2f, comp => comp.NumCasts > 0, "Proximity")
            .ActivateOnEnter<MeteorImpactCharge>()
            .DeactivateOnExit<MeteorImpactProximity>();
        Cast(id + 0x10, AID.MeteorImpact, 0.9f, 11);
        // +0.9s: first set bound
        ComponentCondition<MeteorImpactCharge>(id + 0x20, 2.7f, comp => comp.NumCasts >= 4, "Charges 1");
        // +5.4s: second set bound
        ComponentCondition<MeteorImpactCharge>(id + 0x30, 7.0f, comp => comp.NumCasts >= 8, "Charges 2")
            .DeactivateOnExit<MeteorImpactCharge>();
        ComponentCondition<MeteorImpactExplosion>(id + 0x40, 2.1f, comp => comp.Casters.Count > 0)
            .ActivateOnEnter<MeteorImpactExplosion>();
        ComponentCondition<MeteorImpactExplosion>(id + 0x41, 5, comp => comp.NumCasts > 0, "Explosions")
            .DeactivateOnExit<MeteorImpactExplosion>();
    }

    private void BlackHole(uint id, float delay)
    {
        Cast(id, AID.BlackHole, delay, 5);
        ComponentCondition<BlackHole>(id + 2, 0.8f, comp => comp.Baiter != null)
            .ActivateOnEnter<BlackHole>();
        CastStartMulti(id + 0x10, [AID.FracturedEventideWE, AID.FracturedEventideEW], 1.4f);
        ComponentCondition<BlackHole>(id + 0x11, 7.9f, comp => comp.Voidzone != null, "Black hole bait")
            .ActivateOnEnter<FracturedEventide>();
        CastEnd(id + 0x12, 2.1f);
        ComponentCondition<FracturedEventide>(id + 0x13, 0.5f, comp => comp.NumCasts > 0, "Laser start");
        ComponentCondition<FracturedEventide>(id + 0x20, 9.2f, comp => comp.NumCasts > 20, "Laser end")
            .DeactivateOnExit<FracturedEventide>();
        ComponentCondition<BlackHole>(id + 0x30, 4.1f, comp => comp.Voidzone == null, "Black hole resolve")
            .DeactivateOnExit<BlackHole>();
    }

    private void RendTheRift(uint id, float delay)
    {
        Cast(id, AID.RendTheRift, delay, 6, "Raidwide")
            .SetHint(StateMachine.StateHint.Raidwide);
    }

    private void DimensionalSurgeNostalgia(uint id, float delay)
    {
        ComponentCondition<NostalgiaDimensionalSurge>(id, delay, comp => comp.Casters.Count > 0)
            .ActivateOnEnter<NostalgiaDimensionalSurge>();
        CastStart(id + 0x10, AID.Nostalgia, 4, "First puddles");
        ComponentCondition<NostalgiaDimensionalSurge>(id + 0x20, 4, comp => comp.Casters.Count == 0, "Last puddles")
            .DeactivateOnExit<NostalgiaDimensionalSurge>();
        CastEnd(id + 0x30, 1);
        ComponentCondition<Nostalgia>(id + 0x40, 0.8f, comp => comp.NumCasts >= 1, "Raidwides start")
            .ActivateOnEnter<Nostalgia>()
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Nostalgia>(id + 0x41, 1, comp => comp.NumCasts >= 2)
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Nostalgia>(id + 0x42, 1, comp => comp.NumCasts >= 3)
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Nostalgia>(id + 0x43, 1, comp => comp.NumCasts >= 4)
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Nostalgia>(id + 0x44, 2, comp => comp.NumCasts >= 5)
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Nostalgia>(id + 0x45, 1, comp => comp.NumCasts >= 6)
            .SetHint(StateMachine.StateHint.Raidwide);
        ComponentCondition<Nostalgia>(id + 0x46, 3, comp => comp.NumCasts >= 7, "Raidwides end")
            .DeactivateOnExit<Nostalgia>()
            .SetHint(StateMachine.StateHint.Raidwide);
    }

    private void FlowOfTheAbyss(uint id, float delay, bool withPuddles)
    {
        CastStart(id, AID.FlowOfTheAbyss, delay)
            .ActivateOnEnter<NostalgiaDimensionalSurge>(withPuddles);
        ComponentCondition<FlowOfTheAbyssSpreadStack>(id + 1, 3.1f, comp => comp.Active)
            .ActivateOnEnter<FlowOfTheAbyssDimensionalSurge>()
            .ActivateOnEnter<FlowOfTheAbyssSpreadStack>()
            .DeactivateOnExit<NostalgiaDimensionalSurge>(withPuddles);
        CastEnd(id + 2, 3.9f);
        ComponentCondition<FlowOfTheAbyssSpreadStack>(id + 3, 1.1f, comp => !comp.Active, "Spread/stack/pairs")
            .ActivateOnEnter<FlowOfTheAbyssAkhRhai>()
            .DeactivateOnExit<FlowOfTheAbyssSpreadStack>();
        ComponentCondition<FlowOfTheAbyssDimensionalSurge>(id + 4, 0.9f, comp => comp.NumCasts > 0, "Line")
            .DeactivateOnExit<FlowOfTheAbyssDimensionalSurge>();

        Cast(id + 0x10, AID.ChasmicNails, 4.2f, 7)
            .ActivateOnEnter<ChasmicNails>()
            .ActivateOnEnter<FlowOfTheAbyssDimensionalSurge>()
            .ActivateOnEnter<NostalgiaDimensionalSurge>() // first puddles start ~4s into cast
            .DeactivateOnExit<FlowOfTheAbyssAkhRhai>();
        ComponentCondition<ChasmicNails>(id + 0x20, 0.7f, comp => comp.NumCasts >= 1, "Pizza start");
        ComponentCondition<NostalgiaDimensionalSurge>(id + 0x21, 0.3f, comp => comp.NumCasts > 0, "Puddles")
            .DeactivateOnExit<NostalgiaDimensionalSurge>();
        ComponentCondition<ChasmicNails>(id + 0x22, 0.4f, comp => comp.NumCasts >= 2);
        ComponentCondition<FlowOfTheAbyssDimensionalSurge>(id + 0x23, 0.6f, comp => comp.NumCasts > 0, "Line")
            .DeactivateOnExit<FlowOfTheAbyssDimensionalSurge>();
        ComponentCondition<ChasmicNails>(id + 0x24, 0.1f, comp => comp.NumCasts >= 3);
        ComponentCondition<ChasmicNails>(id + 0x25, 0.7f, comp => comp.NumCasts >= 4);
        ComponentCondition<ChasmicNails>(id + 0x26, 0.7f, comp => comp.NumCasts >= 5, "Pizza resolve")
            .DeactivateOnExit<ChasmicNails>();
    }
}