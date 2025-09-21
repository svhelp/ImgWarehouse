﻿using ImgWarehouse.Logging;
using ImgWarehouse.Models;
using Microsoft.Extensions.Logging;

namespace ImgWarehouse.Core
{
    internal class MainProcessor
    {
        static readonly string[] SupportedImageExtensions = [".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp"];

        public MainProcessor(ProcessorConfig config)
        {
            Config = config;
        }

        private ILogger Logger { get; set; } = LoggerServiceLocator.CreateLogger<MainProcessor>();

        private ContactList ContactList { get; set; } = new ContactList();

        private Archiver Archiver { get; set; } = new Archiver();

        private ProcessorConfig Config { get; set; }

        public bool Process(int contactListSize, int chunkSize, bool recoursive)
        {

            string baseDirectory = Directory.GetCurrentDirectory();

            Logger.LogDebug($"Base Directory: {baseDirectory}");

            foreach (var subDir in Directory.GetDirectories(baseDirectory))
            {
                ProcessDirectory(baseDirectory, subDir);
            }

            return true;
        }

        private void TraverseDirectories(string directoryPath, Action<string> action, bool recoursive)
        {
            foreach (var subDir in Directory.GetDirectories(directoryPath))
            {
                action(directoryPath);

                if (recoursive)
                {
                    TraverseDirectories(subDir, action, recoursive);
                }
            }
        }

        private void ProcessDirectory(string baseDirectory, string directoryPath)
        {
            Logger.LogDebug($"Processing directory: {directoryPath}");

            var images = Directory.EnumerateFiles(directoryPath)
                .Where(file => SupportedImageExtensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                .ToList();

            if (!images.Any())
            {
                Logger.LogWarning($"No images found images in directory: {directoryPath}");

                return;
            }

            ContactList.Create(images, Path.Combine(baseDirectory, $"{directoryPath}.jpg"));

            Archiver.ArchiveDirectory(Config.ArchiveConfig, directoryPath);

            Logger.LogInformation($"Directory successfully processed: {directoryPath}");
        }
    }
}
