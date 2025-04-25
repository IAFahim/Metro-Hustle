# BovineLabs Essence

BovineLabs Essence is a comprehensive stat and attribute library for Unity's Entity Component System (ECS). It provides a robust framework for managing character statistics, resources, and their modifiers - all within the DOTS ecosystem.

Essence integrates deeply with [BovineLabs Reaction](https://gitlab.com/tertle/com.bovinelabs.reaction) to create a powerful event-driven gameplay framework that enables condition-based gameplay mechanics.

For support and discussions, join [Discord](https://discord.gg/RTsw6Cxvw3). If you want to support my work or get access to a few private libraries, [Buy Me a Coffee](https://buymeacoffee.com/bovinelabs).

## Installation

1. Ensure you have the required dependencies installed:
    - Unity 6
    - Entities 1.3.5+
    - [BovineLabs Reaction](https://gitlab.com/tertle/com.bovinelabs.reaction)
    - [BovineLabs Core](https://gitlab.com/tertle/com.bovinelabs.core)

2. Add the package to your project via the Package Manager
    - Add via Git URL: `https://gitlab.com/tertle/com.bovinelabs.stats.git`
    - Or extract the released zip file to your project's `Packages` folder

## Core Concepts

Essence revolves around two primary concepts: Stats and Intrinsics.

### Stats

Stats are entity attributes that can be modified through various sources and formulas. They represent properties like Strength, Intelligence, Movement Speed, etc.

Key features:
- Base values with additive, multiplicative, and percentage-based modifiers
- ECS-optimized calculation system
- Automatic recalculation when modifiers change
- Integration with Reaction's condition system

#### Defining Stats

Stats are defined using `StatSchemaObject` scriptable objects:

1. In your project settings, create a StatSettings asset
2. Add stat definitions to the statSchemas list
3. Each StatSchemaObject will have a unique key and name

#### Using Stats

```csharp
// Add StatAuthoring component to an entity
public partial struct MySystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // Reading a stat value
        foreach (var stats in SystemAPI.Query<DynamicBuffer<Stat>>())
        {
            // Create a StatKey from your custom key value
            StatKey moveSpeedKey = new StatKey { Value = (ushort)StatKeys.MoveSpeed };
            // Or use implicit conversion if available
            // StatKey moveSpeedKey = StatKeys.MoveSpeed;
            
            float moveSpeed = stats.GetValue(moveSpeedKey);
            // Use the value...
        }
    }
}
```

#### Modifying Stats

Stats can be modified through the `StatModifiers` buffer:

```csharp
// Adding a modifier
var statModifiers = EntityManager.GetBuffer<StatModifiers>(entity);
statModifiers.Add(new StatModifiers
{
    SourceEntity = sourceEntity,
    Value = new StatModifier
    {
        Type = new StatKey { Value = (ushort)StatKeys.Strength }, // Explicitly create a StatKey
        // Or with implicit conversion if available:
        // Type = StatKeys.Strength,
        ModifyType = StatModifyType.Added,
        Value = 10 // Use Value for Added type modifiers
    }
});

// If using multiplicative or additive modifiers, use ValueHalf instead
statModifiers.Add(new StatModifiers
{
    SourceEntity = sourceEntity,
    Value = new StatModifier
    {
        Type = new StatKey { Value = (ushort)StatKeys.CriticalChance }, // Explicitly create a StatKey
        ModifyType = StatModifyType.Multiplicative,
        ValueHalf = new half(0.25f) // 25% increase, use ValueHalf for multiplicative modifiers
    }
});
```

### Intrinsics

Intrinsics are resources or properties that can change over time but aren't affected by the same modifier system. They represent things like Health, Mana, Experience, etc.

Key features:
- Simple integer-based values
- Min/Max clamping
- Event triggering on change
- Integration with Reaction's condition system

#### Defining Intrinsics

Intrinsics are defined using `IntrinsicSchemaObject` scriptable objects:

1. In your project settings, create a StatSettings asset
2. Add intrinsic definitions to the intrinsicSchemas list
3. Each IntrinsicSchemaObject will have a unique key, name, and value range

#### Using Intrinsics

```csharp
// Add StatAuthoring component with Intrinsics enabled
public partial struct DamageSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // Reading/writing intrinsic values
        var intrinsicLookup = SystemAPI.GetBufferLookup<Intrinsic>();
        
        foreach (var (damageEvent, entity) in SystemAPI.Query<DamageEvent>().WithEntityAccess())
        {
            if (intrinsicLookup.TryGetBuffer(entity, out var buffer))
            {
                var intrinsics = buffer.AsMap();
                
                // Create an IntrinsicKey from your custom key value
                IntrinsicKey healthKey = new IntrinsicKey { Value = (ushort)IntrinsicKeys.Health };
                // Or use implicit conversion if available
                // IntrinsicKey healthKey = IntrinsicKeys.Health;
                
                var health = intrinsics.GetValue(healthKey);
                
                // Subtract damage from health
                intrinsics.GetOrAddRef(healthKey) -= damageEvent.Amount;
            }
        }
    }
}
```

For more controlled access with events, use the `IntrinsicWriter`:

```csharp
var intrinsicWriter = new IntrinsicWriter.Lookup();
intrinsicWriter.Create(ref systemState);
intrinsicWriter.Update(ref systemState);

// Later in your code:
if (intrinsicWriter.TryGet(entity, out var writer))
{
    writer.Subtract(IntrinsicKeys.Health, damageAmount);
    // This will automatically trigger events and update conditions
}
```

### Actions

Essence includes a powerful action system for applying stat and intrinsic modifications through the Reaction framework:

#### Action Stats

`ActionStat` components define how stats should be modified when an action is performed:

1. Add ActionStatAuthoring component to your entity
2. Configure the Stats array with the stats to modify, their modification type, and values

#### Action Intrinsics

`ActionIntrinsic` components define how intrinsics should be modified:

1. Add ActionIntrinsicAuthoring component to your entity
2. Configure the Intrinsics array with the intrinsics to modify and their values

### Integration with Reaction

Essence tightly integrates with the Reaction framework to create condition-driven gameplay mechanics.

Example: Create a condition that triggers when health falls below 20%
1. Create an IntrinsicSchemaObject for Health
2. Add the "Add Event Condition" button in the inspector
3. Reference this condition in your ReactionAuthoring component

## Architecture

Essence is built with a clean architecture that separates:

- **Data Layer**: Contains all component definitions and structures
- **Systems Layer**: Handles stat calculation, intrinsic update, and event triggering
- **Authoring Layer**: Provides Unity editor integration for designer-friendly workflows

### Key System Flow

1. `StatCalculationSystem` recalculates all stats when modifiers change
2. `ActionStatSystem` and `ActionIntrinsicSystem` apply changes from actions
3. `ConditionStatWriteSystem` and `ConditionIntrinsicWriteSystem` update conditions based on stat/intrinsic changes
4. Reaction systems then handle the triggered conditions

## Examples

### Setting Up a Character with Stats

1. Create a `StatSettings` asset in your project
2. Add stat definitions for Strength, Agility, Health, etc.
3. Create an entity with `StatAuthoring` component
4. Configure default stat values
5. At runtime, access and modify stats as needed

### Creating a Damage System

```csharp
public partial struct DamageSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var intrinsicWriters = new IntrinsicWriter.Lookup();
        intrinsicWriters.Create(ref state);
        intrinsicWriters.Update(ref state);

        // Process damage events
        foreach (var (damage, entity) in SystemAPI.Query<DamageComponent>().WithEntityAccess())
        {
            if (intrinsicWriters.TryGet(entity, out var writer))
            {
                // Create an IntrinsicKey from your custom key value
                IntrinsicKey healthKey = new IntrinsicKey { Value = (ushort)IntrinsicKeys.Health };
                // Or use implicit conversion if available
                // IntrinsicKey healthKey = IntrinsicKeys.Health;
                
                writer.Subtract(healthKey, damage.Amount);
                
                // Check if entity died
                var health = writer.Intrinsics.GetValue(healthKey);
                if (health <= 0)
                {
                    // Trigger death event or destroy entity
                }
            }
        }
    }
}
```

## Advanced Usage

### Stat Linear Scaling

Stats can be configured to scale linearly based on condition values:

In ActionStatAuthoring:
1. Set ValueType to Linear
2. Set a Condition to scale from
3. Configure FromMin/FromMax as the condition range
4. Configure ToMin/ToMax as the output range

### Stat Initialization

For entities that need to copy stats from other entities:

1. Add InitializeStatsAuthoring component
2. Set Source to the target you want to copy from (e.g., Owner, Source)
3. This works with projectiles or temporary entities that should inherit stats

### Event Conditions

Both Stats and Intrinsics can trigger Reaction events:

- For intrinsics: Add Event Condition in the IntrinsicSchemaObject inspector
- For stats: Add Event Condition in the StatSchemaObject inspector

These events can be used in Reaction's event-based gameplay systems

## Performance Considerations

- Stats are calculated only when modifiers change, not every frame
- The `StatModifiers` buffer is disabled after calculation to prevent unnecessary recalculation
- Intrinsic operations use efficient hash maps for fast lookups
- All systems are fully Burst-compatible for maximum performance

## Compatibility

Essence is designed for Unity 6 and Entities 1.3.5 or newer. It requires BovineLabs Core and Reaction packages.

For custom inspectors and improved editor experience, use BovineLabs' fork of [Unity Entities](https://github.com/tertle/com.unity.entities).