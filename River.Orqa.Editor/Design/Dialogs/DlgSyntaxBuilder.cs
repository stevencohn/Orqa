namespace River.Orqa.Editor.Design.Dialogs
{
    using River.Orqa.Editor.Common;
    using River.Orqa.Editor.Design;
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class DlgSyntaxBuilder : Form
    {
        // Methods
        public DlgSyntaxBuilder()
        {
            this.updating = false;
            this.sRemoveScroll = new string('x', 100);
            this.cFontSize = 8.25f;
            this.InitializeComponent();
            this.lexer = new Lexer();
            this.rootNode = this.tvLexer.Nodes[0];
            this.generalNode = this.rootNode.Nodes[0];
            this.stylesNode = this.rootNode.Nodes[1];
            this.statesNode = this.rootNode.Nodes[2];
        }

        public DlgSyntaxBuilder(SyntaxBuilderEditor editor) : this()
        {
            this.scriptEditor = editor;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.TopLevel = true;
            base.ShowInTaskbar = false;
        }

        private void AddBlockClick(object sender, EventArgs e)
        {
            ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            string text1 = this.GetNewSyntaxBlockName(state1);
            ILexSyntaxBlock block1 = state1.AddLexSyntaxBlock();
            block1.Name = text1;
            TreeNode node1 = this.GetStateNode(this.tvLexer.SelectedNode);
            if (node1 != null)
            {
                node1.Nodes.Add(new TreeNode(text1));
            }
            node1.Expand();
            this.tvLexer.SelectedNode = node1.LastNode;
        }

        private void AddResWordSetClick(object sender, EventArgs e)
        {
            ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            string text1 = this.GetNewResWordSetName(block1);
            int num1 = block1.AddLexResWordSet();
            block1.LexResWordSets[num1].Name = text1;
            TreeNode node1 = this.GetSyntaxBlockNode(this.tvLexer.SelectedNode);
            if (node1 != null)
            {
                node1.Nodes.Add(new TreeNode(text1));
            }
            node1.Expand();
            this.tvLexer.SelectedNode = node1.LastNode;
        }

        private void AddStateClick(object sender, EventArgs e)
        {
            string text1 = this.GetNewStateName();
            ILexState state1 = this.Scheme.AddLexState();
            TreeNode node1 = new TreeNode(text1);
            node1.ImageIndex = 4;
            node1.SelectedImageIndex = 4;
            this.statesNode.Nodes.Add(node1);
            this.statesNode.Expand();
            this.tvLexer.SelectedNode = this.statesNode.LastNode;
            state1.Name = text1;
            this.UpdateStates(this.tvLexer.SelectedNode.Index);
        }

        private void AddStyleClick(object sender, EventArgs e)
        {
            string text1 = this.GetNewStyleName();
            ILexStyle style1 = this.Scheme.AddLexStyle();
            style1.FontStyle = FontStyle.Regular;
            style1.ForeColor = Color.Black;
            this.stylesNode.Nodes.Add(new TreeNode(text1));
            this.stylesNode.Expand();
            this.tvLexer.SelectedNode = this.stylesNode.LastNode;
            style1.Name = text1;
            this.UpdateLexStyles();
        }

        private void btBlockEdit_Click(object sender, EventArgs e)
        {
            this.tbBlockName.Focus();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.Cancel;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            this.ClearScheme();
        }

        private void btColor_Click(object sender, EventArgs e)
        {
            if ((this.colorDialog1.ShowDialog() == DialogResult.OK) && (this.GetNodeKind(this.tvLexer.SelectedNode) == NodeKind.nkStyle))
            {
                ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
                style1.ForeColor = this.colorDialog1.Color;
            }
        }

        private void btLoadScheme_Click(object sender, EventArgs e)
        {
            this.LoadScheme();
        }

        private void btOk_Click(object sender, EventArgs e)
        {
            base.DialogResult = DialogResult.OK;
        }

        private void btResWordSetEdit_Click(object sender, EventArgs e)
        {
            this.tbResWordSetName.Focus();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            this.SaveScheme();
        }

        private void btStateEdit_Click(object sender, EventArgs e)
        {
            this.tbStateName.Focus();
        }

        private void btStyleEdit_Click(object sender, EventArgs e)
        {
            this.tbStyleName.Focus();
        }

        private void cbBlockExpr_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) && (this.cbBlockExpr.Text != string.Empty))
            {
                string text1 = this.GetSampleExpression();
                ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                if (block1 != null)
                {
                    if (this.lbBlockExpr.SelectedIndex == -1)
                    {
                        this.lbBlockExpr.Items.Add(text1);
                        this.lbBlockExpr.SelectedIndex = this.lbBlockExpr.Items.Count - 1;
                    }
                    else
                    {
                        this.lbBlockExpr.Items[this.lbBlockExpr.SelectedIndex] = text1;
                    }
                }
                this.UpdateBlockExpressions();
            }
        }

        private void cbLeaveState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                if ((block1 != null) && (this.cbLeaveState.SelectedIndex >= 0))
                {
                    block1.LeaveState = this.Scheme.States[this.cbLeaveState.SelectedIndex];
                }
            }
        }

        private void cbResWordStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                ILexResWordSet set1 = this.GetLexResWordSet(this.tvLexer.SelectedNode, this.Scheme);
                if ((set1 != null) && (this.cbResWordStyle.SelectedIndex >= 0))
                {
                    set1.ResWordStyle = this.Scheme.Styles[this.cbResWordStyle.SelectedIndex];
                }
                if (this.cbResWordStyle.SelectedIndex < 0)
                {
                    this.lbReswords.Font = new Font(this.lbReswords.Font.Name, this.cFontSize, FontStyle.Regular);
                }
            }
        }

        private void cbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                if ((block1 != null) && (this.cbStyle.SelectedIndex >= 0))
                {
                    block1.Style = this.Scheme.Styles[this.cbStyle.SelectedIndex];
                }
                if (this.cbStyle.SelectedIndex >= 0)
                {
                    this.lbBlockExpr.Font = new Font(this.lbBlockExpr.Font.Name, this.lbBlockExpr.Font.Size, block1.Style.FontStyle);
                    this.lbBlockExpr.ForeColor = block1.Style.ForeColor;
                    this.lbBlockExpr.BackColor = block1.Style.BackColor;
                }
                else
                {
                    this.lbBlockExpr.Font = new Font(this.lbBlockExpr.Font.Name, this.lbBlockExpr.Font.Size, FontStyle.Regular);
                    this.lbBlockExpr.ForeColor = SystemColors.WindowText;
                    this.lbBlockExpr.BackColor = SystemColors.Window;
                }
            }
        }

        private void chbBold_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void chbCaseSensitive_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                NodeKind kind1 = this.GetNodeKind(this.tvLexer.SelectedNode);
                if ((kind1 == NodeKind.nkState) || (kind1 == NodeKind.nkSyntaxBlock))
                {
                    ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
                    state1.CaseSensitive = this.chbCaseSensitive.Checked;
                }
            }
        }

        private void chbCaseSensitive_TextChanged(object sender, EventArgs e)
        {
            ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            if (state1 != null)
            {
                state1.CaseSensitive = this.chbCaseSensitive.Checked;
            }
        }

        private void chbItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void chbPlainText_CheckedChanged(object sender, EventArgs e)
        {
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (style1 != null)
            {
                style1.PlainText = this.chbPlainText.Checked;
            }
        }

        private void chbStrikeout_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void chbUnderline_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.updating)
            {
                this.UpdateFontStyle();
            }
        }

        private void clbBkColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (style1 != null)
            {
                style1.BackColor = this.clbBkColor.SelectedColor;
            }
            this.UpdateSample();
        }

        private void clbForeColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (style1 != null)
            {
                style1.ForeColor = this.clbForeColor.SelectedColor;
            }
            this.UpdateSample();
        }

        private void ClearScheme()
        {
            this.Scheme.ClearScheme();
            this.statesNode.Nodes.Clear();
            this.stylesNode.Nodes.Clear();
            this.Text = SyntaxBuilderConsts.SyntaxFormCaption;
        }

        private void DeleteBlockClick(object sender, EventArgs e)
        {
            ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            this.tvLexer.SelectedNode.Remove();
            int num1 = (block1 != null) ? block1.Index : 0;
            LexState state1 = (LexState) this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            LexSyntaxBlock[] blockArray1 = new LexSyntaxBlock[state1.SyntaxBlocks.Length - 1];
            int num2 = 0;
            for (int num3 = 0; num3 < state1.GetSyntaxBlocksCount(); num3++)
            {
                if (num1 != num3)
                {
                    blockArray1[num2] = (LexSyntaxBlock) state1.GetSyntaxBlock(num3);
                    blockArray1[num2].Index = num2;
                    num2++;
                }
            }
            state1.SyntaxBlocks = blockArray1;
        }

        private void DeleteResWordSetClick(object sender, EventArgs e)
        {
            ILexResWordSet set1 = this.GetLexResWordSet(this.tvLexer.SelectedNode, this.Scheme);
            this.tvLexer.SelectedNode.Remove();
            int num1 = (set1 != null) ? set1.Index : 0;
            LexSyntaxBlock block1 = (LexSyntaxBlock) this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            LexResWordSet[] setArray1 = new LexResWordSet[block1.ResWordSets.Length - 1];
            int num2 = 0;
            for (int num3 = 0; num3 < block1.ResWordSets.Length; num3++)
            {
                if (num1 != num3)
                {
                    setArray1[num2] = block1.ResWordSets[num3];
                    setArray1[num2].Index = num2;
                    num2++;
                }
            }
            block1.ResWordSets = setArray1;
        }

        private void DeleteStateClick(object sender, EventArgs e)
        {
            ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            TreeNode node1 = this.GetStateNode(this.tvLexer.SelectedNode);
            node1.Remove();
            int num1 = (state1 != null) ? state1.Index : 0;
            LexState[] stateArray1 = new LexState[this.Scheme.States.Length - 1];
            int num2 = 0;
            for (int num3 = 0; num3 < this.Scheme.States.Length; num3++)
            {
                if (num1 != num3)
                {
                    stateArray1[num2] = this.Scheme.States[num3];
                    stateArray1[num2].Index = num2;
                    num2++;
                }
            }
            this.Scheme.States = stateArray1;
            node1 = this.GetStateNode(this.tvLexer.SelectedNode);
            this.UpdateStates((node1 != null) ? node1.Index : -1);
        }

        private void DeleteStyleClick(object sender, EventArgs e)
        {
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            TreeNode node1 = this.GetStyleNode(this.tvLexer.SelectedNode);
            if (node1 != null)
            {
                node1.Remove();
            }
            if (!this.tvLexer.Focused)
            {
                this.tvLexer.Focus();
            }
            int num1 = (style1 != null) ? style1.Index : 0;
            LexStyle[] styleArray1 = new LexStyle[this.Scheme.Styles.Length - 1];
            int num2 = 0;
            for (int num3 = 0; num3 < this.Scheme.Styles.Length; num3++)
            {
                if (num1 != num3)
                {
                    styleArray1[num2] = this.Scheme.Styles[num3];
                    styleArray1[num2].Index = num2;
                    num2++;
                }
            }
            this.Scheme.Styles = styleArray1;
            this.UpdateLexStyles();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DlgSyntaxBuilder_Closing(object sender, CancelEventArgs e)
        {
            this.DoExit();
        }

        private void DoExit()
        {
            if ((base.DialogResult == DialogResult.Cancel) && (MessageBox.Show("Exit without changes?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
            {
                base.DialogResult = DialogResult.None;
            }
        }

        private Panel GetCurrentPanel(NodeKind Kind)
        {
            switch (Kind)
            {
                case NodeKind.nkStyles:
                case NodeKind.nkStyle:
                {
                    return this.pnStyles;
                }
                case NodeKind.nkStates:
                case NodeKind.nkState:
                case NodeKind.nkSyntaxBlock:
                case NodeKind.nkResWordSet:
                {
                    return this.pnStates;
                }
                case NodeKind.nkGeneral:
                {
                    return this.pnGeneral;
                }
            }
            return this.pnGeneral;
        }

        private int GetFirstNumber(ArrayList list)
        {
            int num1 = 0;
            for (int num2 = 0; num2 < list.Count; num2++)
            {
                if (((int) list[num2]) == num1)
                {
                    num1++;
                }
            }
            return num1;
        }

        private int GetFirstNumber(string[] list, string AStart)
        {
            ArrayList list1 = new ArrayList();
            for (int num1 = 0; num1 < list.Length; num1++)
            {
                string text1 = list[num1];
                if ((text1 != null) && text1.StartsWith(AStart))
                {
                    string text2 = text1.Substring(AStart.Length);
                    try
                    {
                        int num2 = int.Parse(text2);
                        list1.Add(num2);
                    }
                    catch
                    {
                    }
                }
            }
            list1.Sort();
            int num3 = 0;
            for (int num4 = 0; num4 < list1.Count; num4++)
            {
                if (((int) list1[num4]) == num3)
                {
                    num3++;
                }
            }
            return num3;
        }

        private TreeNode GetFolderNode()
        {
            TreeNode node1 = this.tvLexer.SelectedNode;
            while (node1 != null)
            {
                if ((node1.ImageIndex != 0) && (node1.ImageIndex != 1))
                {
                    node1 = node1.Parent;
                    continue;
                }
                return node1;
            }
            return node1;
        }

        private int GetLeaveStateIndex(ILexSyntaxBlock Block)
        {
            if ((Block != null) && (Block.LeaveState != null))
            {
                for (int num1 = 0; num1 < this.cbLeaveState.Items.Count; num1++)
                {
                    if (string.Compare(this.cbLeaveState.Items[num1].ToString(), Block.LeaveState.Name) == 0)
                    {
                        return num1;
                    }
                }
            }
            return -1;
        }

        private ILexResWordSet GetLexResWordSet(TreeNode Node, LexScheme Scheme)
        {
            NodeKind kind1 = this.GetNodeKind(Node);
            if (kind1 != NodeKind.nkResWordSet)
            {
                return null;
            }
            ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(Node.Parent, Scheme);
            if (block1 == null)
            {
                return null;
            }
            return block1.LexResWordSets[Node.Index];
        }

        public ILexState GetLexState(TreeNode Node, LexScheme Scheme)
        {
            switch (this.GetNodeKind(Node))
            {
                case NodeKind.nkStates:
                {
                    if ((this.statesNode.Nodes.Count > 0) && (Scheme.States.Length > 0))
                    {
                        return Scheme.States[0];
                    }
                    return null;
                }
                case NodeKind.nkState:
                {
                    return Scheme.States[Node.Index];
                }
                case NodeKind.nkSyntaxBlock:
                {
                    return Scheme.States[Node.Parent.Index];
                }
                case NodeKind.nkResWordSet:
                {
                    return Scheme.States[Node.Parent.Parent.Index];
                }
            }
            return null;
        }

        public ILexStyle GetLexStyle(TreeNode Node, LexScheme Scheme)
        {
            switch (this.GetNodeKind(Node))
            {
                case NodeKind.nkStyles:
                {
                    if ((Node.Nodes.Count > 0) && (Scheme.Styles.Length > 0))
                    {
                        return Scheme.Styles[0];
                    }
                    return null;
                }
                case NodeKind.nkStyle:
                {
                    return Scheme.Styles[Node.Index];
                }
            }
            return null;
        }

        private ILexSyntaxBlock GetLexSyntaxBlock(TreeNode Node, LexScheme Scheme)
        {
            NodeKind kind1 = this.GetNodeKind(Node);
            if ((kind1 != NodeKind.nkSyntaxBlock) && (kind1 != NodeKind.nkResWordSet))
            {
                return null;
            }
            ILexState state1 = this.GetLexState(Node.Parent, Scheme);
            if (kind1 == NodeKind.nkResWordSet)
            {
                Node = Node.Parent;
            }
            if (state1 == null)
            {
                return null;
            }
            return ((LexState) state1).GetSyntaxBlock(Node.Index);
        }

        private string GetNewResWordSetName(ILexSyntaxBlock lexBlock)
        {
            ILexResWordSet[] setArray1 = lexBlock.LexResWordSets;
            string[] textArray1 = new string[setArray1.Length];
            for (int num1 = 0; num1 < setArray1.Length; num1++)
            {
                textArray1[num1] = setArray1[num1].Name;
            }
            int num2 = this.GetFirstNumber(textArray1, SyntaxBuilderConsts.ResWordSetText);
            return (SyntaxBuilderConsts.ResWordSetText + num2.ToString());
        }

        private string GetNewStateName()
        {
            string[] textArray1 = new string[this.Scheme.States.Length];
            for (int num1 = 0; num1 < this.Scheme.States.Length; num1++)
            {
                textArray1[num1] = this.Scheme.States[num1].Name;
            }
            int num2 = this.GetFirstNumber(textArray1, SyntaxBuilderConsts.StateText);
            return (SyntaxBuilderConsts.StateText + num2.ToString());
        }

        private string GetNewStyleName()
        {
            string[] textArray1 = new string[this.Scheme.Styles.Length];
            for (int num1 = 0; num1 < this.Scheme.Styles.Length; num1++)
            {
                textArray1[num1] = this.Scheme.Styles[num1].Name;
            }
            int num2 = this.GetFirstNumber(textArray1, SyntaxBuilderConsts.StyleText);
            return (SyntaxBuilderConsts.StyleText + num2.ToString());
        }

        private string GetNewSyntaxBlockName(ILexState lexState)
        {
            ILexSyntaxBlock[] blockArray1 = lexState.LexSyntaxBlocks;
            string[] textArray1 = new string[blockArray1.Length];
            for (int num1 = 0; num1 < blockArray1.Length; num1++)
            {
                textArray1[num1] = blockArray1[num1].Name;
            }
            int num2 = this.GetFirstNumber(textArray1, SyntaxBuilderConsts.BlockText);
            return (SyntaxBuilderConsts.BlockText + num2.ToString());
        }

        private NodeKind GetNodeKind(TreeNode Node)
        {
            int num1 = this.GetNodeLevel(Node);
            TreeNode node1 = Node.Parent;
            switch (num1)
            {
                case 0:
                {
                    return NodeKind.nkGeneral;
                }
                case 1:
                {
                    switch (Node.Index)
                    {
                        case 0:
                        {
                            return NodeKind.nkGeneral;
                        }
                        case 1:
                        {
                            return NodeKind.nkStyles;
                        }
                        case 2:
                        {
                            return NodeKind.nkStates;
                        }
                    }
                    return NodeKind.nkNone;
                }
                case 2:
                {
                    if (node1 == this.stylesNode)
                    {
                        return NodeKind.nkStyle;
                    }
                    if (node1 == this.statesNode)
                    {
                        return NodeKind.nkState;
                    }
                    return NodeKind.nkNone;
                }
                case 3:
                {
                    return NodeKind.nkSyntaxBlock;
                }
                case 4:
                {
                    return NodeKind.nkResWordSet;
                }
            }
            return NodeKind.nkNone;
        }

        private int GetNodeLevel(TreeNode Node)
        {
            if (Node == null)
            {
                return -1;
            }
            TreeNode node1 = Node;
            int num1 = -1;
            while (node1 != null)
            {
                node1 = node1.Parent;
                num1++;
            }
            return num1;
        }

        private TreeNode GetParent(TreeNode node)
        {
            TreeNode node1 = node;
            while (node1.Parent != null)
            {
                node1 = node1.Parent;
            }
            return node1;
        }

        private int GetReswordStyleIndex(ILexResWordSet resWordSet)
        {
            if ((resWordSet != null) && (resWordSet.ResWordStyle != null))
            {
                for (int num1 = 0; num1 < this.cbResWordStyle.Items.Count; num1++)
                {
                    if (string.Compare(this.cbResWordStyle.Items[num1].ToString(), resWordSet.ResWordStyle.Name) == 0)
                    {
                        return num1;
                    }
                }
            }
            return -1;
        }

        private string GetSampleExpression()
        {
            string text1 = this.cbBlockExpr.Text;
            int num1 = text1.IndexOf(" ");
            if (num1 >= 0)
            {
                text1 = text1.Remove(0, num1);
                text1 = text1.Trim();
            }
            return text1;
        }

        private TreeNode GetStateNode(TreeNode Node)
        {
            switch (this.GetNodeKind(Node))
            {
                case NodeKind.nkStates:
                {
                    if (this.statesNode.Nodes.Count > 0)
                    {
                        return Node.Nodes[0];
                    }
                    return null;
                }
                case NodeKind.nkState:
                {
                    return Node;
                }
                case NodeKind.nkSyntaxBlock:
                {
                    return Node.Parent;
                }
                case NodeKind.nkResWordSet:
                {
                    return Node.Parent.Parent;
                }
            }
            return null;
        }

        private int GetStyleIndex(ILexSyntaxBlock Block)
        {
            if ((Block != null) && (Block.Style != null))
            {
                for (int num1 = 0; num1 < this.cbStyle.Items.Count; num1++)
                {
                    if (string.Compare(this.cbStyle.Items[num1].ToString(), Block.Style.Name) == 0)
                    {
                        return num1;
                    }
                }
            }
            return -1;
        }

        private TreeNode GetStyleNode(TreeNode Node)
        {
            switch (this.GetNodeKind(Node))
            {
                case NodeKind.nkStyles:
                {
                    if (Node.Nodes.Count > 0)
                    {
                        return Node.Nodes[0];
                    }
                    return null;
                }
                case NodeKind.nkStyle:
                {
                    return Node;
                }
            }
            return null;
        }

        private TreeNode GetSyntaxBlockNode(TreeNode Node)
        {
            NodeKind kind1 = this.GetNodeKind(Node);
            if (kind1 == NodeKind.nkSyntaxBlock)
            {
                return Node;
            }
            if (kind1 == NodeKind.nkResWordSet)
            {
                return Node.Parent;
            }
            return null;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ResourceManager manager1 = new ResourceManager(typeof(DlgSyntaxBuilder));
            this.pnButtons = new Panel();
            this.btCancel = new Button();
            this.btOk = new Button();
            this.btLoadScheme = new Button();
            this.btClear = new Button();
            this.btSave = new Button();
            this.tvLexer = new TreeView();
            this.imLexer = new ImageList(this.components);
            this.pnMain = new Panel();
            this.tcPanels = new TabControl();
            this.tpGeneral = new TabPage();
            this.pnGeneral = new Panel();
            this.laCopyright = new Label();
            this.laDescription = new Label();
            this.laSchemeName = new Label();
            this.laAuthor = new Label();
            this.tbCopyright = new TextBox();
            this.tbDescription = new TextBox();
            this.tbSchemeName = new TextBox();
            this.tbAuthor = new TextBox();
            this.tpStyles = new TabPage();
            this.pnStyles = new Panel();
            this.pnStyleButtons = new Panel();
            this.btStyleDelete = new Button();
            this.ilButtons = new ImageList(this.components);
            this.btStyleEdit = new Button();
            this.btStyleAdd = new Button();
            this.pnSample = new Panel();
            this.laSample = new Label();
            this.gbStyleProperties = new GroupBox();
            this.chbPlainText = new CheckBox();
            this.clbBkColor = new ColorBox(this.components);
            this.clbForeColor = new ColorBox(this.components);
            this.tbStyleName = new TextBox();
            this.laStyleName = new Label();
            this.laStyleBkColor = new Label();
            this.laStyleForeColor = new Label();
            this.tbStyleDesc = new TextBox();
            this.laStyleDesc = new Label();
            this.gbFontStyles = new GroupBox();
            this.chbStrikeout = new CheckBox();
            this.chbItalic = new CheckBox();
            this.chbUnderline = new CheckBox();
            this.chbBold = new CheckBox();
            this.tpStates = new TabPage();
            this.pnStates = new Panel();
            this.gbReswordsProperties = new GroupBox();
            this.tbResWordSetName = new TextBox();
            this.laResWordSetName = new Label();
            this.pnReswordButtons = new Panel();
            this.btResWordSetDelete = new Button();
            this.btResWordSetEdit = new Button();
            this.btResWordSetAdd = new Button();
            this.lbReswords = new ListBox();
            this.cbResWordStyle = new ComboBox();
            this.tbResword = new TextBox();
            this.laBlockReswords = new Label();
            this.laResWordStyle = new Label();
            this.gbSyntaxBlockProperties = new GroupBox();
            this.tbBlockName = new TextBox();
            this.laBlockName = new Label();
            this.pnBlockButtons = new Panel();
            this.btBlockDelete = new Button();
            this.btBlockEdit = new Button();
            this.btBlockAdd = new Button();
            this.cbBlockExpr = new ComboBox();
            this.lbBlockExpr = new ListBox();
            this.laBlockExpr = new Label();
            this.tbBlockDesc = new TextBox();
            this.laBlockDesc = new Label();
            this.laLeaveState = new Label();
            this.laStyle = new Label();
            this.cbLeaveState = new ComboBox();
            this.cbStyle = new ComboBox();
            this.gbStateProperties = new GroupBox();
            this.pnStateButtons = new Panel();
            this.btStateDelete = new Button();
            this.btStateEdit = new Button();
            this.btStateAdd = new Button();
            this.tbStateName = new TextBox();
            this.laStateName = new Label();
            this.laStateDesc = new Label();
            this.tbStateDesc = new TextBox();
            this.chbCaseSensitive = new CheckBox();
            this.colorDialog1 = new ColorDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.cmLexer = new ContextMenu();
            this.openFileDialog = new OpenFileDialog();
            this.cmReswords = new ContextMenu();
            this.miAddResword = new MenuItem();
            this.miDeleteResword = new MenuItem();
            this.cmExpressions = new ContextMenu();
            this.miAddBlockExpr = new MenuItem();
            this.miDeleteBlockExpr = new MenuItem();
            this.pnButtons.SuspendLayout();
            this.pnMain.SuspendLayout();
            this.tcPanels.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.pnGeneral.SuspendLayout();
            this.tpStyles.SuspendLayout();
            this.pnStyles.SuspendLayout();
            this.pnStyleButtons.SuspendLayout();
            this.pnSample.SuspendLayout();
            this.gbStyleProperties.SuspendLayout();
            this.gbFontStyles.SuspendLayout();
            this.tpStates.SuspendLayout();
            this.pnStates.SuspendLayout();
            this.gbReswordsProperties.SuspendLayout();
            this.pnReswordButtons.SuspendLayout();
            this.gbSyntaxBlockProperties.SuspendLayout();
            this.pnBlockButtons.SuspendLayout();
            this.gbStateProperties.SuspendLayout();
            this.pnStateButtons.SuspendLayout();
            base.SuspendLayout();
            this.pnButtons.BackColor = SystemColors.Control;
            this.pnButtons.BorderStyle = BorderStyle.Fixed3D;
            this.pnButtons.Controls.Add(this.btCancel);
            this.pnButtons.Controls.Add(this.btOk);
            this.pnButtons.Controls.Add(this.btLoadScheme);
            this.pnButtons.Controls.Add(this.btClear);
            this.pnButtons.Controls.Add(this.btSave);
            this.pnButtons.Dock = DockStyle.Bottom;
            this.pnButtons.Location = new Point(0, 0x1e8);
            this.pnButtons.Name = "pnButtons";
            this.pnButtons.Size = new Size(0x242, 0x30);
            this.pnButtons.TabIndex = 12;
            this.btCancel.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btCancel.DialogResult = DialogResult.Cancel;
            this.btCancel.Location = new Point(0x1f0, 12);
            this.btCancel.Name = "btCancel";
            this.btCancel.TabIndex = 0x21;
            this.btCancel.Text = "Cancel";
            this.btCancel.Click += new EventHandler(this.btCancel_Click);
            this.btOk.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btOk.DialogResult = DialogResult.OK;
            this.btOk.Location = new Point(0x1a0, 12);
            this.btOk.Name = "btOk";
            this.btOk.TabIndex = 0x20;
            this.btOk.Text = "&OK";
            this.btOk.Click += new EventHandler(this.btOk_Click);
            this.btLoadScheme.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btLoadScheme.Location = new Point(160, 12);
            this.btLoadScheme.Name = "btLoadScheme";
            this.btLoadScheme.TabIndex = 0x1c;
            this.btLoadScheme.Text = "Load";
            this.btLoadScheme.Click += new EventHandler(this.btLoadScheme_Click);
            this.btClear.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btClear.Location = new Point(320, 12);
            this.btClear.Name = "btClear";
            this.btClear.TabIndex = 0x1f;
            this.btClear.Text = "Clear";
            this.btClear.Click += new EventHandler(this.btClear_Click);
            this.btSave.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.btSave.Location = new Point(240, 12);
            this.btSave.Name = "btSave";
            this.btSave.TabIndex = 0x1d;
            this.btSave.Text = "Save";
            this.btSave.Click += new EventHandler(this.btSave_Click);
            this.tvLexer.BackColor = SystemColors.Control;
            this.tvLexer.Dock = DockStyle.Left;
            this.tvLexer.HideSelection = false;
            this.tvLexer.ImageIndex = 3;
            this.tvLexer.ImageList = this.imLexer;
            this.tvLexer.LabelEdit = true;
            this.tvLexer.Location = new Point(0, 0);
            this.tvLexer.Name = "tvLexer";
            TreeNode[] nodeArray1 = new TreeNode[1];
            TreeNode[] nodeArray2 = new TreeNode[3] { new TreeNode("General", 3, 2), new TreeNode("Styles", 1, 0), new TreeNode("States", 1, 0) } ;
            nodeArray1[0] = new TreeNode("Syntax Scheme Editor", 0, 0, nodeArray2);
            this.tvLexer.Nodes.AddRange(nodeArray1);
            this.tvLexer.SelectedImageIndex = 2;
            this.tvLexer.ShowLines = false;
            this.tvLexer.ShowRootLines = false;
            this.tvLexer.Size = new Size(0xb8, 0x1e8);
            this.tvLexer.TabIndex = 0x22;
            this.tvLexer.MouseDown += new MouseEventHandler(this.tvLexer_MouseDown);
            this.tvLexer.AfterExpand += new TreeViewEventHandler(this.tvLexer_AfterExpand);
            this.tvLexer.AfterCollapse += new TreeViewEventHandler(this.tvLexer_AfterCollapse);
            this.tvLexer.AfterSelect += new TreeViewEventHandler(this.tvLexer_AfterSelect);
            this.tvLexer.AfterLabelEdit += new NodeLabelEditEventHandler(this.tvLexer_AfterLabelEdit);
            this.tvLexer.BeforeLabelEdit += new NodeLabelEditEventHandler(this.tvLexer_BeforeLabelEdit);
            this.imLexer.ImageSize = new Size(0x10, 0x10);
            this.imLexer.ImageStream = (ImageListStreamer) manager1.GetObject("imLexer.ImageStream");
            this.imLexer.TransparentColor = Color.Red;
            this.pnMain.BackColor = SystemColors.Control;
            this.pnMain.Controls.Add(this.tcPanels);
            this.pnMain.Dock = DockStyle.Fill;
            this.pnMain.Location = new Point(0xb8, 0);
            this.pnMain.Name = "pnMain";
            this.pnMain.Size = new Size(0x18a, 0x1e8);
            this.pnMain.TabIndex = 0x23;
            this.tcPanels.Controls.Add(this.tpGeneral);
            this.tcPanels.Controls.Add(this.tpStyles);
            this.tcPanels.Controls.Add(this.tpStates);
            this.tcPanels.Location = new Point(0, 0);
            this.tcPanels.Name = "tcPanels";
            this.tcPanels.SelectedIndex = 0;
            this.tcPanels.Size = new Size(0x18a, 0x1e8);
            this.tcPanels.TabIndex = 0;
            this.tcPanels.Visible = false;
            this.tpGeneral.Controls.Add(this.pnGeneral);
            this.tpGeneral.Location = new Point(4, 0x16);
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Size = new Size(0x182, 0x1ce);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.pnGeneral.Controls.Add(this.laCopyright);
            this.pnGeneral.Controls.Add(this.laDescription);
            this.pnGeneral.Controls.Add(this.laSchemeName);
            this.pnGeneral.Controls.Add(this.laAuthor);
            this.pnGeneral.Controls.Add(this.tbCopyright);
            this.pnGeneral.Controls.Add(this.tbDescription);
            this.pnGeneral.Controls.Add(this.tbSchemeName);
            this.pnGeneral.Controls.Add(this.tbAuthor);
            this.pnGeneral.Location = new Point(0, 0);
            this.pnGeneral.Name = "pnGeneral";
            this.pnGeneral.Size = new Size(0x180, 0x1c0);
            this.pnGeneral.TabIndex = 1;
            this.pnGeneral.Visible = false;
            this.laCopyright.AutoSize = true;
            this.laCopyright.Location = new Point(0x10, 0x98);
            this.laCopyright.Name = "laCopyright";
            this.laCopyright.Size = new Size(0x38, 0x10);
            this.laCopyright.TabIndex = 6;
            this.laCopyright.Text = "Copyright:";
            this.laDescription.AutoSize = true;
            this.laDescription.Location = new Point(0x10, 0x68);
            this.laDescription.Name = "laDescription";
            this.laDescription.Size = new Size(0x40, 0x10);
            this.laDescription.TabIndex = 4;
            this.laDescription.Text = "Description:";
            this.laSchemeName.AutoSize = true;
            this.laSchemeName.Location = new Point(0x10, 0x38);
            this.laSchemeName.Name = "laSchemeName";
            this.laSchemeName.Size = new Size(0x52, 0x10);
            this.laSchemeName.TabIndex = 2;
            this.laSchemeName.Text = "Scheme Name:";
            this.laAuthor.AutoSize = true;
            this.laAuthor.Location = new Point(0x10, 8);
            this.laAuthor.Name = "laAuthor";
            this.laAuthor.Size = new Size(0x29, 0x10);
            this.laAuthor.TabIndex = 0;
            this.laAuthor.Text = "Author:";
            this.tbCopyright.Anchor = AnchorStyles.Right | (AnchorStyles.Left | AnchorStyles.Top);
            this.tbCopyright.Location = new Point(0x10, 0xb0);
            this.tbCopyright.Name = "tbCopyright";
            this.tbCopyright.Size = new Size(360, 20);
            this.tbCopyright.TabIndex = 7;
            this.tbCopyright.Text = "";
            this.tbDescription.Anchor = AnchorStyles.Right | (AnchorStyles.Left | AnchorStyles.Top);
            this.tbDescription.Location = new Point(0x10, 0x80);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new Size(360, 20);
            this.tbDescription.TabIndex = 5;
            this.tbDescription.Text = "";
            this.tbSchemeName.Anchor = AnchorStyles.Right | (AnchorStyles.Left | AnchorStyles.Top);
            this.tbSchemeName.Location = new Point(0x10, 80);
            this.tbSchemeName.Name = "tbSchemeName";
            this.tbSchemeName.Size = new Size(360, 20);
            this.tbSchemeName.TabIndex = 3;
            this.tbSchemeName.Text = "";
            this.tbAuthor.Anchor = AnchorStyles.Right | (AnchorStyles.Left | AnchorStyles.Top);
            this.tbAuthor.Location = new Point(0x10, 0x20);
            this.tbAuthor.Name = "tbAuthor";
            this.tbAuthor.Size = new Size(360, 20);
            this.tbAuthor.TabIndex = 1;
            this.tbAuthor.Text = "";
            this.tpStyles.Controls.Add(this.pnStyles);
            this.tpStyles.Location = new Point(4, 0x16);
            this.tpStyles.Name = "tpStyles";
            this.tpStyles.Size = new Size(0x182, 0x1ce);
            this.tpStyles.TabIndex = 1;
            this.tpStyles.Text = "Styles";
            this.pnStyles.BackColor = SystemColors.Control;
            this.pnStyles.Controls.Add(this.pnStyleButtons);
            this.pnStyles.Controls.Add(this.pnSample);
            this.pnStyles.Controls.Add(this.gbStyleProperties);
            this.pnStyles.Location = new Point(0, 0);
            this.pnStyles.Name = "pnStyles";
            this.pnStyles.Size = new Size(0x1c8, 480);
            this.pnStyles.TabIndex = 4;
            this.pnStyles.Visible = false;
            this.pnStyleButtons.Controls.Add(this.btStyleDelete);
            this.pnStyleButtons.Controls.Add(this.btStyleEdit);
            this.pnStyleButtons.Controls.Add(this.btStyleAdd);
            this.pnStyleButtons.Location = new Point(8, 0x130);
            this.pnStyleButtons.Name = "pnStyleButtons";
            this.pnStyleButtons.Size = new Size(0x34, 0x12);
            this.pnStyleButtons.TabIndex = 7;
            this.btStyleDelete.ImageIndex = 2;
            this.btStyleDelete.ImageList = this.ilButtons;
            this.btStyleDelete.Location = new Point(0x11, 0);
            this.btStyleDelete.Name = "btStyleDelete";
            this.btStyleDelete.Size = new Size(0x11, 0x11);
            this.btStyleDelete.TabIndex = 1;
            this.btStyleDelete.Click += new EventHandler(this.DeleteStyleClick);
            this.ilButtons.ImageSize = new Size(0x10, 0x10);
            this.ilButtons.ImageStream = (ImageListStreamer) manager1.GetObject("ilButtons.ImageStream");
            this.ilButtons.TransparentColor = SystemColors.Control;
            this.btStyleEdit.ImageIndex = 1;
            this.btStyleEdit.ImageList = this.ilButtons;
            this.btStyleEdit.Location = new Point(0x22, 0);
            this.btStyleEdit.Name = "btStyleEdit";
            this.btStyleEdit.Size = new Size(0x11, 0x11);
            this.btStyleEdit.TabIndex = 2;
            this.btStyleEdit.Click += new EventHandler(this.btStyleEdit_Click);
            this.btStyleAdd.ImageIndex = 0;
            this.btStyleAdd.ImageList = this.ilButtons;
            this.btStyleAdd.Location = new Point(0, 0);
            this.btStyleAdd.Name = "btStyleAdd";
            this.btStyleAdd.Size = new Size(0x11, 0x11);
            this.btStyleAdd.TabIndex = 0;
            this.btStyleAdd.Click += new EventHandler(this.AddStyleClick);
            this.pnSample.BorderStyle = BorderStyle.Fixed3D;
            this.pnSample.Controls.Add(this.laSample);
            this.pnSample.Location = new Point(8, 0xe8);
            this.pnSample.Name = "pnSample";
            this.pnSample.Size = new Size(0x17a, 0x40);
            this.pnSample.TabIndex = 6;
            this.laSample.AutoSize = true;
            this.laSample.BorderStyle = BorderStyle.Fixed3D;
            this.laSample.Font = new Font("Microsoft Sans Serif", 18f, FontStyle.Regular, GraphicsUnit.Point, 0xcc);
            this.laSample.Location = new Point(120, 0x10);
            this.laSample.Name = "laSample";
            this.laSample.Size = new Size(0x7f, 0x22);
            this.laSample.TabIndex = 0;
            this.laSample.Text = "AaBbYyZz";
            this.gbStyleProperties.Controls.Add(this.chbPlainText);
            this.gbStyleProperties.Controls.Add(this.clbBkColor);
            this.gbStyleProperties.Controls.Add(this.clbForeColor);
            this.gbStyleProperties.Controls.Add(this.tbStyleName);
            this.gbStyleProperties.Controls.Add(this.laStyleName);
            this.gbStyleProperties.Controls.Add(this.laStyleBkColor);
            this.gbStyleProperties.Controls.Add(this.laStyleForeColor);
            this.gbStyleProperties.Controls.Add(this.tbStyleDesc);
            this.gbStyleProperties.Controls.Add(this.laStyleDesc);
            this.gbStyleProperties.Controls.Add(this.gbFontStyles);
            this.gbStyleProperties.Location = new Point(8, 8);
            this.gbStyleProperties.Name = "gbStyleProperties";
            this.gbStyleProperties.Size = new Size(380, 0xd8);
            this.gbStyleProperties.TabIndex = 5;
            this.gbStyleProperties.TabStop = false;
            this.gbStyleProperties.Text = "Style Properties";
            this.chbPlainText.Location = new Point(0x100, 160);
            this.chbPlainText.Name = "chbPlainText";
            this.chbPlainText.Size = new Size(80, 0x18);
            this.chbPlainText.TabIndex = 8;
            this.chbPlainText.Text = "Plain Text";
            this.clbBkColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.clbBkColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.clbBkColor.Location = new Point(0x10, 160);
            this.clbBkColor.Name = "clbBkColor";
            this.clbBkColor.SelectedColor = SystemColors.ActiveBorder;
            this.clbBkColor.Size = new Size(0xd8, 0x15);
            this.clbBkColor.TabIndex = 7;
            this.clbForeColor.DrawMode = DrawMode.OwnerDrawFixed;
            this.clbForeColor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.clbForeColor.Location = new Point(0x10, 0x72);
            this.clbForeColor.Name = "clbForeColor";
            this.clbForeColor.SelectedColor = SystemColors.ActiveBorder;
            this.clbForeColor.Size = new Size(0xd8, 0x15);
            this.clbForeColor.TabIndex = 5;
            this.tbStyleName.Location = new Point(0x10, 0x20);
            this.tbStyleName.Name = "tbStyleName";
            this.tbStyleName.Size = new Size(0xd8, 20);
            this.tbStyleName.TabIndex = 1;
            this.tbStyleName.Text = "";
            this.laStyleName.AutoSize = true;
            this.laStyleName.Location = new Point(0x10, 0x10);
            this.laStyleName.Name = "laStyleName";
            this.laStyleName.Size = new Size(0x26, 0x10);
            this.laStyleName.TabIndex = 0;
            this.laStyleName.Text = "Name:";
            this.laStyleBkColor.AutoSize = true;
            this.laStyleBkColor.Location = new Point(0x10, 0x90);
            this.laStyleBkColor.Name = "laStyleBkColor";
            this.laStyleBkColor.Size = new Size(0x3f, 0x10);
            this.laStyleBkColor.TabIndex = 6;
            this.laStyleBkColor.Text = "Back Color:";
            this.laStyleForeColor.AutoSize = true;
            this.laStyleForeColor.Location = new Point(0x10, 0x63);
            this.laStyleForeColor.Name = "laStyleForeColor";
            this.laStyleForeColor.Size = new Size(0x3d, 0x10);
            this.laStyleForeColor.TabIndex = 4;
            this.laStyleForeColor.Text = "Fore Color:";
            this.tbStyleDesc.Location = new Point(0x10, 0x48);
            this.tbStyleDesc.Name = "tbStyleDesc";
            this.tbStyleDesc.Size = new Size(0xd8, 20);
            this.tbStyleDesc.TabIndex = 3;
            this.tbStyleDesc.Text = "";
            this.laStyleDesc.AutoSize = true;
            this.laStyleDesc.Location = new Point(0x10, 0x38);
            this.laStyleDesc.Name = "laStyleDesc";
            this.laStyleDesc.Size = new Size(0x40, 0x10);
            this.laStyleDesc.TabIndex = 2;
            this.laStyleDesc.Text = "Description:";
            this.gbFontStyles.Controls.Add(this.chbStrikeout);
            this.gbFontStyles.Controls.Add(this.chbItalic);
            this.gbFontStyles.Controls.Add(this.chbUnderline);
            this.gbFontStyles.Controls.Add(this.chbBold);
            this.gbFontStyles.Location = new Point(0x100, 0x20);
            this.gbFontStyles.Name = "gbFontStyles";
            this.gbFontStyles.Size = new Size(0x70, 120);
            this.gbFontStyles.TabIndex = 9;
            this.gbFontStyles.TabStop = false;
            this.gbFontStyles.Text = "Font Styles";
            this.chbStrikeout.Location = new Point(0x10, 0x58);
            this.chbStrikeout.Name = "chbStrikeout";
            this.chbStrikeout.Size = new Size(80, 0x18);
            this.chbStrikeout.TabIndex = 3;
            this.chbStrikeout.Text = "StrikeOut";
            this.chbItalic.Location = new Point(0x10, 40);
            this.chbItalic.Name = "chbItalic";
            this.chbItalic.Size = new Size(80, 0x18);
            this.chbItalic.TabIndex = 1;
            this.chbItalic.Text = "Italic";
            this.chbUnderline.Location = new Point(0x10, 0x40);
            this.chbUnderline.Name = "chbUnderline";
            this.chbUnderline.Size = new Size(80, 0x18);
            this.chbUnderline.TabIndex = 2;
            this.chbUnderline.Text = "Underline";
            this.chbBold.Location = new Point(0x10, 0x10);
            this.chbBold.Name = "chbBold";
            this.chbBold.Size = new Size(80, 0x18);
            this.chbBold.TabIndex = 0;
            this.chbBold.Text = "Bold";
            this.tpStates.Controls.Add(this.pnStates);
            this.tpStates.Location = new Point(4, 0x16);
            this.tpStates.Name = "tpStates";
            this.tpStates.Size = new Size(0x182, 0x1ce);
            this.tpStates.TabIndex = 2;
            this.tpStates.Text = "States";
            this.pnStates.BackColor = SystemColors.Control;
            this.pnStates.Controls.Add(this.gbReswordsProperties);
            this.pnStates.Controls.Add(this.gbSyntaxBlockProperties);
            this.pnStates.Controls.Add(this.gbStateProperties);
            this.pnStates.Location = new Point(0, 0);
            this.pnStates.Name = "pnStates";
            this.pnStates.Size = new Size(390, 490);
            this.pnStates.TabIndex = 3;
            this.pnStates.Visible = false;
            this.gbReswordsProperties.Controls.Add(this.tbResWordSetName);
            this.gbReswordsProperties.Controls.Add(this.laResWordSetName);
            this.gbReswordsProperties.Controls.Add(this.pnReswordButtons);
            this.gbReswordsProperties.Controls.Add(this.lbReswords);
            this.gbReswordsProperties.Controls.Add(this.cbResWordStyle);
            this.gbReswordsProperties.Controls.Add(this.tbResword);
            this.gbReswordsProperties.Controls.Add(this.laBlockReswords);
            this.gbReswordsProperties.Controls.Add(this.laResWordStyle);
            this.gbReswordsProperties.Location = new Point(8, 320);
            this.gbReswordsProperties.Name = "gbReswordsProperties";
            this.gbReswordsProperties.Size = new Size(380, 0xa4);
            this.gbReswordsProperties.TabIndex = 2;
            this.gbReswordsProperties.TabStop = false;
            this.gbReswordsProperties.Text = "Resword Set Properties";
            this.tbResWordSetName.Location = new Point(0x70, 0x10);
            this.tbResWordSetName.Name = "tbResWordSetName";
            this.tbResWordSetName.Size = new Size(0x100, 20);
            this.tbResWordSetName.TabIndex = 1;
            this.tbResWordSetName.Text = "";
            this.tbResWordSetName.KeyDown += new KeyEventHandler(this.tbResWordSetName_KeyDown);
            this.laResWordSetName.AutoSize = true;
            this.laResWordSetName.Location = new Point(8, 0x13);
            this.laResWordSetName.Name = "laResWordSetName";
            this.laResWordSetName.Size = new Size(0x26, 0x10);
            this.laResWordSetName.TabIndex = 0;
            this.laResWordSetName.Text = "Name:";
            this.pnReswordButtons.Controls.Add(this.btResWordSetDelete);
            this.pnReswordButtons.Controls.Add(this.btResWordSetEdit);
            this.pnReswordButtons.Controls.Add(this.btResWordSetAdd);
            this.pnReswordButtons.Location = new Point(8, 0x8a);
            this.pnReswordButtons.Name = "pnReswordButtons";
            this.pnReswordButtons.Size = new Size(0x34, 0x12);
            this.pnReswordButtons.TabIndex = 7;
            this.btResWordSetDelete.ImageIndex = 2;
            this.btResWordSetDelete.ImageList = this.ilButtons;
            this.btResWordSetDelete.Location = new Point(0x11, 0);
            this.btResWordSetDelete.Name = "btResWordSetDelete";
            this.btResWordSetDelete.Size = new Size(0x11, 0x11);
            this.btResWordSetDelete.TabIndex = 1;
            this.btResWordSetDelete.Click += new EventHandler(this.DeleteResWordSetClick);
            this.btResWordSetEdit.ImageIndex = 1;
            this.btResWordSetEdit.ImageList = this.ilButtons;
            this.btResWordSetEdit.Location = new Point(0x22, 0);
            this.btResWordSetEdit.Name = "btResWordSetEdit";
            this.btResWordSetEdit.Size = new Size(0x11, 0x11);
            this.btResWordSetEdit.TabIndex = 2;
            this.btResWordSetEdit.Click += new EventHandler(this.btResWordSetEdit_Click);
            this.btResWordSetAdd.ImageIndex = 0;
            this.btResWordSetAdd.ImageList = this.ilButtons;
            this.btResWordSetAdd.Location = new Point(0, 0);
            this.btResWordSetAdd.Name = "btResWordSetAdd";
            this.btResWordSetAdd.Size = new Size(0x11, 0x11);
            this.btResWordSetAdd.TabIndex = 0;
            this.btResWordSetAdd.Click += new EventHandler(this.AddResWordSetClick);
            this.lbReswords.Location = new Point(0x70, 0x58);
            this.lbReswords.Name = "lbReswords";
            this.lbReswords.Size = new Size(0x100, 0x45);
            this.lbReswords.Sorted = true;
            this.lbReswords.TabIndex = 6;
            this.cbResWordStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbResWordStyle.Location = new Point(0x70, 40);
            this.cbResWordStyle.Name = "cbResWordStyle";
            this.cbResWordStyle.Size = new Size(0x100, 0x15);
            this.cbResWordStyle.TabIndex = 3;
            this.tbResword.Location = new Point(0x70, 0x40);
            this.tbResword.Name = "tbResword";
            this.tbResword.Size = new Size(0x100, 20);
            this.tbResword.TabIndex = 5;
            this.tbResword.Text = "";
            this.tbResword.Enter += new EventHandler(this.tbResword_Enter);
            this.laBlockReswords.AutoSize = true;
            this.laBlockReswords.Location = new Point(8, 0x43);
            this.laBlockReswords.Name = "laBlockReswords";
            this.laBlockReswords.Size = new Size(0x3a, 0x10);
            this.laBlockReswords.TabIndex = 4;
            this.laBlockReswords.Text = "Reswords:";
            this.laResWordStyle.AutoSize = true;
            this.laResWordStyle.Location = new Point(8, 0x2b);
            this.laResWordStyle.Name = "laResWordStyle";
            this.laResWordStyle.Size = new Size(0x53, 0x10);
            this.laResWordStyle.TabIndex = 2;
            this.laResWordStyle.Text = "ResWord Style:";
            this.gbSyntaxBlockProperties.Controls.Add(this.tbBlockName);
            this.gbSyntaxBlockProperties.Controls.Add(this.laBlockName);
            this.gbSyntaxBlockProperties.Controls.Add(this.pnBlockButtons);
            this.gbSyntaxBlockProperties.Controls.Add(this.cbBlockExpr);
            this.gbSyntaxBlockProperties.Controls.Add(this.lbBlockExpr);
            this.gbSyntaxBlockProperties.Controls.Add(this.laBlockExpr);
            this.gbSyntaxBlockProperties.Controls.Add(this.tbBlockDesc);
            this.gbSyntaxBlockProperties.Controls.Add(this.laBlockDesc);
            this.gbSyntaxBlockProperties.Controls.Add(this.laLeaveState);
            this.gbSyntaxBlockProperties.Controls.Add(this.laStyle);
            this.gbSyntaxBlockProperties.Controls.Add(this.cbLeaveState);
            this.gbSyntaxBlockProperties.Controls.Add(this.cbStyle);
            this.gbSyntaxBlockProperties.Location = new Point(8, 0x66);
            this.gbSyntaxBlockProperties.Name = "gbSyntaxBlockProperties";
            this.gbSyntaxBlockProperties.Size = new Size(380, 0xd4);
            this.gbSyntaxBlockProperties.TabIndex = 1;
            this.gbSyntaxBlockProperties.TabStop = false;
            this.gbSyntaxBlockProperties.Text = "Block Properties";
            this.tbBlockName.Location = new Point(0x70, 0x10);
            this.tbBlockName.Name = "tbBlockName";
            this.tbBlockName.Size = new Size(0x100, 20);
            this.tbBlockName.TabIndex = 1;
            this.tbBlockName.Text = "";
            this.tbBlockName.KeyDown += new KeyEventHandler(this.tbBlockName_KeyDown);
            this.laBlockName.AutoSize = true;
            this.laBlockName.Location = new Point(8, 0x13);
            this.laBlockName.Name = "laBlockName";
            this.laBlockName.Size = new Size(0x26, 0x10);
            this.laBlockName.TabIndex = 0;
            this.laBlockName.Text = "Name:";
            this.pnBlockButtons.Controls.Add(this.btBlockDelete);
            this.pnBlockButtons.Controls.Add(this.btBlockEdit);
            this.pnBlockButtons.Controls.Add(this.btBlockAdd);
            this.pnBlockButtons.Location = new Point(8, 0xba);
            this.pnBlockButtons.Name = "pnBlockButtons";
            this.pnBlockButtons.Size = new Size(0x34, 0x12);
            this.pnBlockButtons.TabIndex = 11;
            this.btBlockDelete.ImageIndex = 2;
            this.btBlockDelete.ImageList = this.ilButtons;
            this.btBlockDelete.Location = new Point(0x11, 0);
            this.btBlockDelete.Name = "btBlockDelete";
            this.btBlockDelete.Size = new Size(0x11, 0x11);
            this.btBlockDelete.TabIndex = 1;
            this.btBlockDelete.Click += new EventHandler(this.DeleteBlockClick);
            this.btBlockEdit.ImageIndex = 1;
            this.btBlockEdit.ImageList = this.ilButtons;
            this.btBlockEdit.Location = new Point(0x22, 0);
            this.btBlockEdit.Name = "btBlockEdit";
            this.btBlockEdit.Size = new Size(0x11, 0x11);
            this.btBlockEdit.TabIndex = 2;
            this.btBlockEdit.Click += new EventHandler(this.btBlockEdit_Click);
            this.btBlockAdd.ImageIndex = 0;
            this.btBlockAdd.ImageList = this.ilButtons;
            this.btBlockAdd.Location = new Point(0, 0);
            this.btBlockAdd.Name = "btBlockAdd";
            this.btBlockAdd.Size = new Size(0x11, 0x11);
            this.btBlockAdd.TabIndex = 0;
            this.btBlockAdd.Click += new EventHandler(this.AddBlockClick);
            object[] objArray1 = new object[5] { "Identifiers   [a-zA-Z_][a-zA-Z0-9_]*", "Comments   //.", @"Numbers     ([0-9]+\.[0-9]*(e|E)(\+|\-)?[0-9]+)|([0-9]+\.[0-9]*)|([0-9]+)", "Strings        '[^']*'", @"Whitespace (\s)*" } ;
            this.cbBlockExpr.Items.AddRange(objArray1);
            this.cbBlockExpr.Location = new Point(0x70, 0x70);
            this.cbBlockExpr.Name = "cbBlockExpr";
            this.cbBlockExpr.Size = new Size(0x100, 0x15);
            this.cbBlockExpr.TabIndex = 9;
            this.lbBlockExpr.Location = new Point(0x70, 0x88);
            this.lbBlockExpr.Name = "lbBlockExpr";
            this.lbBlockExpr.Size = new Size(0x100, 0x45);
            this.lbBlockExpr.TabIndex = 10;
            this.laBlockExpr.AutoSize = true;
            this.laBlockExpr.Location = new Point(8, 0x73);
            this.laBlockExpr.Name = "laBlockExpr";
            this.laBlockExpr.Size = new Size(0x45, 0x10);
            this.laBlockExpr.TabIndex = 8;
            this.laBlockExpr.Text = "Expressions:";
            this.tbBlockDesc.Location = new Point(0x70, 40);
            this.tbBlockDesc.Name = "tbBlockDesc";
            this.tbBlockDesc.Size = new Size(0x100, 20);
            this.tbBlockDesc.TabIndex = 3;
            this.tbBlockDesc.Text = "";
            this.laBlockDesc.AutoSize = true;
            this.laBlockDesc.Location = new Point(8, 0x2b);
            this.laBlockDesc.Name = "laBlockDesc";
            this.laBlockDesc.Size = new Size(0x40, 0x10);
            this.laBlockDesc.TabIndex = 2;
            this.laBlockDesc.Text = "Description:";
            this.laLeaveState.AutoSize = true;
            this.laLeaveState.Location = new Point(8, 0x43);
            this.laLeaveState.Name = "laLeaveState";
            this.laLeaveState.Size = new Size(0x44, 0x10);
            this.laLeaveState.TabIndex = 4;
            this.laLeaveState.Text = "Leave State:";
            this.laStyle.AutoSize = true;
            this.laStyle.Location = new Point(8, 0x5b);
            this.laStyle.Name = "laStyle";
            this.laStyle.Size = new Size(0x21, 0x10);
            this.laStyle.TabIndex = 6;
            this.laStyle.Text = "Style:";
            this.cbLeaveState.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbLeaveState.Location = new Point(0x70, 0x40);
            this.cbLeaveState.Name = "cbLeaveState";
            this.cbLeaveState.Size = new Size(0x100, 0x15);
            this.cbLeaveState.TabIndex = 5;
            this.cbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cbStyle.Location = new Point(0x70, 0x58);
            this.cbStyle.Name = "cbStyle";
            this.cbStyle.Size = new Size(0x100, 0x15);
            this.cbStyle.TabIndex = 7;
            this.gbStateProperties.Controls.Add(this.pnStateButtons);
            this.gbStateProperties.Controls.Add(this.tbStateName);
            this.gbStateProperties.Controls.Add(this.laStateName);
            this.gbStateProperties.Controls.Add(this.laStateDesc);
            this.gbStateProperties.Controls.Add(this.tbStateDesc);
            this.gbStateProperties.Controls.Add(this.chbCaseSensitive);
            this.gbStateProperties.Location = new Point(8, 8);
            this.gbStateProperties.Name = "gbStateProperties";
            this.gbStateProperties.Size = new Size(380, 0x5c);
            this.gbStateProperties.TabIndex = 0;
            this.gbStateProperties.TabStop = false;
            this.gbStateProperties.Text = "State Properties";
            this.pnStateButtons.Controls.Add(this.btStateDelete);
            this.pnStateButtons.Controls.Add(this.btStateEdit);
            this.pnStateButtons.Controls.Add(this.btStateAdd);
            this.pnStateButtons.Location = new Point(8, 0x44);
            this.pnStateButtons.Name = "pnStateButtons";
            this.pnStateButtons.Size = new Size(0x34, 0x12);
            this.pnStateButtons.TabIndex = 8;
            this.btStateDelete.ImageIndex = 2;
            this.btStateDelete.ImageList = this.ilButtons;
            this.btStateDelete.Location = new Point(0x11, 0);
            this.btStateDelete.Name = "btStateDelete";
            this.btStateDelete.Size = new Size(0x11, 0x11);
            this.btStateDelete.TabIndex = 1;
            this.btStateDelete.Click += new EventHandler(this.DeleteStateClick);
            this.btStateEdit.ImageIndex = 1;
            this.btStateEdit.ImageList = this.ilButtons;
            this.btStateEdit.Location = new Point(0x22, 0);
            this.btStateEdit.Name = "btStateEdit";
            this.btStateEdit.Size = new Size(0x11, 0x11);
            this.btStateEdit.TabIndex = 2;
            this.btStateEdit.Click += new EventHandler(this.btStateEdit_Click);
            this.btStateAdd.ImageIndex = 0;
            this.btStateAdd.ImageList = this.ilButtons;
            this.btStateAdd.Location = new Point(0, 0);
            this.btStateAdd.Name = "btStateAdd";
            this.btStateAdd.Size = new Size(0x11, 0x11);
            this.btStateAdd.TabIndex = 0;
            this.btStateAdd.Click += new EventHandler(this.AddStateClick);
            this.tbStateName.Location = new Point(0x70, 0x10);
            this.tbStateName.Name = "tbStateName";
            this.tbStateName.Size = new Size(0x100, 20);
            this.tbStateName.TabIndex = 1;
            this.tbStateName.Text = "";
            this.laStateName.AutoSize = true;
            this.laStateName.Location = new Point(8, 0x13);
            this.laStateName.Name = "laStateName";
            this.laStateName.Size = new Size(0x26, 0x10);
            this.laStateName.TabIndex = 0;
            this.laStateName.Text = "Name:";
            this.laStateDesc.AutoSize = true;
            this.laStateDesc.Location = new Point(8, 0x2b);
            this.laStateDesc.Name = "laStateDesc";
            this.laStateDesc.Size = new Size(0x40, 0x10);
            this.laStateDesc.TabIndex = 2;
            this.laStateDesc.Text = "Description:";
            this.tbStateDesc.Location = new Point(0x70, 40);
            this.tbStateDesc.Name = "tbStateDesc";
            this.tbStateDesc.Size = new Size(0x100, 20);
            this.tbStateDesc.TabIndex = 3;
            this.tbStateDesc.Text = "";
            this.chbCaseSensitive.Location = new Point(0x108, 0x40);
            this.chbCaseSensitive.Name = "chbCaseSensitive";
            this.chbCaseSensitive.TabIndex = 4;
            this.chbCaseSensitive.Text = "CaseSensitive";
            MenuItem[] itemArray1 = new MenuItem[2] { this.miAddResword, this.miDeleteResword } ;
            this.cmReswords.MenuItems.AddRange(itemArray1);
            this.miAddResword.Index = 0;
            this.miAddResword.Text = "Add resword";
            this.miAddResword.Click += new EventHandler(this.miAddResword_Click);
            this.miDeleteResword.Index = 1;
            this.miDeleteResword.Text = "Delete resword";
            this.miDeleteResword.Click += new EventHandler(this.miDeleteResword_Click);
            itemArray1 = new MenuItem[2] { this.miAddBlockExpr, this.miDeleteBlockExpr } ;
            this.cmExpressions.MenuItems.AddRange(itemArray1);
            this.miAddBlockExpr.Index = 0;
            this.miAddBlockExpr.Text = "Add Expression";
            this.miAddBlockExpr.Click += new EventHandler(this.miAddBlockExpr_Click);
            this.miDeleteBlockExpr.Index = 1;
            this.miDeleteBlockExpr.Text = "Delete Expression";
            this.miDeleteBlockExpr.Click += new EventHandler(this.miDeleteBlockExpr_Click);
            base.AutoScaleMode = AutoScaleMode.None;
            this.AutoScaleDimensions = new Size(5, 13);
            base.ClientSize = new Size(0x242, 0x218);
            base.Controls.Add(this.pnMain);
            base.Controls.Add(this.tvLexer);
            base.Controls.Add(this.pnButtons);
            base.FormBorderStyle = FormBorderStyle.FixedSingle;
            base.Name = "DlgSyntaxBuilder";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Syntax Scheme Builder";
            base.Closing += new CancelEventHandler(this.DlgSyntaxBuilder_Closing);
            base.Load += new EventHandler(this.MainForm_Load);
            this.pnButtons.ResumeLayout(false);
            this.pnMain.ResumeLayout(false);
            this.tcPanels.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.pnGeneral.ResumeLayout(false);
            this.tpStyles.ResumeLayout(false);
            this.pnStyles.ResumeLayout(false);
            this.pnStyleButtons.ResumeLayout(false);
            this.pnSample.ResumeLayout(false);
            this.gbStyleProperties.ResumeLayout(false);
            this.gbFontStyles.ResumeLayout(false);
            this.tpStates.ResumeLayout(false);
            this.pnStates.ResumeLayout(false);
            this.gbReswordsProperties.ResumeLayout(false);
            this.pnReswordButtons.ResumeLayout(false);
            this.gbSyntaxBlockProperties.ResumeLayout(false);
            this.pnBlockButtons.ResumeLayout(false);
            this.gbStateProperties.ResumeLayout(false);
            this.pnStateButtons.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        private bool IsReswordExist(string AResWord, ILexSyntaxBlock ALexBlock, int Index)
        {
            if (ALexBlock == null)
            {
                return false;
            }
            return false;
        }

        private bool IsSchemeComplete(LexScheme Scheme, out string AErrorMsg)
        {
            AErrorMsg = string.Empty;
            bool flag1 = true;
            for (int num1 = 0; num1 < Scheme.States.Length; num1++)
            {
                if (!this.IsStateComplete(Scheme, Scheme.States[num1], out AErrorMsg))
                {
                    flag1 = false;
                    break;
                }
            }
            return flag1;
        }

        private bool IsStateComplete(LexScheme Scheme, ILexState AState, out string AErrorMsg)
        {
            AErrorMsg = string.Empty;
            bool flag1 = true;
            ILexSyntaxBlock[] blockArray1 = AState.LexSyntaxBlocks;
            for (int num1 = 0; num1 < blockArray1.Length; num1++)
            {
                LexSyntaxBlock block1 = (LexSyntaxBlock) blockArray1[num1];
                if (!this.IsSyntaxBlockComplete(Scheme, AState, block1, out AErrorMsg))
                {
                    flag1 = false;
                    break;
                }
            }
            return flag1;
        }

        private bool IsStateUsed()
        {
            bool flag1 = false;
            ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            if (state1 != null)
            {
                for (int num1 = 0; num1 < this.Scheme.States.Length; num1++)
                {
                    if (state1 != this.Scheme.States[num1])
                    {
                        for (int num2 = 0; num2 < this.Scheme.States[num1].SyntaxBlocks.Length; num2++)
                        {
                            if (state1.Equals(this.Scheme.States[num1].SyntaxBlocks[num2].LeaveState))
                            {
                                flag1 = true;
                                break;
                            }
                        }
                    }
                }
            }
            return flag1;
        }

        private bool IsStyleUsed()
        {
            bool flag1 = false;
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (style1 != null)
            {
                for (int num1 = 0; num1 < this.Scheme.States.Length; num1++)
                {
                    for (int num2 = 0; num2 < this.Scheme.States[num1].SyntaxBlocks.Length; num2++)
                    {
                        if (style1.Equals(this.Scheme.States[num1].SyntaxBlocks[num2].Style))
                        {
                            flag1 = true;
                            break;
                        }
                    }
                }
            }
            return flag1;
        }

        private bool IsSyntaxBlockComplete(LexScheme Scheme, ILexState AState, ILexSyntaxBlock ABlock, out string AErrorMsg)
        {
            AErrorMsg = string.Empty;
            bool flag1 = true;
            if (ABlock.LeaveState == null)
            {
                AErrorMsg = string.Format(SyntaxBuilderConsts.IncompleteLeaveState, AState.Name, ABlock.Name);
                return false;
            }
            if (ABlock.Style == null)
            {
                AErrorMsg = string.Format(SyntaxBuilderConsts.IncompleteLeaveStyle, AState.Name, ABlock.Name);
                return false;
            }
            ILexResWordSet[] setArray1 = ABlock.LexResWordSets;
            for (int num1 = 0; num1 < setArray1.Length; num1++)
            {
                LexResWordSet set1 = (LexResWordSet) setArray1[num1];
                if ((set1.ResWords.Length > 0) && (set1.ResWordStyle == null))
                {
                    AErrorMsg = string.Format(SyntaxBuilderConsts.IncompleteResWordStyle, AState.Name, ABlock.Name, set1.Name);
                    return false;
                }
            }
            return flag1;
        }

        private void lbBlockExpr_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbBlockExpr.SelectedIndex >= 0)
            {
                this.cbBlockExpr.Text = this.lbBlockExpr.SelectedItem.ToString();
            }
            else
            {
                this.cbBlockExpr.Text = string.Empty;
            }
        }

        private void lbReswords_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbReswords.SelectedIndex >= 0)
            {
                this.tbResword.Text = this.lbReswords.SelectedItem.ToString();
            }
            else
            {
                this.tbResword.Text = string.Empty;
            }
            this.UpdateReswords();
        }

        private void LoadScheme()
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.ClearScheme();
                this.lexer.LoadScheme(this.openFileDialog.FileName);
                this.Text = SyntaxBuilderConsts.SyntaxFormCaption + " " + this.openFileDialog.FileName;
                this.UpdateScheme();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            TreeNode node1 = new TreeNode(this.sRemoveScroll);
            this.tvLexer.Nodes.Add(node1);
            this.tvLexer.Nodes.Remove(node1);
            this.tbStyleName.KeyDown += new KeyEventHandler(this.tbStyleName_KeyDown);
            this.tbStateName.KeyDown += new KeyEventHandler(this.tbStateName_KeyDown);
            this.cbStyle.SelectedIndexChanged += new EventHandler(this.cbStyle_SelectedIndexChanged);
            this.cbResWordStyle.SelectedIndexChanged += new EventHandler(this.cbResWordStyle_SelectedIndexChanged);
            this.cbLeaveState.SelectedIndexChanged += new EventHandler(this.cbLeaveState_SelectedIndexChanged);
            this.chbBold.CheckedChanged += new EventHandler(this.chbBold_CheckedChanged);
            this.chbItalic.CheckedChanged += new EventHandler(this.chbItalic_CheckedChanged);
            this.chbUnderline.CheckedChanged += new EventHandler(this.chbUnderline_CheckedChanged);
            this.chbStrikeout.CheckedChanged += new EventHandler(this.chbStrikeout_CheckedChanged);
            this.chbPlainText.CheckedChanged += new EventHandler(this.chbPlainText_CheckedChanged);
            this.clbBkColor.SelectedIndexChanged += new EventHandler(this.clbBkColor_SelectedIndexChanged);
            this.clbForeColor.SelectedIndexChanged += new EventHandler(this.clbForeColor_SelectedIndexChanged);
            this.tbStyleDesc.TextChanged += new EventHandler(this.tbStyleDesc_TextChanged);
            this.tbAuthor.TextChanged += new EventHandler(this.tbAuthor_TextChanged);
            this.tbDescription.TextChanged += new EventHandler(this.tbDescription_TextChanged);
            this.tbCopyright.TextChanged += new EventHandler(this.tbCopyright_TextChanged);
            this.tbSchemeName.TextChanged += new EventHandler(this.tbSchemeName_TextChanged);
            this.tbStateDesc.TextChanged += new EventHandler(this.tbStateDesc_TextChanged);
            this.chbCaseSensitive.CheckedChanged += new EventHandler(this.chbCaseSensitive_TextChanged);
            this.tbBlockDesc.TextChanged += new EventHandler(this.tbBlockDesc_TextChanged);
            this.tbResword.KeyDown += new KeyEventHandler(this.tbResword_KeyDown);
            this.lbReswords.SelectedIndexChanged += new EventHandler(this.lbReswords_SelectedIndexChanged);
            this.lbReswords.ContextMenu = this.cmReswords;
            this.cbBlockExpr.KeyDown += new KeyEventHandler(this.cbBlockExpr_KeyDown);
            this.lbBlockExpr.SelectedIndexChanged += new EventHandler(this.lbBlockExpr_SelectedIndexChanged);
            this.lbBlockExpr.ContextMenu = this.cmExpressions;
            this.tvLexer.SelectedNode = this.rootNode;
            this.rootNode.Expand();
            this.saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            this.openFileDialog.Filter = "Scheme files (*.xml)|*.xml";
        }

        private void miAddBlockExpr_Click(object sender, EventArgs e)
        {
            string[] textArray1 = new string[this.lbBlockExpr.Items.Count];
            for (int num1 = 0; num1 < this.lbBlockExpr.Items.Count; num1++)
            {
                textArray1[num1] = this.lbBlockExpr.Items[num1].ToString();
            }
            int num2 = this.GetFirstNumber(textArray1, SyntaxBuilderConsts.NewExprText);
            string text1 = SyntaxBuilderConsts.NewExprText + num2.ToString();
            this.lbBlockExpr.Items.Add(text1);
            this.UpdateBlockExpressions();
        }

        private void miAddResword_Click(object sender, EventArgs e)
        {
            string[] textArray1 = new string[this.lbReswords.Items.Count];
            for (int num1 = 0; num1 < this.lbReswords.Items.Count; num1++)
            {
                textArray1[num1] = this.lbReswords.Items[num1].ToString();
            }
            int num2 = this.GetFirstNumber(textArray1, SyntaxBuilderConsts.NewReswordText);
            string text1 = SyntaxBuilderConsts.NewReswordText + num2.ToString();
            this.lbReswords.SelectedIndex = this.lbReswords.Items.Add(text1);
            this.UpdateReswords();
        }

        private void miDeleteBlockExpr_Click(object sender, EventArgs e)
        {
            if (this.lbBlockExpr.SelectedItem != null)
            {
                this.lbBlockExpr.Items.Remove(this.lbBlockExpr.SelectedItem);
                this.UpdateBlockExpressions();
            }
        }

        private void miDeleteResword_Click(object sender, EventArgs e)
        {
            if (this.lbReswords.SelectedItem != null)
            {
                this.lbReswords.Items.Remove(this.lbReswords.SelectedItem);
                this.UpdateReswords();
            }
        }

        private void SaveScheme()
        {
            string text1 = string.Empty;
            if (!this.IsSchemeComplete(this.Scheme, out text1))
            {
                string[] textArray1 = new string[5] { SyntaxBuilderConsts.InvalidScheme, ": ", text1, "\n", SyntaxBuilderConsts.QueryInvalidScheme } ;
                if (MessageBox.Show(string.Concat(textArray1), Consts.ErrorCaption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
                {
                    return;
                }
            }
            if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                this.lexer.SaveScheme(this.saveFileDialog.FileName);
                this.Text = SyntaxBuilderConsts.SyntaxFormCaption + " " + this.saveFileDialog.FileName;
            }
        }

        private void tbAuthor_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Author = this.tbAuthor.Text;
        }

        private void tbBlockDesc_TextChanged(object sender, EventArgs e)
        {
            ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            if (block1 != null)
            {
                block1.Desc = this.tbBlockDesc.Text;
            }
        }

        private void tbBlockName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                if (block1 != null)
                {
                    this.GetSyntaxBlockNode(this.tvLexer.SelectedNode).Text = this.tbBlockName.Text;
                    block1.Name = this.tbBlockName.Text;
                }
            }
        }

        private void tbCopyright_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Copyright = this.tbCopyright.Text;
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Desc = this.tbDescription.Text;
        }

        private void tbResword_Enter(object sender, EventArgs e)
        {
            if ((this.lbReswords.Items.Count > 0) && (this.lbReswords.SelectedIndex < 0))
            {
                this.lbReswords.SelectedIndex = 0;
            }
        }

        private void tbResword_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Return) && (this.tbResword.Text != string.Empty))
            {
                ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                if (block1 != null)
                {
                    if (this.lbReswords.SelectedIndex == -1)
                    {
                        if (!this.IsReswordExist(this.tbResword.Text, block1, -1))
                        {
                            this.lbReswords.Items.Add(this.tbResword.Text);
                            this.lbReswords.SelectedIndex = this.lbReswords.Items.Count - 1;
                        }
                    }
                    else if (!this.IsReswordExist(this.tbResword.Text, block1, this.lbReswords.SelectedIndex))
                    {
                        this.lbReswords.BeginUpdate();
                        this.lbReswords.Sorted = false;
                        this.lbReswords.Items[this.lbReswords.SelectedIndex] = this.tbResword.Text;
                        this.lbReswords.Sorted = true;
                        this.lbReswords.EndUpdate();
                    }
                }
                this.UpdateReswords();
            }
        }

        private void tbResWordSetName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                ILexResWordSet set1 = this.GetLexResWordSet(this.tvLexer.SelectedNode, this.Scheme);
                if (set1 != null)
                {
                    this.tvLexer.SelectedNode.Text = this.tbResWordSetName.Text;
                    set1.Name = this.tbResWordSetName.Text;
                }
            }
        }

        private void tbSchemeName_TextChanged(object sender, EventArgs e)
        {
            this.Scheme.Name = this.tbSchemeName.Text;
        }

        private void tbStateDesc_TextChanged(object sender, EventArgs e)
        {
            ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            if (state1 != null)
            {
                state1.Desc = this.tbStateDesc.Text;
            }
        }

        private void tbStateName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
                if (state1 != null)
                {
                    TreeNode node1 = this.GetStateNode(this.tvLexer.SelectedNode);
                    if (node1 != null)
                    {
                        node1.Text = this.tbStateName.Text;
                    }
                    state1.Name = this.tbStateName.Text;
                    this.UpdateStates((node1 != null) ? node1.Index : -1);
                }
            }
        }

        private void tbStyleDesc_TextChanged(object sender, EventArgs e)
        {
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (style1 != null)
            {
                style1.Desc = this.tbStyleDesc.Text;
            }
        }

        private void tbStyleName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
                if (style1 != null)
                {
                    TreeNode node1 = this.GetStyleNode(this.tvLexer.SelectedNode);
                    if (node1 != null)
                    {
                        node1.Text = this.tbStyleName.Text;
                    }
                    style1.Name = this.tbStyleName.Text;
                    this.UpdateLexStyles();
                }
            }
        }

        private void tvLexer_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Equals(this.stylesNode) || e.Node.Equals(this.statesNode))
            {
                e.Node.SelectedImageIndex = 1;
            }
        }

        private void tvLexer_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Equals(this.stylesNode) || e.Node.Equals(this.statesNode))
            {
                e.Node.SelectedImageIndex = 0;
            }
        }

        private void tvLexer_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                switch (this.GetNodeKind(e.Node))
                {
                    case NodeKind.nkStyle:
                    {
                        this.Scheme.Styles[e.Node.Index].Name = e.Label;
                        this.UpdateLexStyles();
                        return;
                    }
                    case NodeKind.nkStates:
                    {
                        return;
                    }
                    case NodeKind.nkState:
                    {
                        this.Scheme.States[e.Node.Index].Name = e.Label;
                        this.UpdateStates(e.Node.Index);
                        return;
                    }
                    case NodeKind.nkSyntaxBlock:
                    {
                        ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
                        if (block1 != null)
                        {
                            block1.Name = e.Label;
                        }
                        return;
                    }
                }
            }
        }

        private void tvLexer_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.UpdateTree(e.Node);
        }

        private void tvLexer_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            int num1 = this.GetNodeLevel(e.Node);
            e.CancelEdit = num1 <= 1;
        }

        private void tvLexer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node1 = this.tvLexer.GetNodeAt(e.X, e.Y);
                if (node1 != null)
                {
                    this.tvLexer.SelectedNode = node1;
                }
                this.cmLexer.MenuItems.Clear();
                NodeKind kind1 = this.GetNodeKind(this.tvLexer.SelectedNode);
                switch (kind1)
                {
                    case NodeKind.nkStyles:
                    {
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StyleText, new EventHandler(this.AddStyleClick)));
                        break;
                    }
                    case NodeKind.nkStyle:
                    {
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StyleText, new EventHandler(this.AddStyleClick)));
                        MenuItem item1 = new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.StyleText, new EventHandler(this.DeleteStyleClick));
                        item1.Enabled = !this.IsStyleUsed();
                        this.cmLexer.MenuItems.Add(item1);
                        break;
                    }
                    case NodeKind.nkStates:
                    {
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StateText, new EventHandler(this.AddStateClick)));
                        break;
                    }
                    case NodeKind.nkState:
                    {
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.StateText, new EventHandler(this.AddStateClick)));
                        MenuItem item2 = new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.StateText, new EventHandler(this.DeleteStateClick));
                        item2.Enabled = !this.IsStateUsed();
                        this.cmLexer.MenuItems.Add(item2);
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.BlockText, new EventHandler(this.AddBlockClick)));
                        break;
                    }
                    case NodeKind.nkSyntaxBlock:
                    case NodeKind.nkResWordSet:
                    {
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.BlockText, new EventHandler(this.AddBlockClick)));
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.BlockText, new EventHandler(this.DeleteBlockClick)));
                        this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.AddText + " " + SyntaxBuilderConsts.ResWordSetText, new EventHandler(this.AddResWordSetClick)));
                        if (kind1 == NodeKind.nkResWordSet)
                        {
                            this.cmLexer.MenuItems.Add(new MenuItem(SyntaxBuilderConsts.DeleteText + " " + SyntaxBuilderConsts.ResWordSetText, new EventHandler(this.DeleteResWordSetClick)));
                        }
                        break;
                    }
                }
                this.cmLexer.Show(this.tvLexer, new Point(e.X, e.Y));
            }
        }

        private void UpdateBlockExpressions()
        {
            ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            if (block1 != null)
            {
                block1.Expressions = new string[0];
                for (int num1 = 0; num1 < this.lbBlockExpr.Items.Count; num1++)
                {
                    block1.AddExpression(this.lbBlockExpr.Items[num1].ToString());
                }
            }
        }

        private void UpdateFontStyle()
        {
            NodeKind kind1 = this.GetNodeKind(this.tvLexer.SelectedNode);
            if ((kind1 == NodeKind.nkStyle) || (kind1 == NodeKind.nkStyles))
            {
                this.updating = true;
                try
                {
                    ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
                    if (style1 != null)
                    {
                        style1.FontStyle = FontStyle.Regular;
                        if (this.chbBold.Checked)
                        {
                            style1.FontStyle |= FontStyle.Bold;
                        }
                        if (this.chbItalic.Checked)
                        {
                            style1.FontStyle |= FontStyle.Italic;
                        }
                        if (this.chbUnderline.Checked)
                        {
                            style1.FontStyle |= FontStyle.Underline;
                        }
                        if (this.chbStrikeout.Checked)
                        {
                            style1.FontStyle |= FontStyle.Strikeout;
                        }
                    }
                }
                finally
                {
                    this.updating = false;
                }
                this.UpdateSample();
            }
        }

        private void UpdateGeneralPanel()
        {
            this.pnGeneral.Visible = true;
            this.tbAuthor.Text = this.Scheme.Author;
            this.tbDescription.Text = this.Scheme.Desc;
            this.tbCopyright.Text = this.Scheme.Copyright;
            this.tbSchemeName.Text = this.Scheme.Name;
        }

        private void UpdateImages()
        {
            TreeNode node1 = this.GetFolderNode();
            if (node1 == this.rootNode)
            {
                this.generalNode.ImageIndex = 2;
            }
            else
            {
                this.generalNode.ImageIndex = 3;
            }
            if (node1 == this.stylesNode)
            {
                this.stylesNode.ImageIndex = 0;
            }
            else
            {
                this.stylesNode.ImageIndex = 1;
            }
            if (node1 == this.statesNode)
            {
                this.statesNode.ImageIndex = 0;
            }
            else
            {
                this.statesNode.ImageIndex = 1;
            }
            if (this.stylesNode.Nodes.Count > 0)
            {
                if (this.stylesNode == this.tvLexer.SelectedNode)
                {
                    this.stylesNode.Nodes[0].ImageIndex = 2;
                }
                else
                {
                    this.stylesNode.Nodes[0].ImageIndex = 3;
                }
            }
        }

        private void UpdateLexStyles()
        {
            this.cbStyle.Items.Clear();
            this.cbResWordStyle.Items.Clear();
            for (int num1 = 0; num1 < this.Scheme.Styles.Length; num1++)
            {
                if (this.Scheme.Styles[num1].Name != string.Empty)
                {
                    this.cbStyle.Items.Add(this.Scheme.Styles[num1].Name);
                    this.cbResWordStyle.Items.Add(this.Scheme.Styles[num1].Name);
                }
                else
                {
                    this.cbStyle.Items.Add("");
                    this.cbResWordStyle.Items.Add("");
                }
            }
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            this.tbStyleName.Text = (style1 != null) ? style1.Name : string.Empty;
        }

        private void UpdatePanelControls(NodeKind Kind)
        {
            switch (Kind)
            {
                case NodeKind.nkStyles:
                {
                    this.UpdateStylePanel(false);
                    return;
                }
                case NodeKind.nkStyle:
                {
                    this.UpdateStylePanel(true);
                    return;
                }
                case NodeKind.nkStates:
                case NodeKind.nkState:
                case NodeKind.nkSyntaxBlock:
                case NodeKind.nkResWordSet:
                {
                    this.UpdateStatePanel();
                    return;
                }
                case NodeKind.nkGeneral:
                {
                    this.UpdateGeneralPanel();
                    return;
                }
            }
        }

        private void UpdateReswords()
        {
            ILexResWordSet set1 = this.GetLexResWordSet(this.tvLexer.SelectedNode, this.Scheme);
            if (set1 != null)
            {
                set1.ResWords = new string[0];
                for (int num1 = 0; num1 < this.lbReswords.Items.Count; num1++)
                {
                    set1.AddResWord(this.lbReswords.Items[num1].ToString());
                }
            }
            this.cbResWordStyle.Enabled = (set1 != null) && (set1.ResWords.Length > 0);
        }

        private void UpdateSample()
        {
            ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
            if (style1 != null)
            {
                this.laSample.Font = new Font(this.laSample.Font.Name, this.laSample.Font.Size, style1.FontStyle);
            }
            else
            {
                this.laSample.Font = new Font(this.laSample.Font.Name, this.laSample.Font.Size, FontStyle.Regular);
            }
            this.laSample.ForeColor = this.clbForeColor.SelectedColor;
            this.laSample.BackColor = this.clbBkColor.SelectedColor;
            int num1 = (this.pnSample.Width - this.laSample.Width) / 2;
            int num2 = (this.pnSample.Height - this.laSample.Height) / 2;
            this.laSample.Location = new Point(num1, num2);
        }

        private void UpdateScheme()
        {
            this.cbStyle.Items.Clear();
            this.cbResWordStyle.Items.Clear();
            this.cbLeaveState.Items.Clear();
            this.stylesNode.Nodes.Clear();
            this.statesNode.Nodes.Clear();
            for (int num1 = 0; num1 < this.Scheme.Styles.Length; num1++)
            {
                this.stylesNode.Nodes.Add(new TreeNode(this.Scheme.Styles[num1].Name));
                this.cbStyle.Items.Add(this.Scheme.Styles[num1].Name);
                this.cbResWordStyle.Items.Add(this.Scheme.Styles[num1].Name);
            }
            for (int num2 = 0; num2 < this.Scheme.States.Length; num2++)
            {
                this.statesNode.Nodes.Add(new TreeNode(this.Scheme.States[num2].Name, 4, 4));
                TreeNode node1 = this.statesNode.Nodes[num2];
                for (int num3 = 0; num3 < this.Scheme.States[num2].SyntaxBlocks.Length; num3++)
                {
                    node1.Nodes.Add(new TreeNode(this.Scheme.States[num2].SyntaxBlocks[num3].Name));
                    for (int num4 = 0; num4 < this.Scheme.States[num2].SyntaxBlocks[num3].ResWordSets.Length; num4++)
                    {
                        string text1 = this.Scheme.States[num2].SyntaxBlocks[num3].ResWordSets[num4].Name;
                        if (((text1 == null) || (text1 == string.Empty)) || (text1.Trim() == ""))
                        {
                            int num5 = num4 + 1;
                            text1 = SyntaxBuilderConsts.ResWordSetText + num5.ToString();
                        }
                        node1.Nodes[num3].Nodes.Add(new TreeNode(text1));
                    }
                }
                this.cbLeaveState.Items.Add(this.Scheme.States[num2].Name);
            }
            this.tvLexer.CollapseAll();
            this.tvLexer.SelectedNode = this.generalNode;
        }

        private void UpdateStatePanel()
        {
            string[] textArray1;
            int num1;
            NodeKind kind1 = this.GetNodeKind(this.tvLexer.SelectedNode);
            this.pnStates.Visible = (kind1 != NodeKind.nkStates) || ((kind1 == NodeKind.nkStates) && (this.statesNode.Nodes.Count > 0));
            switch (kind1)
            {
                case NodeKind.nkStates:
                {
                    this.gbStateProperties.Visible = this.statesNode.Nodes.Count > 0;
                    this.gbSyntaxBlockProperties.Visible = false;
                    this.gbReswordsProperties.Visible = false;
                    break;
                }
                case NodeKind.nkState:
                {
                    this.gbStateProperties.Visible = true;
                    this.gbSyntaxBlockProperties.Visible = false;
                    this.gbReswordsProperties.Visible = false;
                    break;
                }
                case NodeKind.nkSyntaxBlock:
                {
                    this.gbStateProperties.Visible = true;
                    this.gbSyntaxBlockProperties.Visible = true;
                    this.gbReswordsProperties.Visible = false;
                    break;
                }
                case NodeKind.nkResWordSet:
                {
                    this.gbStateProperties.Visible = true;
                    this.gbSyntaxBlockProperties.Visible = true;
                    this.gbReswordsProperties.Visible = true;
                    break;
                }
            }
            ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            ILexSyntaxBlock block1 = this.GetLexSyntaxBlock(this.tvLexer.SelectedNode, this.Scheme);
            ILexResWordSet set1 = this.GetLexResWordSet(this.tvLexer.SelectedNode, this.Scheme);
            if (state1 != null)
            {
                this.chbCaseSensitive.Checked = state1.CaseSensitive;
                this.tbStateDesc.Text = state1.Desc;
                this.tbStateName.Text = state1.Name;
            }
            this.cbStyle.SelectedIndex = this.GetStyleIndex(block1);
            this.btStateDelete.Enabled = !this.IsStateUsed();
            this.cbLeaveState.SelectedIndex = this.GetLeaveStateIndex(block1);
            this.cbBlockExpr.Text = string.Empty;
            this.tbResword.Text = string.Empty;
            this.lbBlockExpr.Items.Clear();
            this.lbReswords.Items.Clear();
            if (block1 != null)
            {
                this.tbBlockName.Text = block1.Name;
                this.tbBlockDesc.Text = block1.Desc;
                textArray1 = block1.Expressions;
                for (num1 = 0; num1 < textArray1.Length; num1++)
                {
                    string text1 = textArray1[num1];
                    this.lbBlockExpr.Items.Add(text1);
                }
            }
            if (set1 != null)
            {
                this.tbResWordSetName.Text = set1.Name;
                this.cbResWordStyle.Enabled = set1.ResWords.Length > 0;
                this.cbResWordStyle.SelectedIndex = this.GetReswordStyleIndex(set1);
                textArray1 = set1.ResWords;
                for (num1 = 0; num1 < textArray1.Length; num1++)
                {
                    string text2 = textArray1[num1];
                    this.lbReswords.Items.Add(text2);
                }
            }
        }

        private void UpdateStates(int Index)
        {
            this.cbLeaveState.Items.Clear();
            for (int num1 = 0; num1 < this.Scheme.States.Length; num1++)
            {
                if (this.Scheme.States[num1].Name != null)
                {
                    this.cbLeaveState.Items.Add(this.Scheme.States[num1].Name);
                }
                else
                {
                    this.cbLeaveState.Items.Add("");
                }
            }
            if (this.cbLeaveState.Items.Count > Index)
            {
                this.cbLeaveState.SelectedIndex = Index;
            }
            ILexState state1 = this.GetLexState(this.tvLexer.SelectedNode, this.Scheme);
            this.tbStateName.Text = (state1 != null) ? state1.Name : string.Empty;
        }

        private void UpdateStylePanel(bool IsStyle)
        {
            this.pnStyles.Visible = IsStyle || (!IsStyle && (this.stylesNode.Nodes.Count > 0));
            if (this.pnStyles.Visible)
            {
                ILexStyle style1 = this.GetLexStyle(this.tvLexer.SelectedNode, this.Scheme);
                if (style1 != null)
                {
                    this.tbStyleName.Text = style1.Name;
                    this.tbStyleDesc.Text = style1.Desc;
                    this.clbForeColor.SelectedColor = style1.ForeColor;
                    this.clbBkColor.SelectedColor = style1.BackColor;
                    this.chbBold.Checked = (style1.FontStyle & FontStyle.Bold) != FontStyle.Regular;
                    this.chbItalic.Checked = (style1.FontStyle & FontStyle.Italic) != FontStyle.Regular;
                    this.chbUnderline.Checked = (style1.FontStyle & FontStyle.Underline) != FontStyle.Regular;
                    this.chbStrikeout.Checked = (style1.FontStyle & FontStyle.Strikeout) != FontStyle.Regular;
                    this.chbPlainText.Checked = style1.PlainText;
                }
                this.btStyleDelete.Enabled = !this.IsStyleUsed();
                this.UpdateSample();
            }
        }

        private void UpdateTree(TreeNode Node)
        {
            this.updating = true;
            try
            {
                this.UpdateImages();
                this.UpdateVisible(this.GetNodeKind(Node));
            }
            finally
            {
                this.updating = false;
            }
        }

        private void UpdateVisible(NodeKind Kind)
        {
            Panel panel1 = this.GetCurrentPanel(Kind);
            if (panel1 != null)
            {
                this.pnMain.Controls.Add(panel1);
            }
            for (int num1 = 0; num1 < this.pnMain.Controls.Count; num1++)
            {
                if (!this.pnMain.Controls[num1].Equals(panel1) && (this.pnMain.Controls[num1] is Panel))
                {
                    this.pnMain.Controls[num1].Visible = false;
                }
            }
            panel1.Dock = DockStyle.Fill;
            panel1.BringToFront();
            this.UpdatePanelControls(Kind);
        }


        // Properties
        public LexScheme Scheme
        {
            get
            {
                return this.lexer.Scheme;
            }
            set
            {
                this.lexer.Scheme = value;
                this.UpdateScheme();
            }
        }


        // Fields
        private Button btBlockAdd;
        private Button btBlockDelete;
        private Button btBlockEdit;
        private Button btCancel;
        private Button btClear;
        private Button btLoadScheme;
        private Button btOk;
        private Button btResWordSetAdd;
        private Button btResWordSetDelete;
        private Button btResWordSetEdit;
        private Button btSave;
        private Button btStateAdd;
        private Button btStateDelete;
        private Button btStateEdit;
        private Button btStyleAdd;
        private Button btStyleDelete;
        private Button btStyleEdit;
        public ComboBox cbBlockExpr;
        public ComboBox cbLeaveState;
        public ComboBox cbResWordStyle;
        public ComboBox cbStyle;
        private const int cCloseFolder = 1;
        private float cFontSize;
        public CheckBox chbBold;
        public CheckBox chbCaseSensitive;
        public CheckBox chbItalic;
        public CheckBox chbPlainText;
        public CheckBox chbStrikeout;
        public CheckBox chbUnderline;
        public ColorBox clbBkColor;
        public ColorBox clbForeColor;
        private ContextMenu cmExpressions;
        private ContextMenu cmLexer;
        private ContextMenu cmReswords;
        private ColorDialog colorDialog1;
        private IContainer components;
        private const int cOpenFolder = 0;
        private const int cSelItem = 2;
        private const int cState = 4;
        private const int cUnSelItem = 3;
        public GroupBox gbFontStyles;
        private GroupBox gbReswordsProperties;
        public GroupBox gbStateProperties;
        public GroupBox gbStyleProperties;
        public GroupBox gbSyntaxBlockProperties;
        private TreeNode generalNode;
        private ImageList ilButtons;
        private ImageList imLexer;
        public Label laAuthor;
        public Label laBlockDesc;
        public Label laBlockExpr;
        private Label laBlockName;
        public Label laBlockReswords;
        public Label laCopyright;
        public Label laDescription;
        public Label laLeaveState;
        private Label laResWordSetName;
        public Label laResWordStyle;
        public Label laSample;
        public Label laSchemeName;
        public Label laStateDesc;
        private Label laStateName;
        public Label laStyle;
        public Label laStyleBkColor;
        public Label laStyleDesc;
        public Label laStyleForeColor;
        public Label laStyleName;
        public ListBox lbBlockExpr;
        public ListBox lbReswords;
        private Lexer lexer;
        private MenuItem miAddBlockExpr;
        private MenuItem miAddResword;
        private MenuItem miDeleteBlockExpr;
        private MenuItem miDeleteResword;
        private OpenFileDialog openFileDialog;
        private Panel pnBlockButtons;
        private Panel pnButtons;
        public Panel pnGeneral;
        private Panel pnMain;
        private Panel pnReswordButtons;
        public Panel pnSample;
        private Panel pnStateButtons;
        public Panel pnStates;
        private Panel pnStyleButtons;
        public Panel pnStyles;
        private TreeNode rootNode;
        private SaveFileDialog saveFileDialog;
        private SyntaxBuilderEditor scriptEditor;
        private string sRemoveScroll;
        private TreeNode statesNode;
        private TreeNode stylesNode;
        public TextBox tbAuthor;
        public TextBox tbBlockDesc;
        private TextBox tbBlockName;
        public TextBox tbCopyright;
        public TextBox tbDescription;
        public TextBox tbResword;
        private TextBox tbResWordSetName;
        public TextBox tbSchemeName;
        public TextBox tbStateDesc;
        public TextBox tbStateName;
        public TextBox tbStyleDesc;
        public TextBox tbStyleName;
        private TabControl tcPanels;
        private TabPage tpGeneral;
        private TabPage tpStates;
        private TabPage tpStyles;
        private TreeView tvLexer;
        private bool updating;

        // Nested Types
        internal enum NodeKind
        {
            // Fields
            nkGeneral = 6,
            nkNone = 0,
            nkResWordSet = 7,
            nkState = 4,
            nkStates = 3,
            nkStyle = 2,
            nkStyles = 1,
            nkSyntaxBlock = 5
        }
    }
}

