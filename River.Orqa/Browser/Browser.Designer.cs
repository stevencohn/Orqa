namespace River.Orqa.Browser
{
	partial class Browser
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Browser));
			this.tabset = new River.Orqa.Controls.Controls.TabControl();
			this.objectPage = new River.Orqa.Controls.Controls.TabPage();
			this.schemaManager = new River.Orqa.Browser.SchemaManager();
			this.projectPage = new River.Orqa.Controls.Controls.TabPage();
			this.projectTree = new River.Orqa.Browser.ProjectTree();
			this.templatePage = new River.Orqa.Controls.Controls.TabPage();
			this.templateTree = new River.Orqa.Browser.TemplateTree();
			this.dockImages = new System.Windows.Forms.ImageList(this.components);
			this.objectPage.SuspendLayout();
			this.projectPage.SuspendLayout();
			this.templatePage.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabset
			// 
			resources.ApplyResources(this.tabset, "tabset");
			this.tabset.HideTabsMode = River.Orqa.Controls.Controls.TabControl.HideTabsModes.ShowAlways;
			this.tabset.Name = "tabset";
			this.tabset.SelectedIndex = 2;
			this.tabset.SelectedTab = this.templatePage;
			this.tabset.TabPages.AddRange(new River.Orqa.Controls.Controls.TabPage[] {
            this.objectPage,
            this.projectPage,
            this.templatePage});
			this.tabset.SelectionChanged += new System.EventHandler(this.DoTabIndexChanged);
			this.tabset.TabIndexChanged += new System.EventHandler(this.DoTabIndexChanged);
			// 
			// objectPage
			// 
			this.objectPage.Controls.Add(this.schemaManager);
			this.objectPage.Icon = ((System.Drawing.Icon)(resources.GetObject("objectPage.Icon")));
			resources.ApplyResources(this.objectPage, "objectPage");
			this.objectPage.Name = "objectPage";
			this.objectPage.Selected = false;
			// 
			// schemaManager
			// 
			resources.ApplyResources(this.schemaManager, "schemaManager");
			this.schemaManager.Name = "schemaManager";
			this.schemaManager.SchemaSelected += new River.Orqa.Browser.BrowserSelectorEventHandler(this.DoSchemaSelected);
			this.schemaManager.SchemataSelected += new River.Orqa.Browser.BrowserTreeEventHandler(this.DoSchemataSelected);
			this.schemaManager.Compiling += new River.Orqa.Browser.BrowserTreeEventHandler(this.DoCompiling);
			this.schemaManager.Editing += new River.Orqa.Browser.BrowserTreeEventHandler(this.DoEditing);
			this.schemaManager.Opening += new River.Orqa.Browser.BrowserTreeEventHandler(this.DoOpening);
			this.schemaManager.ShowingProperties += new River.Orqa.Browser.BrowserTreeEventHandler(this.DoShowingProperties);
			// 
			// projectPage
			// 
			this.projectPage.Controls.Add(this.projectTree);
			this.projectPage.Icon = ((System.Drawing.Icon)(resources.GetObject("projectPage.Icon")));
			resources.ApplyResources(this.projectPage, "projectPage");
			this.projectPage.Name = "projectPage";
			this.projectPage.Selected = false;
			// 
			// projectTree
			// 
			resources.ApplyResources(this.projectTree, "projectTree");
			this.projectTree.Name = "projectTree";
			this.projectTree.ScriptSelected += new River.Orqa.Browser.ScriptSelectionHandler(this.DoScriptSelected);
			this.projectTree.ScriptOpen += new River.Orqa.Browser.ScriptOpenHandler(this.DoScriptOpen);
			// 
			// templatePage
			// 
			this.templatePage.Controls.Add(this.templateTree);
			this.templatePage.Icon = ((System.Drawing.Icon)(resources.GetObject("templatePage.Icon")));
			resources.ApplyResources(this.templatePage, "templatePage");
			this.templatePage.Name = "templatePage";
			// 
			// templateTree
			// 
			resources.ApplyResources(this.templateTree, "templateTree");
			this.templateTree.Name = "templateTree";
			this.templateTree.TemplateSelected += new River.Orqa.Browser.TemplateSelectionHandler(this.DoTemplateSelected);
			// 
			// dockImages
			// 
			this.dockImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("dockImages.ImageStream")));
			this.dockImages.TransparentColor = System.Drawing.Color.Transparent;
			this.dockImages.Images.SetKeyName(0, "Properties.gif");
			// 
			// Browser
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Controls.Add(this.tabset);
			this.Name = "Browser";
			this.objectPage.ResumeLayout(false);
			this.projectPage.ResumeLayout(false);
			this.templatePage.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private River.Orqa.Controls.Controls.TabControl tabset;
		private River.Orqa.Controls.Controls.TabPage objectPage;
		private River.Orqa.Controls.Controls.TabPage templatePage;
		private TemplateTree templateTree;
		private SchemaManager schemaManager;
		private System.Windows.Forms.ImageList dockImages;
		private River.Orqa.Controls.Controls.TabPage projectPage;
		private ProjectTree projectTree;
	}
}
