public struct GunInfo
{
    private string _name;
    private int _ammoInMag;
    private int _ammoLeft;

    public string Name => _name;
    public int AmmoInMag => _ammoInMag;
    public int AmmoLeft => _ammoLeft;

    public GunInfo(string name, int ammoInMag, int ammoLeft)
    {
        _name = name;
        _ammoInMag = ammoInMag;
        _ammoLeft = ammoLeft;
    }
}
