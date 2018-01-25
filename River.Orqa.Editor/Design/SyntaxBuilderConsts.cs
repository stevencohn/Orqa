namespace River.Orqa.Editor.Design
{
    using System;

    public class SyntaxBuilderConsts
    {
        // Methods
        static SyntaxBuilderConsts()
        {
            SyntaxBuilderConsts.SyntaxFormCaption = "Syntax Scheme Builder";
            SyntaxBuilderConsts.ResWordText = "ResWord";
            SyntaxBuilderConsts.StyleText = "Style";
            SyntaxBuilderConsts.StateText = "State";
            SyntaxBuilderConsts.BlockText = "Syntax Block";
            SyntaxBuilderConsts.ResWordSetText = "ResWord Set";
            SyntaxBuilderConsts.AddText = "Add";
            SyntaxBuilderConsts.EditText = "Edit";
            SyntaxBuilderConsts.DeleteText = "Delete";
            SyntaxBuilderConsts.NewReswordText = "NewResword";
            SyntaxBuilderConsts.NewExprText = "NewExpression";
            SyntaxBuilderConsts.IncompleteLeaveState = "State {0}, Syntax block {1} : LeaveState is empty";
            SyntaxBuilderConsts.IncompleteLeaveStyle = "State {0}, Syntax block {1} : Style is empty";
            SyntaxBuilderConsts.IncompleteResWordStyle = "State {0}, Syntax block {1}, ResWordSet {2} : ReswordStyle is empty";
            SyntaxBuilderConsts.InvalidScheme = "Scheme is incomplete";
            SyntaxBuilderConsts.QueryInvalidScheme = "Scheme is incomplete. Save changes?";
        }

        public SyntaxBuilderConsts()
        {
        }


        // Fields
        public static string AddText;
        public static string BlockText;
        public const string CancelCaption = "Confirmation";
        public const string CancelText = "Exit without changes?";
        public static string DeleteText;
        public static string EditText;
        public static string IncompleteLeaveState;
        public static string IncompleteLeaveStyle;
        public static string IncompleteResWordStyle;
        public static string InvalidScheme;
        public static string NewExprText;
        public static string NewReswordText;
        public static string QueryInvalidScheme;
        public static string ResWordSetText;
        public static string ResWordText;
        public static string StateText;
        public static string StyleText;
        public static string SyntaxFormCaption;
    }
}

