﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace River.Orqa.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("River.Orqa.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BLOCKQUOTE.sql
        ///{
        ///	color:#808080;
        ///	font-family:Courier New;
        ///	font-size:8pt;
        ///}
        ///DIV.query 
        ///{
        ///	background-color:#6B6CA7;
        ///	color:#FFFFFF;
        ///	font-family:Arial,Tahoma;
        ///	font-size:12pt;
        ///	font-weight:bold;
        ///	padding:2px;
        ///	margin-top:20px;
        ///}
        ///SPAN.operation
        ///{
        ///	height:16px;
        ///	padding-left:5px;
        ///	vertical-align:middle;
        ///}
        ///TABLE.plan
        ///{
        ///	margin-left:15px;
        ///	border-left:solid 1px #C0C0C0;
        ///	border-top:solid 1px #C0C0C0;
        ///	border-bottom:solid 1px #C0C0C0;
        ///}
        ///TABLE.plan TR TD
        ///{
        ///	border-right:solid 1px # [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExplainPlanStyles {
            get {
                return ResourceManager.GetString("ExplainPlanStyles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CREATE GLOBAL TEMPORARY TABLE PLAN_TABLE
        ///(
        ///	statement_id    varchar2(30),
        ///	timestamp       date,
        ///	remarks         varchar2(80),
        ///	operation       varchar2(30),
        ///	options         varchar2(30),
        ///	object_node     varchar2(128),
        ///	object_owner    varchar2(30),
        ///	object_name     varchar2(30),
        ///	object_instance number(38),
        ///	object_type     varchar2(30),
        ///	optimizer       varchar2(255),
        ///	search_columns  number,
        ///	id              number(38),
        ///	parent_id       number(38),
        ///	position        number(38),
        ///	cost   [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ExplainPlanTable {
            get {
                return ResourceManager.GetString("ExplainPlanTable", resourceCulture);
            }
        }
    }
}
