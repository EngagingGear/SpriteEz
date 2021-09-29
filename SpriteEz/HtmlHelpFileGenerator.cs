﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SpriteEzNs
{
    public class HtmlHelpFileGenerator
    {
        private readonly Logger _logger;

        public HtmlHelpFileGenerator(Logger logger)
        {
            _logger = logger;
        }

        private const string FileTemplate = @"
            <!doctype html>
            <html>
            <head>
            <meta charset=""utf-8"">
            <title>Sprite Help</title>
            $StyleLinkTemplate
            <style>
                .title {
                    margin: 15px;
                    margin-bottom: 35px;
                    text-align: center;
                    font-size: 22px;
                    font-weight: bold;
                }
                .table {
                    display: flex;
                    flex-direction: column;
                }
                .row{
                    display: flex;
                    flex-direction: row;
                    align-content: center;
                    justify-content: start;
                }
                .image-cell {
                    flex: 1 1;
                    display: flex;
                    flex-direction: column;
                    align-items: center;
                    margin-right: 10px;
                    margin-bottom: 5px;
                
                }
                .column-header {
                    flex: 1 1;
                    align-items: center;
                    margin-right: 10px;
                    margin-bottom: 5px;
                    text-align: center;
                    font-size: 16px;
                    font-weight: bold;
                }
                .hint-cell {
                    margin-top: 10px;
                    cursor: pointer;
                }
                .hint-cell:hover {
                    background-color: lightgray;
                }
                hr {
                    width: 100%;
                }
                $ImageStyleTemplate
            </style>
            </head>
            <body>
                <script>
                    function copyToClipboard(id) {
                        var el = document.getElementById(id);
                        if(document.selection) {
                            var range = document.bogy.createTextRange();
                            range.moveToElementText(el);
                            range.select().createTextRange();
                            document.execCommand('copy');
                        } else {
                            var range = document.createRange();
                            range.selectNodeContents(el);
                            window.getSelection().removeAllRanges();
                            window.getSelection().addRange(range);
                            try {
                                var success = document.execCommand('copy');
                                var msg = success ? 'successful' : 'unsuccessful';
                                console.log('Copying text command was ' + msg);
                            } catch(err) {
                                console.error('unable to copy ', err);
                            }

                            window.getSelection().removeAllRanges();
                        }
                        alert('Text has been copied to clipboard');
                    }
                </script>
                $BodyTemplate    
            </body>
            </html>
        ";

        private const string StyleLinkTemplate = @"
			<link rel=""stylesheet"" href=""$File"">
        ";

        private const string ImageStyleTemplate = @"
			.image {
				background-image: url('./$File') !important;
				background-repeat: no-repeat !important;
			}
        ";

        private const string BodyTemplate = @"
            $TitleTemplate
            $TableTemplate
        ";

        private const string TitleTemplate = @"
            <div class=""title"">
                The example usage of generated image sprite
            </div>
        ";

        private const string TableTemplate = @"
            <div class=""table"">
                $RowsTemplate
            </div>
        ";

        private const string RowTemplate = @"
            <div class=""row"">
                $ImageCellsTemplate
            </div>
            <hr/>    
        ";

        private const string ColumnHeaderTemplate = @"
            <div class=""column-header"">
                $ColumnName
            </div>
        ";

        private const string ImageCellTemplate = @"
            <div class=""image-cell"">
                <div class=""$ImageCellTemplate""></div>
                <div id=""$HintId"" class=""hint-cell"" onclick=""copyToClipboard('$HintId');return false;"">
                    $HintCellTemplate
                </div>
            </div>
        ";

        private const string DivMarkupTemplate = @"
            &lt;div class=""$ClassContent""/&gt;
        ";

        public void GenerateFile(List<CssEntry> cssEntries, Config config)
        {
            var spritePath = config.SpriteImgFile;
            var spriteCssFile = config.SpriteCssFile;
            var path = FileUtils.GetDestinationPath(config.OutputDirectory, config.HelpFile);

            var bodyText = BodyTemplate
                .Replace("$TitleTemplate", TitleTemplate)
                .Replace("$TableTemplate", GenerateTable(cssEntries, config));

            var styleLinkText = StyleLinkTemplate.Replace("$File", spriteCssFile);
            var imageStyleText = ImageStyleTemplate.Replace("$File", spritePath);

            var content = FileTemplate
                .Replace("$StyleLinkTemplate", styleLinkText)
                .Replace("$ImageStyleTemplate", imageStyleText)
                .Replace("$BodyTemplate", bodyText);
            SaveFile(path, content);
        }

        protected virtual void SaveFile(string path, string content)
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
                File.WriteAllText(path, content);
            }
            catch (Exception exception)
            {
                _logger.Log($"Cannot save html help file {path}. Error message: {exception.Message}");
            }
        }

        private string GenerateTable(List<CssEntry> cssEntries, Config config)
        {
            var sb = new StringBuilder();
            sb.Append(GenerateHeaderRow(config));
            foreach (var cssEntry in cssEntries)
            {
                sb.Append(GenerateRow(cssEntry, config));
            }

            return TableTemplate.Replace("$RowsTemplate", sb.ToString());
        }

        private string GenerateHeaderRow(Config config)
        {
            var sb = new StringBuilder();

            //cell for combined image styles
            sb.Append(ColumnHeaderTemplate.Replace("$ColumnName", "Combined Image"));

            //cell for normal image styles
            sb.Append(ColumnHeaderTemplate.Replace("$ColumnName", "Normal Image"));

            //cell for highlighted image styles
            if (config.GenerateHighlight)
            {
                sb.Append(ColumnHeaderTemplate.Replace("$ColumnName", "Hihglighted Image"));
            }

            //cell for disabled image styles
            if (config.GenerateDisabled)
            {
                sb.Append(ColumnHeaderTemplate.Replace("$ColumnName", "Disabled Image"));
            }

            return RowTemplate.Replace("$ImageCellsTemplate", sb.ToString());
        }

        private string GenerateRow(CssEntry cssEntry, Config config)
        {
            return RowTemplate.Replace("$ImageCellsTemplate", GenerateRowImages(cssEntry, config));
        }

        private string GenerateRowImages(CssEntry cssEntry, Config config)
        {
            var sb = new StringBuilder();
            
            //cell for combined image styles
            sb.Append(ImageCellTemplate
                .Replace("$ImageCellTemplate", $"{config.ImageClass} {cssEntry.ImgName}"))
                .Replace("$HintId", GenerateHintId())
                .Replace("$HintCellTemplate", GenerateHint(cssEntry, config, null)
            );

            //cell for normal image styles
            sb.Append(ImageCellTemplate
                .Replace("$ImageCellTemplate", $"{config.ImageClass} {cssEntry.ImgName}{config.NormalCssSuffix}"))
                .Replace("$HintId", GenerateHintId())
                .Replace("$HintCellTemplate", GenerateHint(cssEntry, config, config.NormalCssSuffix));

            //cell for highlighted image styles
            if (config.GenerateHighlight)
            {
                sb.Append(ImageCellTemplate
                        .Replace("$ImageCellTemplate", $"{config.ImageClass} {cssEntry.ImgName}{config.HighlightCssSuffix}"))
                    .Replace("$HintId", GenerateHintId())
                    .Replace("$HintCellTemplate", GenerateHint(cssEntry, config, config.HighlightCssSuffix));
            }

            //cell for disabled image styles
            if (config.GenerateDisabled)
            {
                sb.Append(ImageCellTemplate
                        .Replace("$ImageCellTemplate", $"{config.ImageClass} {cssEntry.ImgName}{config.DisabledCssSuffix}"))
                        .Replace("$HintId", GenerateHintId())
                        .Replace("$HintCellTemplate", GenerateHint(cssEntry, config, config.DisabledCssSuffix));
            }

            return sb.ToString();
        }

        private string GenerateHint(CssEntry cssEntry, Config config, string suffix)
        {
            var sb = new StringBuilder();
            sb.Append(string.IsNullOrWhiteSpace(suffix)
                ? DivMarkupTemplate.Replace("$ClassContent", $"{config.ImageClass} {cssEntry.ImgName}")
                : DivMarkupTemplate.Replace("$ClassContent", $"{config.ImageClass} {cssEntry.ImgName}{suffix}"));

            return sb.ToString();
        }

        private string GenerateHintId()
        {
            return Guid.NewGuid().ToString()
                .Replace(" ", "")
                .Replace("-", "");
        }
    }
}