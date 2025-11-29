using ImgWarehouse.Models;

namespace ImgWarehouse.Core.Processors;

internal class DirectoryProcessorFactory
{
    public DirectoryProcessor CreateProcessor(ProcessorConfig config)
    {
        switch (config.NestingHanglerType)
        {
            case NestingHandlerType.Recoursive:
                return new RecoursiveProcessor(config);
            case NestingHandlerType.SingleLevel:
                return new SingleLevelProcessor(config);
            case NestingHandlerType.RecoursiveSplit:
                return new RecoursiveSplitProcessor(config);
        }

        throw new NotSupportedException($"Nesting handler type '{config.NestingHanglerType}' is not supported.");
    }
}
