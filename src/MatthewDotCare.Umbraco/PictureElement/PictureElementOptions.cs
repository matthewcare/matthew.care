namespace MatthewDotCare.Umbraco.PictureElement;

public class PictureElementOptions
{
    public Dictionary<string, int>? Breakpoints { get; set; }
    public int DefaultQuality { get; set; }
    public string? ConvertTo { get; set; }
}