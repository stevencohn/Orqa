namespace River.Orqa.Editor
{
    using System;
    using System.Collections;
    using System.Drawing;

    public interface ILineStylesEx : IList, ICollection, IEnumerable
    {
        // Methods
        int AddLineStyle();

        int AddLineStyle(string Name, Color ForeColor, Color BackColor, int ImageIndex, LineStyleOptions Options);

        void Assign(ILineStylesEx Source);

        ILineStyle GetLineStyle(int Index);

        int IndexOfName(string Name);

    }
}

