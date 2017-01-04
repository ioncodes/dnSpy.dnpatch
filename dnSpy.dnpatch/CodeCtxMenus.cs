using System;
using System.ComponentModel.Composition;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnSpy.Contracts.Controls;
using dnSpy.Contracts.Documents.Tabs.DocViewer;
using dnSpy.Contracts.Documents.TreeView;
using dnSpy.Contracts.Extension;
using dnSpy.Contracts.Menus;
using dnSpy.Contracts.Text;
using dnSpy.Contracts.ToolWindows;
using Microsoft.VisualStudio.Text;

// Adds menu items to the text editor context menu
// If you have many similar commands, it's better to create a base class and derive from
// MenuItemBase<TContext> instead of MenuItemBase, see TreeViewCtxMenus.cs for an example.

namespace dnSpy.dnpatch
{
    static class Constants
    {
        //TODO: Use your own guids
        // The first number is the order of the group, and the guid is the guid of the group,
        // see eg. dnSpy.Contracts.Menus.MenuConstants.GROUP_CTX_CODE_HEX etc
        public const string GROUP_TEXTEDITOR = "20000,3567EC95-E68E-44CE-932C-98A686FDCACF";
        public const string GROUP_TREEVIEW = "20000,77ACC18E-D8EB-483B-8D93-3581574B8891";
    }

    // This gets loaded by dnSpy and is used to add the Ctrl+Alt+Q command
    [ExportAutoLoaded]
    sealed class CommandLoader : IAutoLoaded
    {
        static readonly RoutedCommand Option1Command = new RoutedCommand("Option1Command", typeof(CommandLoader));

        [ImportingConstructor]
        CommandLoader(IWpfCommandService wpfCommandService, MySettings mySettings)
        {
            var cmds = wpfCommandService.GetCommands(ControlConstants.GUID_DOCUMENTVIEWER_UICONTEXT);
            // This command will be added to all text editors
            cmds.Add(Option1Command,
                (s, e) => mySettings.BoolOption1 = !mySettings.BoolOption1,
                (s, e) => e.CanExecute = true,
                ModifierKeys.Control | ModifierKeys.Alt, Key.Q);
        }
    }

    [ExportMenuItem(Group = Constants.GROUP_TEXTEDITOR, Order = 0)]
    sealed class TextEditorCommand1 : MenuItemBase
    {
        public override void Execute(IMenuItemContext context)
        {
            var documentViewer = GetDocumentViewer(context);
            if (documentViewer != null)
            {
                try
                {
                    var lineColumn = GetLineColumn(documentViewer.Caret.Position.VirtualBufferPosition);
                    Logger.Instance.WriteLine(lineColumn.Index + ":" + lineColumn.Text);
                    Target t = (Target) ToolWindowControl.ListView.SelectedItem;
                    if (t.Instructions != "")
                        t.Instructions += ",";
                    t.Instructions += lineColumn.Text;
                    if (t.Indices != "")
                        t.Indices += ",";
                    t.Indices += lineColumn.Index;
                    if (ToolWindowControl.ListView.SelectedItem != null)
                        ToolWindowControl.ListView.Items.Remove(ToolWindowControl.ListView.SelectedItem);
                    ToolWindowControl.ListView.Items.Add(t);
                }
                catch (ExternalException) { }
            }
        }

        public override string GetHeader(IMenuItemContext context)
        {
            var documentViewer = GetDocumentViewer(context);
            if (documentViewer == null)
                return "Add Instruction to Target";
            var lineColumn = GetLineColumn(documentViewer.Caret.Position.VirtualBufferPosition);
            return $"Add Instruction to Target {lineColumn.Index}:{lineColumn.Text}";
        }

        LineColumn GetLineColumn(VirtualSnapshotPoint point)
        {
            var line = point.Position.GetContainingLine();
            return new LineColumn(line.GetText(), line.LineNumber-1);
        }

        struct LineColumn
        {
            public string Text { get; }
            public int Index { get; }
            public LineColumn(string text, int index)
            {
                Text = text;
                Index = index;
            }
        }

        IDocumentViewer GetDocumentViewer(IMenuItemContext context)
        {
            // Only show this in the document viewer
            if (context.CreatorObject.Guid != new Guid(MenuConstants.GUIDOBJ_DOCUMENTVIEWERCONTROL_GUID))
                return null;

            return context.Find<IDocumentViewer>();
        }

        //Instruction GetSecondInstruction(TVContext context)
        //{
        //    if (context.Nodes.Length == 0)
        //        return null;
        //    var methNode = context.Nodes[0] as MethodNode;
        //    if (methNode == null)
        //        return null;
        //    var body = methNode.MethodDef.Body;
        //    if (body == null || body.Instructions.Count < 2)
        //        return null;
        //    return body.Instructions[1];
        //}

        // Only show this in the document viewer
        public override bool IsVisible(IMenuItemContext context) => context.CreatorObject.Guid == new Guid(MenuConstants.GUIDOBJ_DOCUMENTVIEWERCONTROL_GUID);
        public override bool IsEnabled(IMenuItemContext context) => GetDocumentViewer(context) != null;
    }
}