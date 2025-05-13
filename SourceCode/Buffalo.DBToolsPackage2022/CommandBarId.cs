using System;
using System.Collections.Generic;
using System.Text;

namespace Buffalo.DBTools
{
    public static class CommandBarId
    {
        public const string ClassDesignerToolbar = "Class Designer Toolbar";
        public const string ClassDesignerContextMenu = "Class Designer Context Menu";
        public const string ClassDesignerAddContextMenu = ClassDesignerContextMenu + "|Add";
        public const string ClassDiagramContextMenu = "Class Diagram Context Menu";
        public const string CodeWindowContextMenu = "Code Window";

        public const string MenuBar = "MenuBar";
        public const string EditMenu = MenuBar + "|Edit";
        public const string ViewMenu = MenuBar + "|View";
        public const string ViewOtherWindowsMenu = ViewMenu + "|Other Windows";
        public const string ClassDiagramMenu = MenuBar + "|ClassDiagram";
    }
}
