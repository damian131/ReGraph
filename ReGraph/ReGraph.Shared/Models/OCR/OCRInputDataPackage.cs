using ReGraph.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.Models.OCR
{
    public class OCRInputDataPackage
    {
		public OCRInputDataPackage(IGraphSpace InputGraph, OCRTypeResult ResultType)
		{
			this.InputGraph = InputGraph;
			this.ResultType = ResultType;
		}

		public IGraphSpace InputGraph { get; private set; }
		public OCRTypeResult ResultType { get; private set; }
    }
}
