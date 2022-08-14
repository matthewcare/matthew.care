using System.Text.RegularExpressions;
using MatthewDotCare.XStatic.Generator.Storage;
using Umbraco.Cms.Core.Web;

namespace MatthewDotCare.XStatic.Generator.Transformers
{
    public class CroppedImageUrlTransformer : ITransformer
    {
        private readonly IImageCropNameGenerator _imageCropNameGenerator;
        private readonly IEnumerable<Crop> _crops;

        public CroppedImageUrlTransformer(IImageCropNameGenerator imageCropNameGenerator, IEnumerable<Crop> crops)
        {
            _imageCropNameGenerator = imageCropNameGenerator;
            _crops = crops;
        }

        public string Transform(string input, IUmbracoContext context)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (_crops?.Any() != true) return input;

            var updatedMarkup = input;

            foreach (var crop in _crops)
            {
                var imagesRegex = new Regex($"/media/.*(width={crop.Width}).*(height={crop.Height})");

                var matches = imagesRegex.Matches(updatedMarkup);

                foreach(Match match in matches)
                {
                    var str = match.ToString();

                    var partialPath = str.Split('?').First().Trim('\'', '\"');
                    var fileExtension = Path.GetExtension(partialPath);

                    var fileName = Path.GetFileName(str);
                    var newName = _imageCropNameGenerator.GetCropFileName(Path.GetFileNameWithoutExtension(str), crop);
                    
                    var newPartialPath = newName.Split('?').First().Trim('\'', '\"');

                    updatedMarkup = updatedMarkup.Replace(fileName, newPartialPath + fileExtension);
                }
            }

            return updatedMarkup;
        }
    }
}