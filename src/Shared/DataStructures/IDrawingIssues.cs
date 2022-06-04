using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public interface IDrawingIssues
    {
        IReadOnlyList<DrawingIssueFile> PreviewDrawings { get; }

        void ClearPreviews();
    }
}
