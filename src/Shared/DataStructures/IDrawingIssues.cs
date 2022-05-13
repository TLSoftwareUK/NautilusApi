using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public interface IDrawingIssues
    {
        IReadOnlyList<SupportFile> PreviewDrawings { get; }

        void ClearPreviews();
    }
}
