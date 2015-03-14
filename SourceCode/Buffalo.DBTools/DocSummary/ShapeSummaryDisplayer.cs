using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling.Diagrams;
using System.Drawing;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using System.Drawing.Drawing2D;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Collections;
using System.Windows.Forms;
using Buffalo.DBTools.HelperKernel;

/** 
@author 
@version 创建时间：2011-12-1
显示类图注释
*/

namespace Buffalo.DBTools.DocSummary
{
    public class ShapeSummaryDisplayer
    {


        /// <summary>
        /// 显示或隐藏注释
        /// </summary>
        /// <param name="dia">图</param>
        public static void ShowOrHideSummary(Diagram dia,Connect conn)
        {
            ShapeElementMoveableCollection nestedChildShapes = dia.NestedChildShapes;
            ClrClassShape shape = null;
            ClrEnumerationShape shape2 = null;
            ClrInterfaceShape shape3 = null;
            AssociationShape shape4 = null;
            bool flag = true;

            DBConfigInfo dbInfo = DBConfigInfo.LoadInfo(conn.GetDesignerInfo());
            SummaryShowItem showInfo = SummaryShowItem.All;

            if (dbInfo != null)
            {
                showInfo = dbInfo.SummaryShow;
            }



            foreach (ShapeElement element in (IEnumerable) nestedChildShapes)
            {
                if (element is ClrClassShape)
                {
                    shape = element as ClrClassShape;
                    if (element.NestedChildShapes.Count > 0)
                    {
                        break;
                    }
                }
            }
            if (shape != null)
            {
                ShapeFieldCollection shapeFields = shape.ShapeFields;
                int index = -1;
                for (int i = 0; i < shapeFields.Count; i++)
                {
                    if (shapeFields[i] is SummaryShape)
                    {
                        index = i;
                        break;
                    }
                }
                if (index < 0)
                {
                    flag = true;
                    SummaryShape shapeField = new SummaryShape();
                    shapeFields.Add(shapeField);
                    
                }
                else
                {
                    flag = false;
                    shapeFields.RemoveAt(index);
                }
                
                foreach (ShapeElement element2 in (IEnumerable) shape.NestedChildShapes)
                {
                    if (!(element2 is CDCompartment))
                    {
                        continue;
                    }
                    shapeFields = element2.ShapeFields;
                    index = -1;
                    for (int j = 0; j < shapeFields.Count; j++)
                    {
                        if (shapeFields[j] is MemberSummary)
                        {
                            index = j;
                            break;
                        }
                    }
                    if (index < 0)
                    {
                        if (flag)
                        {
                            MemberSummary summary = new MemberSummary();

                            summary.FromAddin = conn;
                            summary.SummaryShowInfo = showInfo;
                            shapeFields.Add(summary);
                        }
                    }
                    else if (!flag)
                    {
                        shapeFields.RemoveAt(index);
                    }
                    break;
                }
            }
            foreach (ShapeElement element3 in (IEnumerable) nestedChildShapes)
            {
                if (element3 is ClrEnumerationShape)
                {
                    shape2 = element3 as ClrEnumerationShape;
                    if (element3.NestedChildShapes.Count > 0)
                    {
                        break;
                    }
                }
            }

            if (shape2 != null)
            {
                ShapeFieldCollection fields2 = shape2.ShapeFields;
                int num4 = -1;
                for (int k = 0; k < fields2.Count; k++)
                {
                    if (fields2[k] is EnumSummary)
                    {
                        num4 = k;
                        break;
                    }
                }
                if (num4 < 0)
                {
                    if (flag)
                    {
                        EnumSummary summary2 = new EnumSummary();
                        fields2.Add(summary2);
                    }
                }
                else if (!flag)
                {
                    fields2.RemoveAt(num4);
                }
                foreach (ShapeElement element4 in (IEnumerable) shape.NestedChildShapes)
                {
                    if (!(element4 is CDCompartment))
                    {
                        continue;
                    }
                    fields2 = element4.ShapeFields;
                    num4 = -1;
                    for (int m = 0; m < fields2.Count; m++)
                    {
                        if (fields2[m] is EnumItemSummary)
                        {
                            num4 = m;
                            break;
                        }
                    }
                    if (num4 < 0)
                    {
                        if (flag)
                        {
                            EnumItemSummary summary3 = new EnumItemSummary();
                            summary3.FromAddin = conn;

                            summary3.SummaryShowInfo = showInfo;
                            fields2.Add(summary3);
                        }
                    }
                    else if (!flag)
                    {
                        fields2.RemoveAt(num4);
                    }
                    break;
                }
            }

            foreach (ShapeElement element5 in (IEnumerable) nestedChildShapes)
            {
                if (element5 is ClrInterfaceShape)
                {
                    shape3 = element5 as ClrInterfaceShape;
                    if (element5.NestedChildShapes.Count > 0)
                    {
                        break;
                    }
                }
            }
            if (shape3 != null)
            {
                ShapeFieldCollection fields3 = shape3.ShapeFields;
                int num7 = -1;
                for (int n = 0; n < fields3.Count; n++)
                {
                    if (fields3[n] is InterSummary)
                    {
                        num7 = n;
                        break;
                    }
                }
                if (num7 < 0)
                {
                    if (flag)
                    {
                        InterSummary summary4 = new InterSummary();
                        fields3.Add(summary4);
                    }
                }
                else if (!flag)
                {
                    fields3.RemoveAt(num7);
                }
                foreach (ShapeElement element6 in (IEnumerable) shape3.NestedChildShapes)
                {
                    if (!(element6 is CDCompartment))
                    {
                        continue;
                    }
                    fields3 = element6.ShapeFields;
                    num7 = -1;
                    for (int num9 = 0; num9 < fields3.Count; num9++)
                    {
                        if (fields3[num9] is MemberSummary)
                        {
                            num7 = num9;
                            break;
                        }
                    }
                    if (num7 < 0)
                    {
                        if (flag)
                        {
                            MemberSummary summary5 = new MemberSummary();

                            summary5.FromAddin = conn;
                            summary5.SummaryShowInfo = showInfo;
                            fields3.Add(summary5);
                        }
                    }
                    else if (!flag)
                    {
                        fields3.RemoveAt(num7);
                    }
                    break;
                }
            }

            foreach (ShapeElement element7 in (IEnumerable) nestedChildShapes)
            {
                if (element7 is AssociationShape)
                {
                    shape4 = element7 as AssociationShape;
                }
            }
            if (shape4 != null)
            {
                ShapeFieldCollection fields4 = shape4.Label.ShapeFields;
                int num10 = -1;
                for (int num11 = 0; num11 < fields4.Count; num11++)
                {
                    if (fields4[num11] is AssLabelSummary)
                    {
                        num10 = num11;
                        break;
                    }
                }
                if (num10 < 0)
                {
                    if (flag)
                    {
                        AssLabelSummary summary6 = new AssLabelSummary();
                        fields4.Add(summary6);
                    }
                }
                else if (!flag)
                {
                    fields4.RemoveAt(num10);
                }
            }
        }



    }
}