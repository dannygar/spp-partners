/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject.Modules;
using SportsDataLoader.FileProcessing.Interfaces;
using SportsDataLoader.FileProcessing.Parsers;

namespace SportsDataLoader.MessageProcessor.Console.Modules
{
    public class ParserModule : NinjectModule
    {
        public override void Load()
        {
            LoadFileParsers();
            LoadDataTableParsers();
        }

        private void LoadFileParsers()
        {
            Bind<ICsvFileParser>()
                .To<CsvFileParser>()
                .InTransientScope();

            Bind<ITsvFileParser>()
                .To<TsvFileParser>()
                .InTransientScope();

            Bind<IXlsxFileParser>()
                .To<XlsxFileParser>()
                .InTransientScope();

            Bind<IZipFileParser>()
                .To<ZipFileParser>()
                .InTransientScope();
        }

        private void LoadDataTableParsers()
        {
            Bind<IDelimitedTextFileDataTableParser>()
                .To<DelimitedTextFileDataTableParser>()
                .InTransientScope();

            Bind<IXlsxFileDataTableParser>()
                .To<XlsxFileDataTableParser>()
                .InTransientScope();
        }
    }
}