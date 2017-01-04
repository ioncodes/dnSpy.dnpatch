using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using dnlib.DotNet.Emit;
using dnSpy.Contracts.App;
using dnSpy.Contracts.Documents.TreeView;
using dnSpy.Contracts.Menus;
using dnSpy.Contracts.Text;
using dnSpy.Contracts.TreeView;

// Adds a couple of commands to the file treeview context menu.
// Since there are several commands using the same state, MenuItemBase<TContext> is used
// as the base class so the context is created once and shared by all commands.

namespace dnSpy.dnpatch
{
    sealed class TVContext
    {
        public bool SomeValue { get; }
        public DocumentTreeNodeData[] Nodes { get; }

        public TVContext(bool someValue, IEnumerable<DocumentTreeNodeData> nodes)
        {
            SomeValue = someValue;
            Nodes = nodes.ToArray();
        }
    }

    abstract class TVCtxMenuCommand : MenuItemBase<TVContext>
    {
        protected sealed override object CachedContextKey => ContextKey;
        static readonly object ContextKey = new object();

        protected sealed override TVContext CreateContext(IMenuItemContext context)
        {
            // Make sure it's the file treeview
            if (context.CreatorObject.Guid != new Guid(MenuConstants.GUIDOBJ_DOCUMENTS_TREEVIEW_GUID))
                return null;

            // Extract the data needed by the context
            var nodes = context.Find<TreeNodeData[]>();
            if (nodes == null)
                return null;
            var newNodes = nodes.OfType<DocumentTreeNodeData>();

            bool someValue = true;
            return new TVContext(someValue, newNodes);
        }
    }


    [ExportMenuItem(Header = "Create new object", Group = Constants.GROUP_TREEVIEW, Order = 50)]
    sealed class TVCommand1 : TVCtxMenuCommand
    {
        public override void Execute(TVContext context)
        {
            var node = GetPath(context);
            if (node != null)
            {
                try
                {
                    var t = new Target() {Indices = "", Instructions = "", Method = "", Path = ""};
                    if (node.NodePathName.Name.Contains("(") || node.NodePathName.Name.Contains("::"))
                    {
                        if (ToolWindowControl.ListView.SelectedItem != null)
                        {
                            t = (Target)ToolWindowControl.ListView.SelectedItem;
                        }
                        Logger.Instance.WriteLine(t.Path);
                        string method = node.NodePathName.Name;
                        if (method.Contains("::"))
                        {
                            method = method.Split(new string[] {"::"}, StringSplitOptions.None)[1];
                            method = method.Replace(":", "");
                            method = method.Replace(" ", "");
                            method = method.Split('(')[0];
                        }
                        else
                        {
                            string[] split = method.Split('.');
                            string lastPart = split.Last();
                            method = lastPart.Replace(".", "");
                            method = method.Replace(" ", "");
                            method = method.Split('(')[0];
                        }
                        t.Method = method;
                        Logger.Instance.WriteLine("Method: " + method);
                        if (ToolWindowControl.ListView.SelectedItem != null)
                            ToolWindowControl.ListView.Items.Remove(ToolWindowControl.ListView.SelectedItem);
                        Logger.Instance.WriteLine(t.Method);
                    }
                    else
                    {
                        t.Path = node.NodePathName.Name;
                        Logger.Instance.WriteLine("Path: " + node.NodePathName.Name);
                    }
                    ToolWindowControl.ListView.Items.Add(t);
                }
                catch (ExternalException) { }
            }
        }

        public override string GetHeader(TVContext context)
        {
            var node = GetPath(context);
            if (node == null)
                return string.Empty;
            return node.NodePathName.Name.Contains("(") ? $"Set Method to target {node.NodePathName.Name}" : $"Set path to target {node.NodePathName.Name}";
        }

        DocumentTreeNodeData GetPath(TVContext context)
        {
            return context.Nodes.Length == 0 ? null : context.Nodes[0];
        }

        public override bool IsEnabled(TVContext context) => GetPath(context) != null;
    }
}