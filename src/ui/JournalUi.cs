using Godot;
using System;
using System.Linq;

public partial class JournalUi : CanvasLayer
{
  [Export] public CropRegistryResource CropRegistry { get; set; }

  private SeedGrid _seedGridLeft;
  private SeedGrid _seedGridRight;

  private CropVarietyResource[][] _pages;
  private int _pageIndex = 0;

  private const int SeedsPerPage = 9;

	public override void _Ready()
	{
    _seedGridLeft = GetNode<SeedGrid>("BookBaseContainer/LeftPage/MarginContainer/SeedGridLeft");
    _seedGridRight = GetNode<SeedGrid>("BookBaseContainer/RightPage/MarginContainer/SeedGridRight");

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
}
