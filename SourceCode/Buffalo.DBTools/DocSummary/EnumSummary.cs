using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using Buffalo.DBTools.DocSummary.VSConfig;
using System.Web;
/** 
@author 
@version 创建时间：2011-12-1
显示类图注释
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class EnumSummary : ShapeField
    {
        // Fields
        private float Overrideheight;
        private SolidBrush SumerBrush = new SolidBrush(Color.Maroon);
        private float summeryx = 0.03f;
        private float summeryy = VSConfigManager.CurConfig.MemberSummaryY;

        // Methods
        public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
        {
            base.DoPaint(e, parentShape);
            Font font = this.GetFont(e.View);
            string summrytxt = this.GetSummrytxt(parentShape);
            summrytxt = HttpUtility.HtmlDecode(summrytxt);
            this.Overrideheight = 0.15f;
            RectangleF rect = new RectangleF(this.summeryx, this.summeryy, ((float)parentShape.BoundingBox.Width) - (this.summeryx * 2f), this.Overrideheight);
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(0xde, 0xd7, 0xef), Color.White, 0f);
            e.Graphics.FillRectangle(brush, rect);
            
            e.Graphics.DrawString("枚举:"+summrytxt, font, this.SumerBrush, this.summeryx + 0.05f, this.summeryy);
        }

        private ClrEnumeration GetClass(ShapeElement parentShape)
        {
            if (parentShape is ClrEnumerationShape)
            {
                return (((ClrTypeShape)parentShape).AssociatedType as ClrEnumeration);
            }
            return null;
        }

        private Font GetFont(DiagramClientView View)
        {
            if (View == null)
            {
                return new Font("宋体", 9f, FontStyle.Bold);
            }
            return new Font(View.Font.FontFamily, View.Font.Size * View.ZoomFactor, FontStyle.Bold);
        }

        private string GetSummrytxt(ShapeElement parentShape)
        {
            ClrEnumeration enumeration = this.GetClass(parentShape);
            if (enumeration == null)
            {
                return "";
            }
            return enumeration.DocSummary;
        }
    }
}
