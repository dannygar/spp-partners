/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Models;

namespace SportsDataLoader.FileProcessing.Parsers
{
    public class ZipFileParser : IZipFileParser
    {
        public Task<ZipFile> Parse(Stream fileStream)
        {
            var zipFile = new ZipFile();

            using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                foreach (var archiveEntry in zipArchive.Entries)
                {
                    if (archiveEntry.Length > 0)
                    {
                        var entryStream = new MemoryStream();
                        var archiveEntryStream = archiveEntry.Open();

                        archiveEntryStream.CopyTo(entryStream);

                        zipFile.Entries.Add(archiveEntry.FullName, entryStream);
                    }
                }
            }

            return Task.FromResult(zipFile);
        }
    }
}