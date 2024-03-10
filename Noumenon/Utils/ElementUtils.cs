using ECommons.ImGuiMethods;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Noumenon.Utils
{
    public static class ElementUtils
    {
        public enum Alignment
        {
            Left,
            Middle,
            Right
        }

        public static void setNextItemFullWidthCol()
        {
            float remainingWidth = ImGui.GetColumnWidth() - ImGui.GetStyle().FramePadding.X * 2;
            ImGui.SetNextItemWidth(remainingWidth);
        }

        public static void alignInCol(Alignment alignment)
        {
            float buttonWidth = 22;
            setAlignInCol(alignment, buttonWidth);
        }
        public static void alignInCol(Alignment alignment, float buttonWidth)
        {
            setAlignInCol(alignment, buttonWidth);
        }

        private static void setAlignInCol(Alignment alignment, float elementWidth)
        {
            float remainingWidth = ImGui.GetColumnWidth() - ImGui.GetStyle().FramePadding.X * 2;
            float elementPosX = 0;
            switch (alignment)
            {
                case Alignment.Left:
                    elementPosX = ImGui.GetCursorPosX();
                    break;
                case Alignment.Middle:
                    elementPosX = ImGui.GetCursorPosX() + (ImGui.GetColumnWidth() - elementWidth) / 2;
                    break;
                case Alignment.Right:
                    elementPosX = ImGui.GetCursorPosX() + remainingWidth - elementWidth;
                    break;
                default:
                    Exception e = new Exception("wtf is wrong with me");
                    break;
            }
            ImGui.SetCursorPosX(elementPosX);
        }

    }
}
