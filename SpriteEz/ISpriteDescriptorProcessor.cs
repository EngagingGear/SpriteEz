namespace SpriteEz
{
    internal interface ISpriteDescriptorProcessor
    {
        ProcessedImageData Process(ImageDescriptor imageSet, Config config);
    }
}