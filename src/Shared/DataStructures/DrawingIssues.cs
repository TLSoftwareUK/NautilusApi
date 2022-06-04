using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class DrawingIssues : IDrawingIssues
    {
        public List<DrawingIssueFile> PreviewDrawings { get; set; }

        IReadOnlyList<DrawingIssueFile> IDrawingIssues.PreviewDrawings => PreviewDrawings.OrderBy(pd => pd.DrawingNumber).ToList();

        public DrawingIssues()
        {
            PreviewDrawings = new List<DrawingIssueFile>();
        }

        public void ClearPreviews()
        {
            PreviewDrawings.Clear();
        }
    }
}
