using Autodesk.AutoCAD.Geometry;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using ClassLibrary1;
using System;
using System.Linq;
using System.Windows.Forms;

namespace PipeExample
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        private Document _document;
        private UIDocument _uiDocument;
        private Pipe _pipeNeedRotate;
        private Pipe _pipeAxis;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            _document = commandData.Application.ActiveUIDocument.Document;
            _uiDocument = commandData.Application.ActiveUIDocument;
            Element element = SelectionPiepe();

            if (element is Pipe pipeNeedRotate) {
                Element theSecondElem = SelectPipeIsVector();
                if (theSecondElem == null) {
                    return Result.Cancelled;
                }
                Pipe pipeAxis = theSecondElem as Pipe;
                _pipeNeedRotate = pipeNeedRotate;
                _pipeAxis = pipeAxis;

                FormInput formInput = new FormInput();
                formInput.Rotate += RotatePipe;
                formInput.ShowDialog();
            }
            else {
                MessageBox.Show("Please! Select one element is Pipe.");
            }

            return Result.Succeeded;
        }

        private Element SelectPipeIsVector()
        {
            var refere = _uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select the second pipe is axis rotate.");
            if (refere == null) {
                return null;
            }
            else {
                Element element = _document.GetElement(refere.ElementId);
                if (element is Pipe) {
                    return element;
                }
                else {
                    return SelectPipeIsVector();
                }
            }
        }

        private Element SelectionPiepe()
        {
            var elementIds = _uiDocument.Selection.GetElementIds();
            if (elementIds.Count != 1) {
                MessageBox.Show("Please! Only select one element for rotation.");
                return null;
            }
            return _document.GetElement(elementIds.First());
        }

        private XYZ GetDirection(Pipe pipe)
        {
            var locationCurve = pipe.Location as LocationCurve;
            Line line = locationCurve.Curve as Line;
            return line.Direction;
        }

        private XYZ GetOrigin(Pipe pipe)
        {
            var locationCurve = pipe.Location as LocationCurve;
            Line line = locationCurve.Curve as Line;

            return line.Origin;
        }

        private void RotatePipe(double angle)
        {
            XYZ directionBase = GetDirection(_pipeNeedRotate);
            XYZ directionAxis = GetDirection(_pipeAxis);
            Vector3d vectorBase = new Vector3d(directionBase.X, directionBase.Y, directionBase.Z);
            Vector3d vectorAxis = new Vector3d(directionAxis.X, directionAxis.Y, directionAxis.Z);
            Vector3d vectorRotate = vectorBase.RotateBy(angle, vectorAxis);
            XYZ directionRotate = new XYZ(vectorRotate.X, vectorRotate.Y, vectorRotate.Z);
            var locationCurve = _pipeNeedRotate.Location as LocationCurve;
            Line line = locationCurve.Curve as Line;
            Line nLine = Line.CreateUnbound(GetOrigin(_pipeNeedRotate), directionRotate.Normalize());

            using (Transaction t = new Transaction(_document, "chant")) {
                try {
                    t.Start();

                    locationCurve.Curve = nLine;
                    t.Commit();
                }
                catch (Exception ex) {
                    t.RollBack();
                }
            }
        }
    }
}