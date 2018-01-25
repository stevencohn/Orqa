namespace River.Orqa.Editor.Design
{
    using River.Orqa.Editor.Syntax;
    using System;
    using System.CodeDom;
    using System.ComponentModel;
    using System.ComponentModel.Design;
    using System.ComponentModel.Design.Serialization;
    using System.Resources;

    public class LexerSerializer : CodeDomSerializer
    {
        // Methods
        public LexerSerializer()
        {
        }

        public override object Deserialize(IDesignerSerializationManager manager, object codeObject)
        {
            CodeDomSerializer serializer1 = (CodeDomSerializer) manager.GetSerializer(this.GetPropType().BaseType, typeof(CodeDomSerializer));
            return serializer1.Deserialize(manager, codeObject);
        }

        protected virtual string GetPropName()
        {
            return "XmlScheme";
        }

        protected virtual Type GetPropType()
        {
            return typeof(Lexer);
        }

        public override object Serialize(IDesignerSerializationManager manager, object value)
        {
            CodeDomSerializer serializer1 = (CodeDomSerializer) manager.GetSerializer(this.GetPropType().BaseType, typeof(CodeDomSerializer));
            object obj1 = serializer1.Serialize(manager, value);
            if (this.ShouldSerialize(value) && (obj1 is CodeStatementCollection))
            {
                string text1 = manager.GetName(value);
                if ((text1 != null) && (text1 != string.Empty))
                {
                    CodeStatementCollection collection1 = (CodeStatementCollection) obj1;
                    CodeExpression expression1 = base.SerializeToExpression(manager, value);
                    IDesignerHost host1 = ((Component) value).Container as IDesignerHost;
                    if (host1.RootComponent != null)
                    {
                        string text2 = manager.GetName(host1.RootComponent);
                        if ((text2 != null) && (text2 != string.Empty))
                        {
                            CodePropertyReferenceExpression expression2 = new CodePropertyReferenceExpression(expression1, this.GetPropName());
                            CodeExpression[] expressionArray1 = new CodeExpression[1] { new CodeTypeOfExpression(text2) } ;
                            expressionArray1 = new CodeExpression[1] { new CodePrimitiveExpression(text1 + ".XmlScheme") } ;
                            CodeMethodInvokeExpression expression3 = new CodeMethodInvokeExpression(new CodeObjectCreateExpression(typeof(ResourceManager), expressionArray1), "GetObject", expressionArray1);
                            CodeCastExpression expression4 = new CodeCastExpression("System.String", expression3);
                            CodeAssignStatement statement1 = new CodeAssignStatement(expression2, expression4);
                            collection1.Add(statement1);
                        }
                    }
                }
            }
            return obj1;
        }

        protected bool ShouldSerialize(object value)
        {
            if (value is Lexer)
            {
                return (!((Lexer) value).Scheme.IsEmpty() & ((Lexer) value).ShouldSerializeToCode());
            }
            return false;
        }

    }
}

