using GoogleMobileAds.Api;
using Tech.Singleton;

public class AdManager : Singleton<AdManager>
{
    private const string _bannerID = "ca-app-pub-3940256099942544/6300978111";
    private BannerView _bannerView;
    
    protected override void Awake()
    {
        base.Awake();
        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        MobileAds.Initialize(initStatus => { });
    }

    private void Start()
    {
        LoadAd();
    }

    public void CreateBannerView()
    {
        _bannerView = new BannerView(_bannerID, AdSize.Banner, AdPosition.Bottom);
    }
    
    public void LoadAd()
    {
        if(_bannerView == null)
        {
            CreateBannerView();
        }

        var adRequest = new AdRequest();
        _bannerView.LoadAd(adRequest);
    }
}
