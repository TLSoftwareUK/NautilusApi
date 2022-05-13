using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class DrawingIssues : IDrawingIssues
    {
        public List<SupportFile> PreviewDrawings = new List<SupportFile>();

        IReadOnlyList<SupportFile> IDrawingIssues.PreviewDrawings => PreviewDrawings.OrderBy(pd => pd.Filename).ToList();

        public void ClearPreviews()
        {
            PreviewDrawings.Clear();
        }
    }
}
