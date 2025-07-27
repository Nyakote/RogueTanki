using System;
using System.Collections.Generic;

public interface ITurretMod
{
    string Mod { get; set; }
    float Price { get; set; }
    float MinDamage { get; set; }
    float MaxDamage { get; set; }
    float RotationSpeed { get; set; }
    float RotationAcceleration { get; set; }
    float ReloadTime { get; set; }


    float? ImpactForce { get; set; }
    float? Recoil { get; set; }
    float? MinDamageRange { get; set; }
    float? MaxDamageRange { get; set; }
    float? ProjectileSpeed { get; set; }
    float? WeakDamagePercent { get; set; }
    float? EnergyPerShot { get; set; }
    float? CriticalChance { get; set; }
    float? CriticalDamage { get; set; }
    float? SplashImpactForce { get; set; }
    float? ChargeTime { get; set; }
    float? Delay { get; set; }
    float? PiercingDamagePercent { get; set; }
    float? PiercingPercent { get; set; }
    float? EnergyConsumption { get; set; }
    float? ArcadeReloadTime { get; set; }
    float? ScopedMinDamage { get; set; }
    float? ScopedMaxDamage { get; set; }
    float? ArcadeMinDamage { get; set; }
    float? ArcadeMaxDamage { get; set; }
    float? ArcadeImpactForce { get; set; }
    float? ScopedImpactForce { get; set; }
    float? HorizontalAimSpeed { get; set; }
    float? VerticalAimSpeed { get; set; }
    float? EnergyRecharge { get; set; }
    float? ScopedRotationAcceleration { get; set; }
    float? ScopedExitDelay { get; set; }
    float? ScopedEntryDelay { get; set; }
    float? BurnDamage { get; set; }
    float? Healing { get; set; }
    float? Damage { get; set; }             
    float? SelfHealingPercent { get; set; }
    float? ChargeTimeSecondary { get; set; }
}


public class TurretMod : ITurretMod
{
    public string Mod { get; set; }
    public float Price { get; set; }
    public float MinDamage { get; set; }
    public float MaxDamage { get; set; }
    public float RotationSpeed { get; set; }
    public float RotationAcceleration { get; set; }
    public float ReloadTime { get; set; }

    public float? ChargeTimeSecondary { get; set; }
    public float? ImpactForce { get; set; }
    public float? Recoil { get; set; }
    public float? MinDamageRange { get; set; }
    public float? MaxDamageRange { get; set; }
    public float? ProjectileSpeed { get; set; }
    public float? WeakDamagePercent { get; set; }
    public float? EnergyPerShot { get; set; }
    public float? CriticalChance { get; set; }
    public float? CriticalDamage { get; set; }
    public float? SplashImpactForce { get; set; }
    public float? ChargeTime { get; set; }
    public float? Delay { get; set; }
    public float? PiercingDamagePercent { get; set; }
    public float? PiercingPercent { get; set; }
    public float? EnergyConsumption { get; set; }
    public float? ArcadeReloadTime { get; set; }
    public float? ScopedMinDamage { get; set; }
    public float? ScopedMaxDamage { get; set; }
    public float? ArcadeMinDamage { get; set; }
    public float? ArcadeMaxDamage { get; set; }
    public float? ArcadeImpactForce { get; set; }
    public float? ScopedImpactForce { get; set; }
    public float? HorizontalAimSpeed { get; set; }
    public float? VerticalAimSpeed { get; set; }
    public float? EnergyRecharge { get; set; }
    public float? ScopedRotationAcceleration { get; set; }
    public float? ScopedExitDelay { get; set; }
    public float? ScopedEntryDelay { get; set; }
    public float? BurnDamage { get; set; }
    public float? Healing { get; set; }
    public float? Damage { get; set; }
    public float? SelfHealingPercent { get; set; }
}

public class TurretFixedStats
{
    public int AutoAimUp { get; set; }
    public int AutoAimDown { get; set; }
    public float? WeakDamagePercent { get; set; }
    public int? MaxDamageRadius { get; set; }
    public int? MinDamageRadius { get; set; }
    public float? WeakDamagePercentSplash { get; set; }
    public int? EnergyCapacity { get; set; }
    public float? EnergyRecharge { get; set; }
    public float? IdleEnergyConsumption { get; set; }
    public float? AttackEnergyConsumption { get; set; }
    public float? HealingEnergyConsumption { get; set; }
    public float? ConeAngle { get; set; }
    public int? MinFoliageTransparencyRadius { get; set; }
    public int? MaxFoliageTransparencyRadius { get; set; }
    public float? FovMin { get; set; }
    public float? FovMax { get; set; }
    public float? SlowdownStartPoint { get; set; }
    public float? SlowdownEndPoint { get; set; }
    public float? MinSlowdownFactor { get; set; }
    public float? FreezeDuration { get; set; }
    public float? ProjectileRadius { get; set; }
}

public class Turret
{
    public string Name { get; set; }
    public string Type { get; set; }
    public List<TurretMod> Mods { get; set; }
    public TurretFixedStats Fixeds { get; set; }
}
