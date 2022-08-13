namespace MatthewDotCare.XStatic.Generator.Storage
{
    public interface IImageCropNameGenerator
    {
        string GetCropFileName(string fileNameWithoutExtension, Crop crop);
    }
}