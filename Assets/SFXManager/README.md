# SFXManager

The `SFXManager` system is designed to manage and play sound effects (SFX) in a Unity project. It provides an easy way to handle multiple sound effects with multiple clips + variations in volume and pitch.

## Features

- Singleton pattern for easy access.
- Object pooling for efficient audio source management.
- Supports multiple audio clips for each SFX with random pitch variance.

## Setup

1. Attach the `SFXManager` script to a GameObject in your scene.
2. Assign an `SFXSource` prefab to the `sfxSource` field in the `SFXManager` component.
3. Add your sound effects to the `sfxList` in the `SFXManager` component.

## Usage

To play a sound effect, call the `PlaySFX` method with the name of the sound effect:

```csharp
SFXManager.Instance.PlaySFX("SFXName");
```

Make sure the name matches one of the sound effects in the `sfxList`.

## Example

```csharp
public class Example : MonoBehaviour
{
    void Start()
    {
        SFXManager.Instance.PlaySFX("Explosion");
    }
}
```

## Notes

- Ensure that each SFX has at least one audio clip assigned.
- Duplicate SFX names in the `sfxList` will result in only the last one being used.
