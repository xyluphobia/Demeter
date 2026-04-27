using Godot;
using System;
 
public class FarmBedPlot : PlotObject 
{
  private CropVarietyResource? _crop;
  public CropVarietyResource? Crop { 
    get => _crop; 
    set {
      _crop = value;
      if (value != null)
        CropPlantedSetup();
    }
  }
  public Sprite2D CropSprite  { get; set; }

  public bool HasBeenHarvested { get; private set; } = false;
  public int DaysSincePlanted { get; private set; }

  // 'Effective' versions set in constructor used to provide some variance
  // On when each crop grows 
  private int _effectiveMaturityDays;
  private int _effectiveReharvestDays;
  private SceneTreeTimer _growthTimer;
  private Action _growthAction;

  public int DaysUntilHarvest {
    get {
      if (!HasBeenHarvested)
        return _effectiveMaturityDays - DaysSincePlanted;
      else
        return _effectiveReharvestDays - DaysSincePlanted;
    }
  }
  public int GrowthStage { get; private set; } = 1;

  private readonly Vector2 _worldPos;

  public FarmBedPlot(Vector2I GridPosition) : base(GridPosition)
  {
    _worldPos = FarmManager.I.GridToWorld(GridPosition);

    GameCalendar.I.DayPassed += OnDayPassed;
  }

  private void CropPlantedSetup() {
    float variance = (float)GD.RandRange(-0.15, 0.15);
    _effectiveMaturityDays   = Mathf.RoundToInt(_crop.MaturityDays * (1f + variance));
    _effectiveReharvestDays  = Mathf.RoundToInt(_crop.ReharvestCycleDays * (1f + variance));

    CropSprite = new Sprite2D();
    CropSprite.Texture = _crop.LifeCycleTexture;
    CropSprite.RegionEnabled = true;
    CropSprite.RegionRect = new Rect2(0, 0, 16, _crop.LifeCycleTexture.GetHeight());
    CropSprite.Position = _worldPos;
    FarmManager.I.CropSpriteContainer.AddChild(CropSprite);
  }

  private void OnDayPassed(int day) {
    DaysSincePlanted++;

    if (_growthTimer != null) {
      _growthTimer.Timeout -= _growthAction;
      UpdateGrowthStage();
      _growthTimer = null;
    }

    _growthAction = () =>{
      UpdateGrowthStage();
      _growthTimer = null;
    };

    float delay = (float)GD.RandRange(0.0, 300.0);
    _growthTimer = ((SceneTree)Engine.GetMainLoop()).CreateTimer(delay);
    _growthTimer.Timeout += _growthAction;
  }

  private void UpdateGrowthStage() {
    int minStage = HasBeenHarvested ? 3 : 1; 

    int cycleDays = HasBeenHarvested ? _effectiveReharvestDays : _effectiveMaturityDays;;
    float progress = (float)DaysSincePlanted / cycleDays;

    // 20% of cycle days as a grace period to harvest before wilt. 
    // Using -(cycledays * 0.2f) because DaysUntilHarvest goes negative after hitting 0
    if (DaysUntilHarvest < -(cycleDays * 0.2f)) {
      GrowthStage = 6;
      UpdateSprite();
      return;
    }

    GrowthStage = progress switch {
      >= 1.0f => 5,
      >= 0.8f => 4,
      >= 0.6f => 3,
      >= 0.4f => 2,
      _       => 1,
    };

    GrowthStage = Mathf.Max(GrowthStage, minStage);
    UpdateSprite();
  }

  public void Harvest() {
    HasBeenHarvested = true;
    DaysSincePlanted = 0;
    GrowthStage = 3;
    UpdateSprite();
  }

  private void UpdateSprite() {
    CropSprite.RegionRect = new Rect2(((GrowthStage - 1) * 16), 0, 16, Crop.LifeCycleTexture.GetHeight());
  }

  public void ClearCrop() {
    if (CropSprite != null)
      CropSprite.Texture = null;

    _growthTimer = null;
    _crop = null;
    DaysSincePlanted = 0;
    GrowthStage = 1;
    HasBeenHarvested = false;
    _growthAction = null;
  }

  public override void Cleanup() {
    GameCalendar.I.DayPassed -= OnDayPassed;
    if (_growthTimer != null) {
      _growthTimer.Timeout -= _growthAction;
      _growthTimer = null;
    }
    ClearCrop();
    CropSprite.QueueFree();
  }
}
