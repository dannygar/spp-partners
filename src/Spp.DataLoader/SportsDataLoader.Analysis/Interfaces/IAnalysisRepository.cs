/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using System.Threading.Tasks;

namespace SportsDataLoader.FileProcessing.Interfaces
{
    public interface IAnalysisRepository
    {
        Task DeleteAnalysis(string analysisId);
        Task UpsertAnalysis(Model.Analysis analysis);
        Task<Model.Analysis> GetAnalysis(string analysisId);
        Task<Model.Analysis> GetAnalysisByFileId(string fileId);
    }
}