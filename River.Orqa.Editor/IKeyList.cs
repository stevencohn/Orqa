namespace River.Orqa.Editor
{
    using System;
    using System.Windows.Forms;

    public interface IKeyList
    {
        // Methods
        void Add(Keys KeyData, KeyEvent Action);

        void Add(Keys KeyData, KeyEventEx Action, object Param);

        void Add(Keys KeyData, KeyEvent Action, int State, int LeaveState);

        void Add(Keys KeyData, KeyEventEx Action, object Param, int State, int LeaveState);

        void AddNormal(Keys KeyData, KeyEvent Action);

        void AddNormal(Keys KeyData, KeyEventEx Action, object Param);

        void Clear();

        bool ExecuteKey(Keys KeyData, ref int State);

        bool FindKey(Keys KeyData, int State);

        void Remove(Keys KeyData);

        void Remove(Keys KeyData, int State);

    }
}

