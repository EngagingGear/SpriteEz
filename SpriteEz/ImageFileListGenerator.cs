using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SpriteEzNs
{
    public class ImageFileListGenerator
    {
        private readonly Logger _logger;
        public ImageFileListGenerator(Logger logger)
        {
            _logger = logger;
        }
        public List<ImgFile> GenerateImageFileNames(List<string> fileList, Config config)
        {
            _logger.Log($"Files to process: {string.Join(",", fileList)}");

            List<string> files = new();
            foreach (var fileShort in fileList)
            {
                var file = Path.GetFullPath(fileShort);

                if (fileShort.Contains("*") || fileShort.Contains("?"))
                {
                    var wildcard = Path.GetFileName(fileShort);

                    var directoryName = Path.GetDirectoryName(file);
                    if (directoryName == null)
                    {
                        throw new InvalidOperationException();
                    }
                    if (string.IsNullOrWhiteSpace(config.OutputDirectory))
                    {
                        config.OutputDirectory = directoryName;
                    }
                    var filesBySearchPattern = Directory.GetFiles(directoryName, wildcard);
                    files.AddRange(filesBySearchPattern);
                }
                else
                {
                    files.Add(file);
                }
            }

            var normalFiles = new HashSet<string>();
            var highlightFiles = new HashSet<string>();
            var disabledFiles = new HashSet<string>();
            foreach (var file in files)
            {
                var baseName = Path.GetFileNameWithoutExtension(file);
                if (IsOutputFile(baseName, config))
                {
                    continue;
                }

                if (!ImageUtils.IsSupportedImageExtension(file))
                {
                    _logger.Log($"The file extension {Path.GetExtension(file)} is not supported. File: {file} will be skipped");
                    continue;
                }
                if (ImageUtils.IsAnimatedImage(file))
                {
                    _logger.Log($"File: {file} is animated and will be skipped");
                    continue;
                }


                if (baseName.ToLower().EndsWith(config.HighlightSuffix.ToLower()))
                    highlightFiles.Add(file);
                else if (baseName.ToLower().EndsWith(config.DisabledSuffix.ToLower()))
                    disabledFiles.Add(file);
                else
                    normalFiles.Add(file);
            }

            var imgFiles = new List<ImgFile>();
            foreach (var file in normalFiles)
            {
                var bright = AddExtension(file, config.HighlightSuffix);
                var gray = AddExtension(file, config.DisabledSuffix);
                if (!highlightFiles.Contains(bright))
                    bright = null;
                else
                    highlightFiles.Remove(bright);
                if (!disabledFiles.Contains(gray))
                    gray = null;
                else
                    disabledFiles.Remove(gray);

                imgFiles.Add(new ImgFile
                {
                    HighlightImgFilename = bright,
                    GrayImageFilename = gray,
                    NormalImgFilename = file,
                    ImgName = Path.GetFileNameWithoutExtension(file)
                });
            }

            foreach (var file in highlightFiles)
            {
                imgFiles.Add(new ImgFile
                {
                    HighlightImgFilename = null,
                    GrayImageFilename = null,
                    NormalImgFilename = file,
                    ImgName = Path.GetFileNameWithoutExtension(file)
                });
            }
            foreach (var file in disabledFiles)
            {
                imgFiles.Add(new ImgFile
                {
                    HighlightImgFilename = null,
                    GrayImageFilename = null,
                    NormalImgFilename = file,
                    ImgName = Path.GetFileNameWithoutExtension(file)
                });
            }

            return imgFiles;
        }

        private bool IsOutputFile(string baseName, Config config)
        {
            var outputSprite = Path.GetFileNameWithoutExtension(config.SpriteImgFile);

            if (outputSprite != null && outputSprite.Contains(baseName))
            {
                return true;
            }

            return false;
        }

        private static string AddExtension(string filename, string suffix)
        {
            return Path.Combine(Path.GetDirectoryName(filename) ?? string.Empty,
                Path.GetFileNameWithoutExtension(filename) + suffix + Path.GetExtension(filename));
        }
    }
}