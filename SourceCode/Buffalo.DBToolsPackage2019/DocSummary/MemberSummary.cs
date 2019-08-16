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
using Buffalo2015.DBToolsPackage;
/** 
@author 
@version 创建时间：2011-12-1
显示类图注释
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class MemberSummary : ShapeField
    {
        // Fields
        private BuffaloToolCDCommand _FromAddin;
        private SolidBrush BackBrush = new SolidBrush(Color.White);
        private SolidBrush VarBrush = new SolidBrush(Color.Blue);
        private SolidBrush NameBrush = new SolidBrush(Color.Black);
        private SolidBrush SummaryBrush = new SolidBrush(Color.Green);
        private SolidBrush ModifierBrush = new SolidBrush(Color.Blue);
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
                float MemberStartMargin = 0f;
                float stringMargin = 0.02f;
                if (listField != null)
                {
                    int itemCount = compartment.GetItemCount(listField);
                    for (int i = 0; i < itemCount; i++)
                    {
                        ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
                        MemberStartMargin = (float)listField.GetItemRectangle(parentShape,i,0).Y;
                        compartment.GetItemDrawInfo(listField, i, itemDrawInfo);
                        if (itemDrawInfo.Disabled)
                        {
                            continue;
                        }
                        string[] strArray = itemDrawInfo.Text.Split(new char[] { ':', '(', '{', '<' });

                        Member menberByName = this.GetMenberByName(parentShape.ParentShape, strArray[0].Trim());
                        if ((menberByName == null))
                        {
                            continue;
                        }
                        string docSummary = menberByName.DocSummary;
                        string genericTypeName = "";

                        genericTypeName = menberByName.MemberTypeShortName;
                        
                        BackBrush.Color = Color.White;
                        VarBrush.Color = Color.Blue;
                        ModifierBrush.Color=Color.Blue;
                        NameBrush.Color =Color.Black;
                        SummaryBrush.Color =Color.Green;
                        SelectedShapesCollection seleShapes = this._FromAddin.SelectedShapes;
                        if (seleShapes != null)
                        {
                            foreach (DiagramItem item in seleShapes)
                            {
                                if (((item.Shape == compartment) && (item.Field == listField)) && (item.SubField.SubFieldHashCode == i))
                                {
                                    this.BackBrush.Color = SystemColors.Highlight;
                                    VarBrush.Color = Color.White;
                                    NameBrush.Color = Color.White;
                                    SummaryBrush.Color = Color.White;
                                    ModifierBrush.Color=Color.White;
                                    break;
                                }
                            }
                        }

                        
                        
                        float recX = (float)itemDrawInfo.ImageMargin;//0.16435f
                        RectangleD bound = parentShape.BoundingBox;

                        float width = (float)bound.Width;
                        //float MemberStartMargin = 0.26f;
                        
                        
                        e.Graphics.FillRectangle(this.BackBrush, VSConfigManager.CurConfig.MemberMarginX,
                            MemberStartMargin, width,
                            MemberLineHeight);

                        float curX=VSConfigManager.CurConfig.MemberMarginX;
                        
                        string memberTypeName=menberByName.MemberTypeName.TrimEnd('?','[',']');
                        if (BackBrush.Color == Color.White)//非选中状态
                        {
                            if ((memberTypeName!="void") &&(menberByName.MemberTypeLookupName == memberTypeName))
                            {
                                VarBrush.Color = Color.DodgerBlue;
                            }
                            //if (!menberByName.IsSpecialName) 
                            //{
                            //    VarBrush.Color = Color.DodgerBlue;
                            //}
                        }
                        string curStr = null;
                        
                        if (EnumUnit.ContainerValue((int)_summaryShowInfo, (int)SummaryShowItem.TypeName))
                        {

                            if (menberByName.IsStatic)
                            {
                                curStr = "static";
                                e.Graphics.DrawString(curStr, font, ModifierBrush, curX,
                                    MemberStartMargin + stringMargin);
                                curX += e.Graphics.MeasureString(curStr, font).Width;
                            }


                            curStr = genericTypeName + " ";
                            e.Graphics.DrawString(curStr, font, this.VarBrush, curX,
                                MemberStartMargin + stringMargin);
                            curX += e.Graphics.MeasureString(curStr, font).Width;
                        }
                        if (EnumUnit.ContainerValue((int)_summaryShowInfo, (int)SummaryShowItem.MemberName))
                        {
                            curStr = menberByName.Name;
                            if (menberByName is ClrMethod)
                            {
                                curStr += "()";
                            }


                            e.Graphics.DrawString(curStr, font, this.NameBrush, curX,
                                MemberStartMargin + stringMargin);
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
                            curStr += HttpUtility.HtmlDecode(docSummary);
                            e.Graphics.DrawString(curStr, font, this.SummaryBrush, curX,
                                MemberStartMargin + stringMargin);
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
            ClrType associatedType = null;
            if (clsShape is ClrClassShape)
            {
                associatedType = (clsShape as ClrClassShape).AssociatedType;
            }
            if (clsShape is ClrInterfaceShape)
            {
                associatedType = (clsShape as ClrInterfaceShape).AssociatedType;
            }
            if (associatedType != null)
            {
                foreach (Member member in (IEnumerable)associatedType.Members)
                {
                    if (member.Name == Mname)
                    {
                        return member;
                    }
                }
            }
            return null;
        }
        public SummaryShowItem SummaryShowInfo
        {
            get { return _summaryShowInfo; }
            set { _summaryShowInfo = value; }
        }
        // Properties
        public BuffaloToolCDCommand FromAddin
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
    }


}
