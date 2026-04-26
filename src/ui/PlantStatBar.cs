using Godot;

public partial class PlantStatBar : Node
{
    private AtlasTexture _flavourAtlas;
    private AtlasTexture _yieldAtlas;
    private AtlasTexture _shelfLifeAtlas;
    private AtlasTexture _resilianceAtlas;

    public override void _Ready()
    {
        _flavourAtlas    = DuplicateBarAtlas("Flavour");
        _yieldAtlas      = DuplicateBarAtlas("Yield");
        _shelfLifeAtlas  = DuplicateBarAtlas("ShelfLife");
        _resilianceAtlas = DuplicateBarAtlas("Resiliance");
    }

    private AtlasTexture DuplicateBarAtlas(string statName)
    {
        var barFill = GetNode<TextureRect>($"{statName}/PlantStatBar/BarFill");
        var atlas = (AtlasTexture)barFill.Texture.Duplicate();
        barFill.Texture = atlas; // assign the unique copy back
        return atlas;
    }

    public void Populate(CropVarietyResource crop)
    {
        SetValue(_flavourAtlas,    crop.Flavour);
        SetValue(_yieldAtlas,      crop.Yield);
        SetValue(_shelfLifeAtlas,  crop.ShelfLife);
        SetValue(_resilianceAtlas, crop.Resilience);
    }

    private void SetValue(AtlasTexture atlas, float statValue)
    {
        int step = Mathf.Clamp(Mathf.RoundToInt(statValue / 10f), 0, 10);

        atlas.Region = step == 0
            ? new Rect2(0, 0, 43, 7)
            : new Rect2(0, 7 + (step - 1) * 5, 43, 5);
    }
}
