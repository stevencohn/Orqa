namespace River.Orqa.Editor
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct UndoData
    {
        public UndoOperation operation;
        public UpdateReason reason;
        public int updateCount;
        public bool undoFlag;
        public object data;
        public Point position;
        public UndoData(UndoOperation Operation, object Data)
        {
            this.operation = Operation;
            this.data = Data;
            this.reason = UpdateReason.Other;
            this.updateCount = 0;
            this.undoFlag = false;
            this.position = Point.Empty;
        }
    }
}

