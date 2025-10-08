using System;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.Blocks;

namespace N3O.Umbraco.Blocks;

public interface IBlockPreviewer {
    Task<string> PreviewBlockAsync(Guid blockId, string contentTypeAlias, BlockEditorData blockEditorData);
}