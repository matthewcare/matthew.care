﻿namespace MatthewDotCare.XStatic.Generator.Jobs
{
    public class JobRunner
    {
        private readonly IGenerator _generator;

        public JobRunner(IGenerator generator)
        {
            _generator = generator;
        }

        public async Task<IEnumerable<string>> RunJob(Job job)
        {
            var returnList = new List<string>();

            foreach (var id in job.PageIds)
            {
                returnList.Add(await _generator.GeneratePage(id, job.StaticSiteId, job.NameGenerator, job.Transformers));
            }

            foreach (var id in job.MediaIds)
            {
                returnList.Add(await _generator.GenerateMedia(id, job.StaticSiteId, job.MediaCropSizes));
            }

            foreach (var folder in job.Folders)
            {
                returnList.AddRange(await _generator.GenerateFolder(folder, job.StaticSiteId));
            }

            foreach (var file in job.Files)
            {
                returnList.Add(await _generator.GenerateFile(file, job.StaticSiteId));
            }

            return returnList.Where(x => x != null);
        }
    }
}