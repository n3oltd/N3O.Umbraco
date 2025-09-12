﻿using N3O.Umbraco.Cloud.Platforms.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Platforms;

public interface IPlatformsPageAccessor {
    Task<FoundPlatformsPage> GetAsync();
}