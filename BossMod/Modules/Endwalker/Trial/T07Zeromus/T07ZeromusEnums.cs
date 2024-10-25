namespace BossMod.Endwalker.Trial.T07Zeromus;

public enum OID : uint
{
    Boss = 0x404E, // R20.000, x1
    Comet = 0x40AB, // R2.250, x22
    ToxicBubble = 0x40AC, // R1.700, x0 (spawn during fight)
    FlowOfTheAbyss = 0x4110, // R1.000, x1
    Helper = 0x233C, // R0.500, x30, Helper type
    FlareTower = 0x1EB94E, // R0.500, x0 (spawn during fight), EventObj type
    FlareRay = 0x1EB94F, // R0.500, x0 (spawn during fight), EventObj type
    BlackHole = 0x1EB94C, // R0.500, x0 (spawn during fight), EventObj type
}

public enum AID : uint
{
    AutoAttack = 35912, // Boss->player, no cast, single-target

    AbyssalNox = 35575, // Boss->self, 5.0s cast, range 60 circle, doom on all players, heal to full to remove
    AbyssalEchoesVisualCardinal = 35576, // Helper->self, 9.0s cast, single-target
    AbyssalEchoesVisualIntercardinal = 35577, // Helper->self, 9.0s cast, single-target
    AbyssalEchoes = 35578, // Helper->self, 16.0s cast, range 12 circle

    SableThread = 35566, // Boss->self, 5.0s cast, single-target
    SableThreadTarget = 35567, // Helper->player, no cast, single-target
    SableThreadVisualHitIntermediate = 35568, // Boss->self, no cast, single-target
    SableThreadVisualHitLast = 35569, // Boss->self, no cast, single-target
    SableThreadAOE = 35570, // Helper->self, no cast, range 60 width 12 rect, 5-hit raid stack

    DarkMatter = 35638, // Boss->self, 4.0+1.0s cast, single-target
    DarkMatterAOE = 35639, // Helper->player, no cast, range 8 circle, 3-hit aoe tankbuster

    VisceralWhirlR = 35579, // Boss->self, 8.0+0.8s cast, single-target
    VisceralWhirlRAOE1 = 35580, // Helper->self, 8.8s cast, range 29 width 28 rect
    VisceralWhirlRAOE2 = 35581, // Helper->self, 8.8s cast, range 60 width 28 rect
    VisceralWhirlL = 35582, // Boss->self, 8.0+0.8s cast, single-target
    VisceralWhirlLAOE1 = 35583, // Helper->self, 8.8s cast, range 29 width 28 rect
    VisceralWhirlLAOE2 = 35584, // Helper->self, 8.8s cast, range 60 width 28 rect

    Flare = 35602, // Boss->self, 7.0+1.0s cast, single-target, visual (towers)
    FlareAOE = 35603, // Helper->self, 8.0s cast, range 5 circle tower
    FlareScald = 35765, // Helper->self, no cast, range 5 circle (tower aftereffect, damage + vuln)
    FlareKill = 35605, // Helper->self, 5.0s cast, range 5 circle (tower aftereffect, kill)
    ProminenceSpine = 35606, // Helper->self, 5.0s cast, range 60 width 10 rect (tower aftereffect, ray)

    VoidBio = 35607, // Boss->self, 5.0s cast, single-target
    VoidBioBurst = 35608, // Boss->player, no cast, single-target

    BigBang = 35587, // Boss->self, 10.0s cast, range 60 circle, raidwide with dot
    BigBangAOE = 35589, // Helper->location, 8.0s cast, range 5 circle puddle
    //BigBangSpread = 35806, // Helper->player, 5.0s cast, range 5 circle spread
    BigCrunch = 35588, // Boss->self, 10.0s cast, range 60 circle, raidwide with dot
    BigCrunchAOE = 36144, // Helper->location, 6.0s cast, range 5 circle puddle
    //BigCrunchSpread = 36146, // Helper->player, 5.0s cast, range 5 circle spread

    VoidMeteor = 35596, // Boss->self, 4.8+1.0s cast, single-target, visual (initial meteors)
    MeteorImpactProximity = 35601, // Comet->self, 6.0s cast, range 40 circle with ? falloff
    MeteorImpact = 35595, // Boss->self, 11.0+1.0s cast, single-target visual (tethered meteors)
    MeteorImpactChargeNormal = 35597, // Comet->location, no cast, width 4 rect charge (non-clipping, fatal damage if distance is less than ~25, otherwise small damage)
    MeteorImpactChargeClipping = 35598, // Comet->location, no cast, width 4 rect charge (clipping, fatal damage followed by raidwide and vuln)
    MeteorImpactAppearNormal = 36024, // Helper->location, no cast, range 2 circle
    MeteorImpactAppearClipping = 35599, // Helper->location, no cast, range 60 circle, raidwide with vuln (followed by wipe)
    MeteorImpactMassiveExplosion = 35600, // Comet->self, no cast, range 60 circle (wipe if meteor is clipped)
    MeteorImpactExplosion = 36147, // Comet->self, 5.0s cast, range 10 circle

    DarkDivides = 35593, // Helper->player, no cast, range 5 circle, spread
    DarkBeckons = 35594, // Helper->player, no cast, range 6 circle, stack

    BlackHole = 36133, // Boss->self, 5.0s cast, single-target, visual (placeable black hole + laser)
    BlackHoleHelper = 36134, // Helper->self, 5.7s cast, range 4 circle
    FracturedEventideWE = 35571, // Boss->self, 8.0s cast, single-target, visual (laser W to E)
    FracturedEventideEW = 35572, // Boss->self, 8.0s cast, single-target, visual (laser E to W)
    FracturedEventideAOEFirst = 35910, // Helper->self, 8.5s cast, range 60 width 8 rect
    //FracturedEventideAOEFirst = 35573, // Helper->self, 8.5s cast, range 60 width 8 rect
    FracturedEventideAOERest = 35574, // Helper->self, no cast, range 60 width 8 rect

    Nox = 36135, // Boss->self, 4.0+1.0s cast, single-target, visual (chasing aoe)
    NoxAOEFirst = 36137, // Helper->self, 5.0s cast, range 10 circle
    NoxAOERest = 36131, // Helper->self, no cast, range 10 circle

    RendTheRift = 35609, // Boss->self, 6.0s cast, range 60 circle, raidwide
    NostalgiaDimensionalSurge = 35633, // Helper->location, 4.0s cast, range 5 circle puddle
    Nostalgia = 35610, // Boss->self, 5.0s cast, single-target, visual (multiple raidwides)
    NostalgiaBury1 = 35612, // Helper->self, 0.7s cast, range 60 circle
    NostalgiaBury2 = 35613, // Helper->self, 1.7s cast, range 60 circle
    NostalgiaBury3 = 35614, // Helper->self, 2.7s cast, range 60 circle
    NostalgiaBury4 = 35615, // Helper->self, 3.7s cast, range 60 circle
    NostalgiaRoar1 = 35616, // Helper->self, 5.7s cast, range 60 circle
    NostalgiaRoar2 = 35617, // Helper->self, 6.7s cast, range 60 circle
    NostalgiaPrimalRoar = 35618, // Helper->self, 9.7s cast, range 60 circle

    FlowOfTheAbyss = 36089, // Boss->self, 7.0s cast, single-target, visual (spread/stack + unsafe line)
    FlowOfTheAbyssDimensionalSurge = 35637, // Helper->self, 9.0s cast, range 60 width 14 rect
    AkhRhaiStart = 35619, // Helper->self, no cast, single-target
    AkhRhaiAOE = 35620, // Helper->self, no cast, range 5 circle spread
    UmbralRays = 35621, // Helper->players, 5.0s cast, range 6 circle, 8-man stack
    ChasmicNails = 35622, // Boss->self, 7.0s cast, single-target, visual (pizzas)
    ChasmicNailsAOE1 = 35628, // Helper->self, 7.7s cast, range 60 40-degree cone
    ChasmicNailsAOE2 = 35629, // Helper->self, 8.4s cast, range 60 40-degree cone
    ChasmicNailsAOE3 = 35630, // Helper->self, 9.1s cast, range 60 40-degree cone
    ChasmicNailsAOE4 = 35631, // Helper->self, 9.8s cast, range 60 40-degree cone
    ChasmicNailsAOE5 = 35632, // Helper->self, 10.5s cast, range 60 40-degree cone
    ChasmicNailsVisual1 = 35623, // Helper->self, 1.5s cast, range 60 40-degree cone, visual (telegraph)
    ChasmicNailsVisual2 = 35624, // Helper->self, 3.0s cast, range 60 40-degree cone, visual (telegraph)
    ChasmicNailsVisual3 = 35625, // Helper->self, 4.0s cast, range 60 40-degree cone, visual (telegraph)
    ChasmicNailsVisual4 = 35626, // Helper->self, 5.0s cast, range 60 40-degree cone, visual (telegraph)
    ChasmicNailsVisual5 = 35627, // Helper->self, 6.0s cast, range 60 40-degree cone, visual (telegraph)

    Enrage = 35660, // Boss->self, 10.0s cast, range 60 circle, enrage
}

public enum SID : uint
{
    HPPenalty = 1089, // none->player, extra=0x0
    Pollen = 1507, // Boss->player, extra=0x0
    Doom = 1769, // Boss->player, extra=0x0
    VulnerabilityUp = 1789, // Helper/40AC->player, extra=0x1/0x2/0x3/0x4/0x6
    BluntResistanceDown = 2248, // Comet->player, extra=0x0
    Bind = 2518, // none->player, extra=0x0
    AccelerationBomb = 2657, // none->player, extra=0x0
    BigBang = 3760, // none->Boss, extra=0x0
    BigBounce = 3761, // Boss->player, extra=0x0
    DivisiveDark = 3762, // none->player, extra=0x0
    BeckoningDark = 3763, // none->player, extra=0x0
}

public enum IconID : uint
{
    DarkMatter = 364, // player
    BigBang = 376, // player
    Chain = 326, // player
    AccelerationBomb = 267, // player
    DarkBeckonsUmbralRays = 62, // player
    DarkBeckons = 100, //player
    BlackHole = 330, // player
    Nox = 197, // player
    AkhRhai = 23, // player
    //UmbralPrism = 211, // player
}

public enum TetherID : uint
{
    VoidMeteorCloseClipping = 252, // Comet->player
    VoidMeteorCloseGood = 253, // Comet->player
    VoidMeteorStretchedClipping = 254, // Comet->player
    VoidMeteorStretchedGood = 255, // Comet->player
    BondsOfDarkness = 163, // player->player
    FlowOfTheAbyss = 265, // FlowOfTheAbyss->Boss
}