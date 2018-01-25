namespace River.Orqa.Editor
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Design;

    public class Braces : IBraceMatchingEx, IBraceMatching
    {
        // Methods
        public Braces(ISyntaxEdit Owner)
        {
            this.bracesColor = EditConsts.DefaultBracesForeColor;
            this.useRoundRect = false;
            this.bracesStyle = EditConsts.DefaultBracesFontStyle;
            this.owner = Owner;
        }

        public void Assign(IBraceMatchingEx Source)
        {
            this.BracesColor = Source.BracesColor;
            this.BracesStyle = Source.BracesStyle;
            this.OpenBraces = Source.OpenBraces;
            this.ClosingBraces = Source.ClosingBraces;
            this.BracesOptions = Source.BracesOptions;
            this.UseRoundRect = Source.UseRoundRect;
        }

        public bool FindClosingBrace(ref Point Position)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindClosingBrace(ref Position);
        }

        public bool FindClosingBrace(ref int X, ref int Y)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindClosingBrace(ref X, ref Y);
        }

        public bool FindOpenBrace(ref Point Position)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindOpenBrace(ref Position);
        }

        public bool FindOpenBrace(ref int X, ref int Y)
        {
            if (this.owner == null)
            {
                return false;
            }
            return this.owner.Source.FindOpenBrace(ref X, ref Y);
        }

        public virtual void ResetBracesColor()
        {
            this.BracesColor = EditConsts.DefaultBracesForeColor;
        }

        public virtual void ResetBracesOptions()
        {
            this.BracesOptions = River.Orqa.Editor.BracesOptions.None;
        }

        public virtual void ResetBracesStyle()
        {
            this.BracesStyle = EditConsts.DefaultBracesFontStyle;
        }

        public virtual void ResetClosingBraces()
        {
            this.ClosingBraces = EditConsts.DefaultClosingBraces;
        }

        public virtual void ResetOpenBraces()
        {
            this.OpenBraces = EditConsts.DefaultOpenBraces;
        }

        public virtual void ResetUseRoundRect()
        {
            this.UseRoundRect = false;
        }

        public bool ShouldSerializeBracesColor()
        {
            return (this.bracesColor != EditConsts.DefaultBracesForeColor);
        }

        public bool ShouldSerializeBracesStyle()
        {
            return (this.bracesStyle != EditConsts.DefaultBracesFontStyle);
        }

        public bool ShouldSerializeClosingBraces()
        {
            if (((SyntaxEdit) this.owner).ShouldSerializeSourceProps())
            {
                return !this.ClosingBraces.Equals(EditConsts.DefaultClosingBraces);
            }
            return false;
        }

        public bool ShouldSerializeOpenBraces()
        {
            if (((SyntaxEdit) this.owner).ShouldSerializeSourceProps())
            {
                return !this.OpenBraces.Equals(EditConsts.DefaultOpenBraces);
            }
            return false;
        }

        public void TempHighlightBraces(Rectangle[] Rectangles)
        {
            if (this.owner != null)
            {
                this.owner.Source.TempHighlightBraces(Rectangles);
            }
        }

        public void TempUnhighlightBraces()
        {
            if (this.owner != null)
            {
                this.owner.Source.TempUnhighlightBraces();
            }
        }


        // Properties
        public Color BracesColor
        {
            get
            {
                return this.bracesColor;
            }
            set
            {
                if (this.bracesColor != value)
                {
                    this.bracesColor = value;
                    if ((this.BracesOptions != River.Orqa.Editor.BracesOptions.None) && (this.owner != null))
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        [Editor("QWhale.Design.FlagEnumerationEditor, River.Orqa.Editor", typeof(UITypeEditor)), DefaultValue(0)]
        public River.Orqa.Editor.BracesOptions BracesOptions
        {
            get
            {
                if (this.owner == null)
                {
                    return River.Orqa.Editor.BracesOptions.None;
                }
                return this.owner.Source.BracesOptions;
            }
            set
            {
                if (this.owner != null)
                {
                    this.owner.Source.BracesOptions = value;
                }
            }
        }

        public FontStyle BracesStyle
        {
            get
            {
                return this.bracesStyle;
            }
            set
            {
                if (this.bracesStyle != value)
                {
                    this.bracesStyle = value;
                    if ((this.BracesOptions != River.Orqa.Editor.BracesOptions.None) && (this.owner != null))
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }

        public char[] ClosingBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return null;
                }
                return this.owner.Source.ClosingBraces;
            }
            set
            {
                if (this.owner != null)
                {
                    this.owner.Source.ClosingBraces = value;
                }
            }
        }

        public char[] OpenBraces
        {
            get
            {
                if (this.owner == null)
                {
                    return null;
                }
                return this.owner.Source.OpenBraces;
            }
            set
            {
                if (this.owner != null)
                {
                    this.owner.Source.OpenBraces = value;
                }
            }
        }

        [DefaultValue(false)]
        public bool UseRoundRect
        {
            get
            {
                return this.useRoundRect;
            }
            set
            {
                if (this.useRoundRect != value)
                {
                    this.useRoundRect = value;
                    if ((this.BracesOptions != River.Orqa.Editor.BracesOptions.None) && (this.owner != null))
                    {
                        this.owner.Invalidate();
                    }
                }
            }
        }


        // Fields
        private Color bracesColor;
        private FontStyle bracesStyle;
        private ISyntaxEdit owner;
        private bool useRoundRect;
    }
}

