using Godot;
using System;
using System.Linq;

public partial class JournalUi : CanvasLayer
{
  [Export] public CropRegistryResource CropRegistry { get; set; }

  private SeedGrid _seedGridLeft;
  private SeedGrid _seedGridRight;

  private Control _seedDetails;
  private CropVarietyResource _crop;
  private Vector2I _activeGridPos;

  private CropVarietyResource[][] _pages;
  private int _pageIndex = 0;

  private const int SeedsPerPage = 9;

	public override void _Ready() {
    UIManager.I.OpenSeedSelection += () => {
      this.Visible = true;
      _activeGridPos = FarmManager.I.ActiveGridPos;
    };

    _seedGridLeft = GetNode<SeedGrid>("BookBaseContainer/LeftPage/MarginContainer/SeedGridLeft");
    _seedGridRight = GetNode<SeedGrid>("BookBaseContainer/RightPage/MarginContainer/SeedGridRight");

    _seedGridLeft.SeedPressed  += OnSeedPressed;
    _seedGridRight.SeedPressed += OnSeedPressed;

    _seedDetails = GetNode<Control>("BookBaseContainer/RightPage/SeedDetails");
    _seedDetails.GetNode<TextureButton>("SeedDetailsFreeMove/ActionButtons/CancelButton").Pressed += () => _seedDetails.Visible = false;
    _seedDetails.GetNode<TextureButton>("SeedDetailsFreeMove/ActionButtons/PlantButton").Pressed += OnPlantCrop;

    GetNode<Button>("BookBaseContainer/LeftPage/PrevPage").Pressed += OnPrevPage;
    GetNode<Button>("BookBaseContainer/RightPage/NextPage").Pressed += OnNextPage;

    GetNode<Button>("BookBaseContainer/LeftPage/Summer").Pressed += () => LoadSeeds(CropRegistry.AllCrops
        .Where(c => c.Season == Seasons.SUMMER).ToArray());
    GetNode<Button>("BookBaseContainer/LeftPage/Autumn").Pressed += () => LoadSeeds(CropRegistry.AllCrops
        .Where(c => c.Season == Seasons.AUTUMN).ToArray());
    GetNode<Button>("BookBaseContainer/LeftPage/Winter").Pressed += () => LoadSeeds(CropRegistry.AllCrops
        .Where(c => c.Season == Seasons.WINTER).ToArray());
    GetNode<Button>("BookBaseContainer/LeftPage/Spring").Pressed += () => LoadSeeds(CropRegistry.AllCrops
        .Where(c => c.Season == Seasons.SPRING).ToArray());

    LoadSeeds(CropRegistry.AllCrops);
	}

  private void LoadSeeds(CropVarietyResource[] seeds) {
    var sorted = seeds.OrderBy(crop => crop.Name).ToArray();

    var pages = sorted
      .Select((crop, i) => (crop, i))
      .GroupBy(pair => pair.i / SeedsPerPage)
      .Select(group => {
          var chunk = group.Select(pair => pair.crop).ToArray();
          return chunk.Concat(
              Enumerable.Repeat<CropVarietyResource>(null, SeedsPerPage - chunk.Length)
          ).ToArray();
      })
      .ToList();

    if (pages.Count % 2 != 0)
      pages.Add(Enumerable.Repeat<CropVarietyResource>(null, SeedsPerPage).ToArray());

    _pages = pages.ToArray();
    _pageIndex = 0;
    RefreshPages();
  }

  private void RefreshPages() {
    var leftSeeds = _pageIndex < _pages.Length ? _pages[_pageIndex] : System.Array.Empty<CropVarietyResource>();
    var rightSeeds = _pageIndex + 1 < _pages.Length ? _pages[_pageIndex + 1] : System.Array.Empty<CropVarietyResource>();

    _seedGridLeft.PopulateSeedGrid(leftSeeds);
    _seedGridRight.PopulateSeedGrid(rightSeeds);

    GetNode<Button>("BookBaseContainer/LeftPage/PrevPage").Visible = _pageIndex > 0;
    GetNode<Button>("BookBaseContainer/RightPage/NextPage").Visible = _pageIndex + 2 < _pages.Length;
  }


  private void OnNextPage() {
    if (_pageIndex + 2 < _pages.Length) {
      _pageIndex += 2;
      RefreshPages();
    }
  }

  private void OnPrevPage() {
    if (_pageIndex - 2 >= 0) {
      _pageIndex -= 2;
      RefreshPages();
    }
  }

  private void OnSeedPressed(CropVarietyResource crop)
  {
    _crop = crop;
    _seedDetails.Visible = true;
    
    var freeMove = _seedDetails.GetNode<Control>("SeedDetailsFreeMove/");
    freeMove.GetNode<Label>("Name/Variety").Text                    = crop.Name;
    freeMove.GetNode<Label>("Name/Plant").Text                      = crop.Plant.ToString();
    freeMove.GetNode<Label>("RightStats/TimeToMaturity/TimeToMaturity").Text   = crop.MaturityDays.ToString();
    freeMove.GetNode<Label>("RightStats/SellPrice/SellPrice").Text             = crop.BaseSellPrice.ToString();
    freeMove.GetNode<PlantStatBar>("Stats").Populate(crop);

    var cropImage = freeMove.GetNode<TextureRect>("Container/PlantImage");
    var lifeCycleAtlas = new AtlasTexture();
    lifeCycleAtlas.Atlas = crop.LifeCycleTexture;
    lifeCycleAtlas.Region = new Rect2(4 * 16, 0, 16, crop.LifeCycleTexture.GetHeight()); // frame 6 = mature
    lifeCycleAtlas.FilterClip = true;
    cropImage.Texture = lifeCycleAtlas;

    var seasonIcon = freeMove.GetNode<TextureRect>("Banner/VBoxContainer/BackDropSeason/SeasonIcon");
    var seasonAtlas = (AtlasTexture)seasonIcon.Texture.Duplicate();
    seasonAtlas.Region = crop.Season switch
    {
        Seasons.AUTUMN => new Rect2(20, 3,  9, 10),
        Seasons.SPRING => new Rect2(3,  20, 9, 9),
        Seasons.WINTER => new Rect2(20, 20, 9, 9),
        _              => new Rect2(3,  3,  9, 9),
    };
    seasonIcon.Texture = seasonAtlas;

    var harvestIcon = freeMove.GetNode<TextureRect>("Banner/VBoxContainer/BackDropHarvest/HarvestIcon");
    var atlas = (AtlasTexture)harvestIcon.Texture.Duplicate();
    atlas.Region = crop.HarvestType == HarvestTypes.REPEAT
        ? new Rect2(10, 0, 10, 11)  // second icon
        : new Rect2(0, 0, 10, 11);  // first icon
    harvestIcon.Texture = atlas;
  }
  
  private void OnPlantCrop() {
    FarmManager.I.EmitSignal(FarmManager.SignalName.PlantCrop, _activeGridPos, _crop);
    _seedDetails.Visible = false;
    this.Visible = false;
  }
}
