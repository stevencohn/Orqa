#pragma warning disable 0108

namespace River.Orqa.Editor.Syntax
{
    using System;
    using System.Collections;
    using System.Reflection;

    public interface IListMembers : ICodeCompletionProvider, IList, ICollection, IEnumerable
    {
        // Methods
        IListMember AddMember();

        IListMember InsertMember(int Index);

        void ResetShowHints();

        void ResetShowParams();

        void ResetShowQualifiers();

        void ResetShowResults();


        // Properties
        IListMember this[int Index] { get; }

        bool ShowHints { get; set; }

        bool ShowParams { get; set; }

        bool ShowQualifiers { get; set; }

        bool ShowResults { get; set; }

    }
}

