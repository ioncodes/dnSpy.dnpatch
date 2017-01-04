using dnSpy.Contracts.App;
using dnSpy.Contracts.Menus;
using dnSpy.Contracts.Text;

namespace dnSpy.dnpatch
{
    static class MainMenuConstants
    {
        public const string APP_MENU_EXTENSION = "98E37989-651B-41F1-B812-442C8AD434E3";
        public const string GROUP_EXTENSION_MENU1 = "0,90EAAD01-639F-474B-9D96-C93A12418B59";
        public const string GROUP_EXTENSION_MENU2 = "10,3E412D5B-6B3D-4A2B-B7E1-F09922EA5597";
    }

    // Create the Extension menu and place it right after the Debug menu
    [ExportMenu(OwnerGuid = MenuConstants.APP_MENU_GUID, Guid = MainMenuConstants.APP_MENU_EXTENSION, Order = MenuConstants.ORDER_APP_MENU_DEBUG + 0.1, Header = "_dnpatch")]
    sealed class DebugMenu : IMenu
    {
    }

    [ExportMenuItem(OwnerGuid = MainMenuConstants.APP_MENU_EXTENSION, Header = "Initialize", Group = MainMenuConstants.GROUP_EXTENSION_MENU1, Order = 0)]
    sealed class ExtensionCommand1 : MenuItemBase
    {
        public override void Execute(IMenuItemContext context)
        {
            Logger.Instance.WriteLine("Initialized.");
        }
    }

    [ExportMenuItem(OwnerGuid = MainMenuConstants.APP_MENU_EXTENSION, Header = "Stop", Group = MainMenuConstants.GROUP_EXTENSION_MENU1, Order = 0)]
    sealed class ExtensionCommand2 : MenuItemBase
    {
        public override void Execute(IMenuItemContext context)
        {
            Logger.Instance.WriteLine("Stopped");
        }
    }
}