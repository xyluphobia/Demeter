using Godot;
using System;

[GlobalClass]
public partial class CropRegistryResource : Resource
{
  [Export] public CropVarietyResource[] AllCrops { get; set; } = System.Array.Empty<CropVarietyResource>();
}
