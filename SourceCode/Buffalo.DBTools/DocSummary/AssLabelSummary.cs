using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Collections;
/** 
@author 
@version ����ʱ�䣺2011-12-1
��ʾ��ͼע��
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class AssLabelSummary : ShapeField
    {
        // Fields
        private Connect _FromAddin;
        private SolidBrush BackBrush = new SolidBrush(Color.White);
        private SolidBrush SumerBrush = new SolidBrush(Color.Black);

        // Methods
        public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
        {
            base.DoPaint(e, parentShape);
            if (parentShape.ParentShape is AssociationShape)
            {
                AssociationShape shape = parentShape.ParentShape as AssociationShape;
                Font font = this.GetFont(e.View);
                e.Graphics.FillRectangle(this.BackBrush, 0.18f, 0f, ((float)parentShape.BoundingBox.Width) - 0.18f, (float)parentShape.BoundingBox.Height);
                e.Graphics.DrawString(shape.Member.DocSummary, font, this.SumerBrush, (float)0.19f, (float)0.02f);
            }
        }

        private Font GetFont(DiagramClientView View)
        {
            if (View == null)
            {
                return new Font("����", 9f, FontStyle.Regular);
            }
            return new Font("����", View.Font.Size * View.ZoomFactor, FontStyle.Regular);
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
    }

 

}
