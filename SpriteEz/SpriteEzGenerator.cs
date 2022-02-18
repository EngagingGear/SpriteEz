using System.Collections.Generic;

namespace SpriteEz
{
    /// <summary>
    ///     Simple class to generate the sprite and css file.
    /// </summary>
    public class SpriteEzGenerator
    {
        private readonly Config _config;

        private readonly IImageDataSetBuilder _imageDataSetBuilder;
        private readonly List<ImgFile> _imgFiles;
        private readonly Logger _logger;
        private readonly IOutputWriter _outputWriter;
        private readonly ISpriteDescriptorProcessor _processor;

        /// <summary>
        ///     Constructor which does all the work.
        /// </summary>
        /// <param name="imgFiles">Image files to process</param>
        /// <param name="config">Options for generation</param>
        /// <param name="logger"></param>
        public SpriteEzGenerator(List<ImgFile> imgFiles, Config config, Logger logger)
        {
            _imgFiles = imgFiles;
            _config = config;
            _logger = logger;

            _imageDataSetBuilder = new ImageDataSetBuilder();
            _processor = GetProcessor(_config.Embedded);
            _outputWriter = GetOutputWriter(_config.Embedded);
        }

        public void Generate()
        {
            //Prepare data for processing
            var dataSet = _imageDataSetBuilder.Build(_imgFiles, _config);


            //Process data and get processing results
            var results = new List<ProcessedImageData>();

            foreach (var imageSet in dataSet.Items)
            {
                results.Add(_processor.Process(imageSet, _config));
            }

            //Write results
            _outputWriter.WriteResults(results, _config);
        }

        private ISpriteDescriptorProcessor GetProcessor(bool embedded)
        {
            return embedded ? new EmbeddedProcessor(_logger) : new SpriteProcessor(_logger);
        }

        private IOutputWriter GetOutputWriter(bool embedded)
        {
            return new OutputWriter(_logger, embedded);
        }
    }
}