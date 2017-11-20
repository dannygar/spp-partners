/*--------------------------------------------------------------------------------------------- 
 *  Copyright (c) Microsoft Corporation. All rights reserved. 
 *  Licensed under the MIT License. See LICENSE in the project root for license information. 
 *--------------------------------------------------------------------------------------------*/ 
﻿using Ninject;
using SportsDataLoader.Shared.Interfaces;

namespace SportsDataLoader.Shared
{
    public class Directory<T> : IDirectory<T>

    {
        private readonly IKernel kernel;

        public Directory(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T this[string name] => kernel.Get<T>(name);
    }
}