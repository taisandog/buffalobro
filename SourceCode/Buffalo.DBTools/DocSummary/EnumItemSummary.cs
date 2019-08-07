using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Collections;
using Buffalo.DBTools.DocSummary.VSConfig;
using Buffalo.DBTools.HelperKernel;
using Buffalo.Kernel;
using System.Web;
/** 
@author 
@version 创建时间：2011-12-1
显示类图注释
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class EnumItemSummary : ShapeField
    {
        // Fields
        private Connect _FromAddin;
        private SolidBrush BackBrush = new SolidBrush(Color.White);
        private SolidBrush NameBrush = new SolidBrush(Color.Black);
        private SolidBrush SummaryBrush = new SolidBrush(Color.Green);

        SummaryShowItem _summaryShowInfo = SummaryShowItem.All;



        // Methods
        public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
        {
            base.DoPaint(e, parentShape);
            Font font = this.GetFont(e.View);
            CDCompartment compartment = parentShape as CDCompartment;
            if (compartment != null)
            {
                ListField listField = null;
                foreach (ShapeField field2 in compartment.ShapeFields)
                {
                    if (field2 is ListField)
                    {
                        listField = field2 as ListField;
                        break;
                    }
                }
                float MemberLineHeight = (float)listField.GetItemHeight(parentShape);
                if (listField != null)
                {
                    int itemCount = compartment.GetItemCount(listField);
                    for (int i = 0; i < itemCount; i++)
                    {
                        ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
                        compartment.GetItemDrawInfo(listField, i, itemDrawInfo);
                        if (!itemDrawInfo.Disabled)
                        {
                            string[] strArray = itemDrawInfo.Text.Split(new char[] { ':', '(', '{' });
                            Member menberByName = this.GetMenberByName(parentShape.ParentShape, strArray[0].Trim());
                            if ((menberByName != null))
                            {
                                string docSummary = menberByName.DocSummary;
                                this.BackBrush.Color = Color.White;
                                this.SummaryBrush.Color = Color.Green;
                                this.NameBrush.Color = Color.Black;
                                SelectedShapesCollection seleShapes = this._FromAddin.SelectedShapes;
                                if (seleShapes != null)
                                {
                                    foreach (DiagramItem item in seleShapes)
                                    {
                                        if (((item.Shape == compartment) && (item.Field == listField)) && (item.SubField.SubFieldHashCode == i))
                                        {
                                            this.BackBrush.Color = SystemColors.Highlight;
                                            this.SummaryBrush.Color = Color.White;
                                            this.NameBrush.Color = Color.White;
                                            break;
                                        }
                                    }
                                }
                                float height = 0.19f;
                                float recX = (float)itemDrawInfo.ImageMargin;//0.16435f
                                RectangleD bound = parentShape.BoundingBox;
                                float width = (float)bound.Width;

                                
                                
                                e.Graphics.FillRectangle(this.BackBrush, 0f,
                                    (0f + MemberLineHeight * (float)i),
                                    width, MemberLineHeight);

                                DBConfigInfo dbInfo = DBConfigInfo.LoadInfo(_FromAddin.GetDesignerInfo());

                                string curStr = null;
                                float curX = 0f;
                                if (EnumUnit.ContainerValue((int)_summaryShowInfo, (int)SummaryShowItem.MemberName))
                                {
                                    curStr = menberByName.Name;
                                    
                                    e.Graphics.DrawString(curStr, font, this.NameBrush, curX,
                                        (0f + MemberLineHeight * (float)i + 0.02f));
                                    curX += e.Graphics.MeasureString(curStr, font).Width;
                                }
                                if (EnumUnit.ContainerValue((int)_summaryShowInfo, (int)SummaryShowItem.Summary))
                                {
                                    if (!string.IsNullOrEmpty(curStr))
                                    {
                                        curStr = ":";
                                    }
                                    else
                                    {
                                        curStr = "";
                                    }
                                    curStr += HttpUtility.HtmlDecode(menberByName.DocSummary);
                                    e.Graphics.DrawString(curStr, font, this.SummaryBrush, curX,
                                        (0f + MemberLineHeight * (float)i + 0.02f));
                                }
                            }
                        }
                    }
                }
            }
        }

        private Font GetFont(DiagramClientView View)
        {
            if (View == null)
            {
                return new Font("宋体", 9f, FontStyle.Regular);
            }
            return new Font("宋体", View.Font.Size * View.ZoomFactor, FontStyle.Regular);
        }

        private Member GetMenberByName(ShapeElement clsShape, string Mname)
        {
            if (clsShape is ClrEnumerationShape)
            {
                foreach (Member member in (IEnumerable)(clsShape as ClrEnumerationShape).AssociatedType.Members)
                {
                    if (member.Name == Mname)
                    {
                        return member;
                    }
                }
            }
            return null;
        }

        // Properties
        public Connect FromAddin
        {
            get
            {
                return this._FromAddin;
            }
            set
            {
                this._FromAddin = value;
            }
        }

        public SummaryShowItem SummaryShowInfo
        {
            get { return _summaryShowInfo; }
            set { _summaryShowInfo = value; }
        }
    }

 

}
